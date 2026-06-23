import { ApplicationConfig, mergeApplicationConfig } from '@angular/core';
import { provideServerRendering } from '@angular/ssr';
import { createBaseAppConfig } from '@tphone-shop.web/app-config';
import { appRoutes } from './app.routes';

const serverConfig: ApplicationConfig = {
  providers: [provideServerRendering()],
};

export const config = mergeApplicationConfig(
  createBaseAppConfig(appRoutes),
  serverConfig,
);
