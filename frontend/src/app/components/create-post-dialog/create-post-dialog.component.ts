import { Component, inject } from '@angular/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PostsService } from '../../services/posts.service';
import { CreatePostFormComponent, CreatePostFormValue } from '../create-post-form/create-post-form.component';

@Component({
  selector: 'app-create-post-dialog',
  imports: [MatDialogModule, MatDividerModule, MatButtonModule, CreatePostFormComponent],
  templateUrl: './create-post-dialog.component.html'
})
export class CreatePostDialogComponent {
  private readonly postsService = inject(PostsService);
  protected readonly dialogRef = inject(MatDialogRef<CreatePostDialogComponent>);
  private readonly snackBar = inject(MatSnackBar);

  protected onCreatePost(value: CreatePostFormValue): void {
    this.postsService.createPost(value.content, value.image).subscribe({
      next: () => {
        this.dialogRef.close();
        this.snackBar.open('✓ Post created successfully!', 'Close', {
          duration: 3000,
          panelClass: ['app-snackbar-success']
        });
      },
      error: () => {}
    });
  }
}