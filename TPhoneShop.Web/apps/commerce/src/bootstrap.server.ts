import {
  BootstrapContext,
  bootstrapApplication,
} from '@angular/platform-browser';
import { config } from './app/app.config.server';
import { RemoteEntry } from './app/remote-entry/entry';

const bootstrap = (context: BootstrapContext) =>
  bootstrapApplication(RemoteEntry, config, context);

export default bootstrap;
