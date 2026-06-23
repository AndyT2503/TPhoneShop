import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { appRoutes } from './app.routes';
import {
  EnvironmentConfig,
  provideEnvironmentConfig,
} from '@tphone-shop.web/environment-config';

export const createAppConfig = (
  config: EnvironmentConfig,
): ApplicationConfig => ({
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(appRoutes, withComponentInputBinding()),
    provideEnvironmentConfig(config),
  ],
});
