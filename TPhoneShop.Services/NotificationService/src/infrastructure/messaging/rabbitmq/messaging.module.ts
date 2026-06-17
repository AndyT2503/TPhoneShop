import { AmqpConnection, RabbitMQModule } from '@golevelup/nestjs-rabbitmq';
import { Module, OnModuleInit } from '@nestjs/common';
import { ConfigModule, ConfigService } from '@nestjs/config';
import { CqrsModule } from '@nestjs/cqrs';
import { RabbitMqConsumer } from './rabbitmq.consumer';
import { setupRabbitMQ } from './rabbitmq.setup';

@Module({
  imports: [
    CqrsModule,
    ConfigModule,
    // eslint-disable-next-line @typescript-eslint/no-unsafe-call, @typescript-eslint/no-unsafe-member-access
    RabbitMQModule.forRootAsync({
      inject: [ConfigService],
      useFactory: (config: ConfigService) => {
        const host = config.getOrThrow<string>('rabbitmq.host');
        const port = config.getOrThrow<number>('rabbitmq.port');
        const username = config.getOrThrow<string>('rabbitmq.username');
        const password = config.getOrThrow<string>('rabbitmq.password');

        return {
          uri: `amqp://${username}:${password}@${host}:${port}`,
        };
      },
    }),
  ],
  exports: [RabbitMQModule],
  providers: [RabbitMqConsumer],
})
export class MessagingModule implements OnModuleInit {
  constructor(private readonly connection: AmqpConnection) {}

  async onModuleInit() {
    const channel = this.connection.channel;
    await setupRabbitMQ(channel);
  }
}
