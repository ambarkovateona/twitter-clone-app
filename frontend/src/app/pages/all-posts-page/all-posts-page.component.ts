import { Component, inject } from '@angular/core';
import { PostsService } from '../../services/posts.service';
import { PostListComponent } from '../../components/post-list/post-list.component';
import { createReactivePostsList } from '../../utils/reactive-posts.util';

@Component({
  selector: 'app-all-posts-page',
  imports: [PostListComponent],
  templateUrl: './all-posts-page.component.html'
})
export class AllPostsPageComponent {
  private readonly postsService = inject(PostsService);

  protected readonly posts = createReactivePostsList(
    this.postsService.postsChanged$,
    () => this.postsService.getAllPosts()
  );
}