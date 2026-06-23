import { bootstrapApplication } from '@angular/platform-browser';
import { createAppConfig } from './app/app.config';
import { RemoteEntry } from './app/remote-entry/entry';
import { EnvironmentConfig } from '@tphone-shop.web/environment-config';

fetch('config/env-config.json')
  .then((res) => res.json())
  .then((config: EnvironmentConfig) =>
    bootstrapApplication(RemoteEntry, createAppConfig(config)).catch((err) =>
      console.error(err),
    ),
  );
