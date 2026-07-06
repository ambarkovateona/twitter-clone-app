import { signal, inject, DestroyRef } from '@angular/core';
import { Observable } from 'rxjs';
import { Post } from '../models/post.model';
import { PagedResult } from '../models/paged-result.model';

export function createPaginatedPostsList(
  changeNotifier: Observable<void>,
  fetchFn: (page: number, pageSize: number) => Observable<PagedResult<Post>>,
  pageSize: number = 10
) {
  const posts = signal<Post[]>([]);
  const hasMore = signal(false);
  const loading = signal(true);
  const loadingMore = signal(false);

  let currentPage = 1;

  function loadFirstPage(): void {
    loading.set(true);
    currentPage = 1;
    fetchFn(currentPage, pageSize).subscribe({
      next: result => {
        posts.set(result.items);
        hasMore.set(result.hasMore);
        loading.set(false);
      },
      error: () => {
        loading.set(false);
      }
    });
  }

  function loadMore(): void {
    if (loadingMore() || !hasMore()) {
      return;
    }
    loadingMore.set(true);
    const nextPage = currentPage + 1;
    fetchFn(nextPage, pageSize).subscribe({
      next: result => {
        posts.update(existing => [...existing, ...result.items]);
        hasMore.set(result.hasMore);
        currentPage = nextPage;
        loadingMore.set(false);
      },
      error: () => {
        loadingMore.set(false);
      }
    });
  }

  const destroyRef = inject(DestroyRef);
  const subscription = changeNotifier.subscribe(() => loadFirstPage());
  destroyRef.onDestroy(() => subscription.unsubscribe());

  loadFirstPage();

  return { posts, hasMore, loading, loadingMore, loadMore };
}