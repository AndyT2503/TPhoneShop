import { NotificationChannel, NotificationStatus } from '../enums';

export class NotificationLog {
  id?: string;
  recipientId!: string;
  event!: string;
  channel!: NotificationChannel;
  payload!: Record<string, unknown>;
  status!: NotificationStatus;
  sentAt?: Date;
  errorMessage?: string;
  createdAt!: Date;
}
