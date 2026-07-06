import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, Subject, tap } from 'rxjs';
import { Post } from '../models/post.model';
import { PagedResult } from '../models/paged-result.model';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PostsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/posts`;

  private readonly postsChangedSource = new Subject<void>();
  readonly postsChanged$ = this.postsChangedSource.asObservable();

  getAllPosts(page: number = 1, pageSize: number = 10): Observable<PagedResult<Post>> {
    const params = new HttpParams().set('page', page).set('pageSize', pageSize);
    return this.http.get<PagedResult<Post>>(this.baseUrl, { params });
  }

  getMyPosts(page: number = 1, pageSize: number = 10): Observable<PagedResult<Post>> {
    const params = new HttpParams().set('page', page).set('pageSize', pageSize);
    return this.http.get<PagedResult<Post>>(`${this.baseUrl}/mine`, { params });
  }

  createPost(content: string, image: File | null): Observable<Post> {
    const formData = new FormData();
    formData.append('content', content);
    if (image) {
      formData.append('image', image);
    }
    return this.http.post<Post>(this.baseUrl, formData).pipe(
      tap(() => this.postsChangedSource.next())
    );
  }

  deletePost(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`).pipe(
      tap(() => this.postsChangedSource.next())
    );
  }
}