import { NotificationEvent } from 'src/domain/enums';

export class EmailRequestedEvent {
  constructor(
    public readonly notificationId: string,
    public readonly recipientEmail: string,
    public readonly event: NotificationEvent,
    public readonly payload: Record<string, unknown>,
  ) {}
}
