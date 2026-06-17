import { EventsHandler, IEventHandler } from '@nestjs/cqrs';
import { NotificationStatus } from 'src/domain/enums';
import { INotificationLogRepository } from 'src/domain/repositories';
import { EmailService } from 'src/infrastructure/channels/email/services';
import { EmailRequestedEvent } from './email-requested.event';

@EventsHandler(EmailRequestedEvent)
export class EmailRequestedHandler implements IEventHandler<EmailRequestedEvent> {
  constructor(
    private readonly repository: INotificationLogRepository,
    private readonly emailService: EmailService,
  ) {}

  async handle(event: EmailRequestedEvent): Promise<void> {
    try {
      await this.emailService.sendEmail(
        event.recipientEmail,
        event.event,
        event.payload,
      );

      await this.repository.updateStatus(
        event.notificationId,
        NotificationStatus.SUCCESS,
      );
    } catch (error) {
      await this.repository.updateStatus(
        event.notificationId,
        NotificationStatus.FAILED,
        JSON.stringify(error),
      );
    }
  }
}
