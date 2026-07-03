import { toSignal } from '@angular/core/rxjs-interop';
import { Observable, startWith, switchMap } from 'rxjs';
import { Post } from '../models/post.model';

export function createReactivePostsList(
  changeNotifier: Observable<void>,
  fetchFn: () => Observable<Post[]>
) {
  return toSignal(
    changeNotifier.pipe(
      startWith(undefined),
      switchMap(() => fetchFn())
    ),
    { initialValue: null }
  );
}