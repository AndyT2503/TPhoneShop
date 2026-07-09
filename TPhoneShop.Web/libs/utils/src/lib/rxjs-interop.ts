import { assertInInjectionContext, Signal } from '@angular/core';
import { toObservable, ToObservableOptions } from '@angular/core/rxjs-interop';
import { Observable, skip } from 'rxjs';

export function toSyncObservable<T>(sourceSignal: Signal<T>, options?: ToObservableOptions): Observable<T> {
  if (ngDevMode && !options?.injector) {
    assertInInjectionContext(toSyncObservable);
  }
  const nextValues$ = toObservable(sourceSignal, options).pipe(skip(1));

  return new Observable<T>((subscriber) => {
    subscriber.next(sourceSignal());
    const subscription = nextValues$.subscribe(subscriber);
    return () => subscription.unsubscribe();
  });
}
