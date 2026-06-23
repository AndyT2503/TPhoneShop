import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners
} from '@angular/core';
import {
  provideRouter,
  Routes,
  withComponentInputBinding,
} from '@angular/router';
import {
  EnvironmentConfig,
  provideEnvironmentConfig
} from '@tphone-shop.web/environment-config';

export const createAppBrowserConfig = (
  config: EnvironmentConfig,
  appRoutes: Routes,
): ApplicationConfig => ({
  providers: [
    ...createBaseAppConfig(appRoutes).providers,
    provideEnvironmentConfig(config),
  ],
});

export const createBaseAppConfig = (
  appRoutes: Routes,
): ApplicationConfig => ({
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(appRoutes, withComponentInputBinding()),
  ],
});
