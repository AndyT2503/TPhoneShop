import { bootstrapApplication } from '@angular/platform-browser';
import { EnvironmentConfig } from '@tphone-shop.web/environment-config';
import { App } from './app/app';
import { createAppBrowserConfig } from '@tphone-shop.web/app-config';
import { appRoutes } from './app/app.routes';

fetch('/config/env-config.json')
  .then((res) => res.json())
  .then((config: EnvironmentConfig) =>
    bootstrapApplication(App, createAppBrowserConfig(config, appRoutes)).catch(
      (err) => console.error(err),
    ),
  );
