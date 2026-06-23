import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import {
  EnvironmentConfig,
  provideEnvironmentConfig,
} from '@tphone-shop.web/environment-config';
import { appRoutes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(appRoutes, withComponentInputBinding()),
  ],
};

export const createAppConfig = (
  config: EnvironmentConfig,
): ApplicationConfig => ({
  providers: [
    ...appConfig.providers,
    provideEnvironmentConfig(config),
  ],
});
