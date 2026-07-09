import { NotificationChannel, NotificationStatus } from '../enums';

//index BTree, Hash, Inverted index
//non functional & functional
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
