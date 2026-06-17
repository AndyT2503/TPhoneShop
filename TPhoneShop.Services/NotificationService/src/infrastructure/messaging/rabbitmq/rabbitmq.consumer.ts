/* eslint-disable @typescript-eslint/no-unsafe-argument */
import {
  AmqpConnection,
  Nack,
  RabbitSubscribe,
} from '@golevelup/nestjs-rabbitmq';
import { Injectable, Logger } from '@nestjs/common';
import { CommandBus } from '@nestjs/cqrs';
import type { ConsumeMessage } from 'amqplib';
import { SendEmailCommand } from 'src/application/commands/send-email/send-email.command';
import { EXCHANGES, QUEUES, RoutingKeys } from './constants';
import { NotificationEvent } from 'src/domain/enums';

@Injectable()
export class RabbitMqConsumer {
  private readonly logger = new Logger(RabbitMqConsumer.name);

  constructor(
    private readonly commandBus: CommandBus,
    private readonly amqpConnection: AmqpConnection,
  ) {}

  // eslint-disable-next-line @typescript-eslint/no-unsafe-call
  @RabbitSubscribe({
    exchange: EXCHANGES.IDENTITY,
    routingKey: [RoutingKeys.IDENTITY_USER_FORGOT_PASSWORD],
    queue: QUEUES.EMAIL_MAIN,
    createQueueIfNotExists: false,
  })
  async handleIdentityMessage(
    payload: Record<string, any>,
    amqpMsg: ConsumeMessage,
  ) {
    try {
      const routingKey = amqpMsg.fields.routingKey as RoutingKeys;
      await this.dispatchCommand(routingKey, payload);
    } catch (error) {
      const retryCount = this.getRetryCount(amqpMsg);
      if (retryCount < 3) {
        this.logger.log(
          `Processing failed. Retrying... Attempt: ${retryCount + 1}`,
        );
        return new Nack(false);
      } else {
        const newHeaders = { ...amqpMsg.properties.headers };
        delete newHeaders['x-death'];
        await this.amqpConnection.publish(
          EXCHANGES.NOTIFICATION_DLX,
          RoutingKeys.PUSH_TO_DEAD_LETTER_QUEUE,
          amqpMsg.content,
          {
            headers: newHeaders,
            persistent: true,
          },
        );

        this.logger.log('Max retries reached. Sending to final error queue.');
        this.logger.error(error);
        return;
      }
    }
  }

  private getRetryCount(msg: ConsumeMessage): number {
    const xDeath = msg.properties.headers?.['x-death'];

    if (!xDeath || !Array.isArray(xDeath)) {
      return 0;
    }

    return xDeath[0]?.count ?? 0;
  }

  private async dispatchCommand(
    routingKey: RoutingKeys,
    payload: Record<string, any>,
  ) {
    switch (routingKey) {
      case RoutingKeys.IDENTITY_USER_FORGOT_PASSWORD:
        this.logger.log(`Received SEND_EMAIL for ${payload.email}`);
        await this.commandBus.execute(
          new SendEmailCommand(
            payload['recipientId'],
            payload['email'],
            NotificationEvent.FORGOT_PASSWORD,
            {
              token: payload['token'],
            },
          ),
        );
        break;
    }
  }
}
