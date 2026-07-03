import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { CreatePostDialogComponent } from './components/create-post-dialog/create-post-dialog.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, MatToolbarModule, MatButtonModule, MatIconModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  private readonly dialog = inject(MatDialog);

 protected openCreatePostDialog(): void {
  this.dialog.open(CreatePostDialogComponent, {
    width: '500px',
    maxWidth: '90vw',
    autoFocus: 'first-tabbable'
  });
}
}