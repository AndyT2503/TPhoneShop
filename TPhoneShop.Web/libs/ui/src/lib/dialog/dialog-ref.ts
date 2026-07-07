import { OverlayRef } from '@angular/cdk/overlay';
import { Observable, Subject } from 'rxjs';

export class DialogRef<T = unknown> {
  private readonly afterClosedSubject = new Subject<T | undefined>();
  readonly afterClosed: Observable<T | undefined> = this.afterClosedSubject.asObservable();

  constructor(private readonly overlayRef: OverlayRef) {}

  close(result?: T): void {
    this.overlayRef.dispose();
    this.afterClosedSubject.next(result);
    this.afterClosedSubject.complete();
  }

  dismiss(): void {
    this.close(undefined);
  }
}
