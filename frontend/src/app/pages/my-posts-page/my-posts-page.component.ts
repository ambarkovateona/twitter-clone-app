import { Component, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PostsService } from '../../services/posts.service';
import { PostListComponent } from '../../components/post-list/post-list.component';
import { ConfirmDialogComponent } from '../../components/confirm-dialog/confirm-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { createPaginatedPostsList } from '../../utils/paginated-posts.util';

@Component({
  selector: 'app-my-posts-page',
  imports: [PostListComponent, MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './my-posts-page.component.html'
})
export class MyPostsPageComponent {
  private readonly postsService = inject(PostsService);
  private readonly dialog = inject(MatDialog);

  private readonly paginated = createPaginatedPostsList(
    this.postsService.postsChanged$,
    (page, pageSize) => this.postsService.getMyPosts(page, pageSize)
  );

  protected readonly posts = this.paginated.posts;
  protected readonly loading = this.paginated.loading;
  protected readonly hasMore = this.paginated.hasMore;
  protected readonly loadingMore = this.paginated.loadingMore;
  protected readonly loadMore = this.paginated.loadMore;

  protected onDeletePost(id: string): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete post',
        message: 'Are you sure you want to delete this post? This action cannot be undone.'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.postsService.deletePost(id).subscribe({ error: () => {} });
      }
    });
  }
}