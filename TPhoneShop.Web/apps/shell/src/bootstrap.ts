import { bootstrapApplication } from '@angular/platform-browser';
import { EnvironmentConfig } from '@tphone-shop.web/environment-config';
import { App } from './app/app';
import { createAppConfig } from './app/app.config';

fetch('config/env-config.json')
  .then((res) => res.json())
  .then((config: EnvironmentConfig) =>
    bootstrapApplication(App, createAppConfig(config)).catch((err) =>
      console.error(err),
    ),
  );
