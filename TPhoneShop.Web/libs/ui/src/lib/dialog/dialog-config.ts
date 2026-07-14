export interface DialogConfig<TData = unknown> {
  title?: string;
  data?: TData;
  width?: string;
  minHeight?: string;
  maxWidth?: string;
  panelClass?: string | string[];
  backdropClass?: string | string[];
  hasBackdrop?: boolean;
  disableClose?: boolean;
  showCloseButton?: boolean;
}

export const DEFAULT_DIALOG_CONFIG: DialogConfig = {
  hasBackdrop: true,
  disableClose: false,
  showCloseButton: true,
  backdropClass: 'cdk-overlay-dark-backdrop',
  panelClass: 'lib-dialog-panel',
  width: '640px',
  maxWidth: 'calc(100vw - 32px)',
};
