import {
  assertInInjectionContext,
  inject,
  InjectionToken,
} from '@angular/core';
import { EnvironmentConfig } from './environment-config';

export const ENVIRONMENT_CONFIG = new InjectionToken<EnvironmentConfig>('TPhoneShop.config');

export const provideEnvironmentConfig = (value: EnvironmentConfig) => ({
  provide: ENVIRONMENT_CONFIG,
  useValue: value,
});

export const injectEnvironmentConfig = () => {
  assertInInjectionContext(injectEnvironmentConfig);
  return inject(ENVIRONMENT_CONFIG);
};
