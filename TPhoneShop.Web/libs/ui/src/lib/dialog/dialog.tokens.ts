import { Portal } from '@angular/cdk/portal';
import { InjectionToken } from '@angular/core';

import { DialogConfig } from './dialog-config';

export const DIALOG_DATA = new InjectionToken<unknown>('DIALOG_DATA');
export const DIALOG_CONFIG = new InjectionToken<DialogConfig>('DIALOG_CONFIG');
export const DIALOG_PORTAL = new InjectionToken<Portal<unknown>>('DIALOG_PORTAL');
