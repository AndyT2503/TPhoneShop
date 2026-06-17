import { CommandHandler, EventBus, ICommandHandler } from '@nestjs/cqrs';
import { SendEmailCommand } from './send-email.command';
import { NotificationChannel, NotificationStatus } from 'src/domain/enums';
import { INotificationLogRepository } from 'src/domain/repositories';
import { EmailRequestedEvent } from 'src/application/events/email-requested/email-requested.event';

@CommandHandler(SendEmailCommand)
export class SendEmailCommandHandler implements ICommandHandler<SendEmailCommand> {
  constructor(
    private readonly repository: INotificationLogRepository,
    private readonly eventBus: EventBus,
  ) {}

  async execute(command: SendEmailCommand): Promise<void> {
    const notification = await this.repository.create({
      recipientId: command.recipientId,
      event: command.event,
      payload: command.payload,
      channel: NotificationChannel.EMAIL,
      status: NotificationStatus.PENDING,
      createdAt: new Date(),
    });

    this.eventBus.publish(
      new EmailRequestedEvent(
        notification.id!,
        command.recipientEmail,
        command.event,
        command.payload,
      ),
    );
  }
}
