import { Provider } from '@nestjs/common';
import { SendEmailCommandHandler } from './commands/send-email/send-email.handler';
import { EmailRequestedHandler } from './events/email-requested/email-requested.handler';

const commandHandlerProviders: Provider[] = [SendEmailCommandHandler];
const eventHandlerProviders: Provider[] = [EmailRequestedHandler];

export const applicationProviders: Provider[] = [
  ...commandHandlerProviders,
  ...eventHandlerProviders,
];
