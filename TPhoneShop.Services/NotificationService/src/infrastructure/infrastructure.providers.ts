import { Provider } from '@nestjs/common';
import { INotificationLogRepository } from 'src/domain/repositories';
import { NotificationLogRepository } from './persistence/repositories';
import { EmailService } from './channels/email/services';
import { EmailSender, ResendSender } from './channels/email/senders';

const repositoryProviders: Provider[] = [
  {
    provide: INotificationLogRepository,
    useClass: NotificationLogRepository,
  },
];

const emailProviders: Provider[] = [
  {
    provide: EmailSender,
    useClass: ResendSender,
  },
  EmailService,
];

export const infrastructureProviders: Provider[] = [
  ...repositoryProviders,
  ...emailProviders,
];
