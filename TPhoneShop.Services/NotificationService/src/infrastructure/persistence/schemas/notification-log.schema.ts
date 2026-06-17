import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { Schema as MongooseSchema } from 'mongoose';
import { NotificationChannel, NotificationStatus } from 'src/domain/enums';

@Schema()
export class NotificationLogDocument {
  @Prop({ required: true })
  recipientId!: string;

  @Prop({ required: true })
  event!: string;

  @Prop({
    enum: NotificationChannel,
    required: true,
  })
  channel!: NotificationChannel;

  @Prop({
    type: MongooseSchema.Types.Mixed,
    required: true,
  })
  payload!: Record<string, any>;

  @Prop({ default: Date.now })
  createdAt!: Date;

  @Prop({ required: false })
  sentAt?: Date;

  @Prop({
    enum: NotificationStatus,
    default: NotificationStatus.PENDING,
  })
  status!: NotificationStatus;

  @Prop({ required: false })
  errorMessage?: string;
}

export const NotificationLogSchema = SchemaFactory.createForClass(
  NotificationLogDocument,
);
