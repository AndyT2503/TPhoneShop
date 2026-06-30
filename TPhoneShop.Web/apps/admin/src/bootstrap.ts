import { createAppBrowserConfig } from '@tphone-shop.web/app-config';
import { bootstrapApplication } from '@angular/platform-browser';
import { EnvironmentConfig } from '@tphone-shop.web/environment-config';
import { appRoutes } from './app/app.routes';
import { RemoteEntry } from './app/remote-entry/entry';

fetch('config/env-config.json')
  .then((res) => res.json())
  .then((config: EnvironmentConfig) => {
    bootstrapApplication(
      RemoteEntry,
      createAppBrowserConfig(config, appRoutes),
    ).catch((err) => console.error(err));
  });
