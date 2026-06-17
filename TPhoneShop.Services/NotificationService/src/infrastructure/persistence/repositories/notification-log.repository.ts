import { Injectable, Scope } from '@nestjs/common';
import { InjectModel } from '@nestjs/mongoose';
import { Model } from 'mongoose';
import { NotificationStatus } from 'src/domain/enums';
import { INotificationLogRepository } from 'src/domain/repositories';
import { NotificationLogDocument } from '../schemas';
import { NotificationLog } from 'src/domain/entities';

@Injectable({
  scope: Scope.REQUEST,
})
export class NotificationLogRepository implements INotificationLogRepository {
  constructor(
    @InjectModel(NotificationLogDocument.name)
    private readonly model: Model<NotificationLogDocument>,
  ) {}

  async create(
    notification: NotificationLogDocument,
  ): Promise<NotificationLog> {
    const created = await this.model.create(notification);

    return {
      ...notification,
      id: created._id.toString(),
    };
  }

  async updateStatus(
    id: string,
    status: NotificationStatus,
    errorMessage?: string,
  ): Promise<void> {
    await this.model.updateOne(
      { _id: id },
      {
        status,
        sentAt: new Date(),
        errorMessage,
      },
    );
  }
}
