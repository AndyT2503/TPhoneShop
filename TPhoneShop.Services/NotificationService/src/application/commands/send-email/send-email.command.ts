import { NotificationEvent } from 'src/domain/enums';

export class SendEmailCommand {
  constructor(
    public readonly recipientId: string,
    public readonly recipientEmail: string,
    public readonly event: NotificationEvent,
    public readonly payload: Record<string, unknown>,
  ) {}
}
