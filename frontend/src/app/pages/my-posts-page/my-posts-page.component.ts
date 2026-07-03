import { Component, inject } from '@angular/core';
import { PostsService } from '../../services/posts.service';
import { PostListComponent } from '../../components/post-list/post-list.component';
import { createReactivePostsList } from '../../utils/reactive-posts.util';

@Component({
  selector: 'app-my-posts-page',
  imports: [PostListComponent],
  templateUrl: './my-posts-page.component.html'
})
export class MyPostsPageComponent {
  private readonly postsService = inject(PostsService);

  protected readonly posts = createReactivePostsList(
    this.postsService.postsChanged$,
    () => this.postsService.getMyPosts()
  );

  protected onDeletePost(id: string): void {
    this.postsService.deletePost(id).subscribe({
      error: () => {}
    });
  }
}