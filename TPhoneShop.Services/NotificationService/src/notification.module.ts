import { Module } from '@nestjs/common';
import { ConfigModule, ConfigService } from '@nestjs/config';
import { CqrsModule } from '@nestjs/cqrs';
import { MongooseModule } from '@nestjs/mongoose';
import { applicationProviders } from './application/application.providers';
import configuration from './config/configuration';
import { infrastructureProviders } from './infrastructure/infrastructure.providers';
import { MessagingModule } from './infrastructure/messaging/rabbitmq/messaging.module';
import {
  NotificationLogDocument,
  NotificationLogSchema,
} from './infrastructure/persistence/schemas';

@Module({
  imports: [
    ConfigModule.forRoot({
      isGlobal: true,
      load: [configuration],
    }),
    MongooseModule.forRootAsync({
      inject: [ConfigService],
      useFactory: (configService: ConfigService) => ({
        uri: configService.get<string>('mongoUri'),
      }),
    }),
    MongooseModule.forFeature([
      {
        name: NotificationLogDocument.name,
        schema: NotificationLogSchema,
      },
    ]),
    MessagingModule,
    CqrsModule,
  ],
  controllers: [],
  providers: [...infrastructureProviders, ...applicationProviders],
})
export class NotificationModule {}
