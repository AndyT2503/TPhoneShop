import { Channel } from 'amqplib';
import { EXCHANGES, QUEUES, RoutingKeys } from './constants';

export async function setupRabbitMQ(channel: Channel) {
  await channel.assertExchange(EXCHANGES.IDENTITY, 'direct', {
    durable: true,
  });

  await channel.assertExchange(EXCHANGES.NOTIFICATION_DLX, 'direct', {
    durable: true,
  });

  await channel.assertQueue(QUEUES.EMAIL_MAIN, {
    durable: true,
    arguments: {
      'x-dead-letter-exchange': EXCHANGES.NOTIFICATION_DLX,
      'x-dead-letter-routing-key': RoutingKeys.PUSH_TO_RETRY_QUEUE,
    },
  });

  // RETRY QUEUE
  await channel.assertQueue(QUEUES.EMAIL_RETRY, {
    durable: true,
    messageTtl: 5000,
    arguments: {
      'x-dead-letter-exchange': EXCHANGES.IDENTITY,
      'x-dead-letter-routing-key': RoutingKeys.IDENTITY_USER_FORGOT_PASSWORD,
    },
  });

  // DLQ
  await channel.assertQueue(QUEUES.EMAIL_DLX, {
    durable: true,
  });

  // BINDINGS
  await channel.bindQueue(
    QUEUES.EMAIL_MAIN,
    EXCHANGES.IDENTITY,
    RoutingKeys.IDENTITY_USER_FORGOT_PASSWORD,
  );

  await channel.bindQueue(
    QUEUES.EMAIL_RETRY,
    EXCHANGES.NOTIFICATION_DLX,
    RoutingKeys.PUSH_TO_RETRY_QUEUE,
  );

  await channel.bindQueue(
    QUEUES.EMAIL_DLX,
    EXCHANGES.NOTIFICATION_DLX,
    RoutingKeys.PUSH_TO_DEAD_LETTER_QUEUE,
  );
}
