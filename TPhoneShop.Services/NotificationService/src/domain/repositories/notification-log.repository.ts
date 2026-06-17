import { NotificationLog } from '../entities';
import { NotificationStatus } from '../enums';

export abstract class INotificationLogRepository {
  abstract create(notification: NotificationLog): Promise<NotificationLog>;

  abstract updateStatus(
    id: string,
    status: NotificationStatus,
    errorMessage?: string,
  ): Promise<void>;
}
