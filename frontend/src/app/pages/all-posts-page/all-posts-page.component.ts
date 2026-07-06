import { Component, inject } from '@angular/core';
import { PostsService } from '../../services/posts.service';
import { PostListComponent } from '../../components/post-list/post-list.component';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { createPaginatedPostsList } from '../../utils/paginated-posts.util';

@Component({
  selector: 'app-all-posts-page',
  imports: [PostListComponent, MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './all-posts-page.component.html'
})
export class AllPostsPageComponent {
  private readonly postsService = inject(PostsService);

  private readonly paginated = createPaginatedPostsList(
    this.postsService.postsChanged$,
    (page, pageSize) => this.postsService.getAllPosts(page, pageSize)
  );

  protected readonly posts = this.paginated.posts;
  protected readonly loading = this.paginated.loading;
  protected readonly hasMore = this.paginated.hasMore;
  protected readonly loadingMore = this.paginated.loadingMore;
  protected readonly loadMore = this.paginated.loadMore;
}