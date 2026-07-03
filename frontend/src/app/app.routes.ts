import { Routes } from '@angular/router';
import { AllPostsPageComponent } from './pages/all-posts-page/all-posts-page.component';
import { MyPostsPageComponent } from './pages/my-posts-page/my-posts-page.component';

export const routes: Routes = [
  { path: '', component: AllPostsPageComponent },
  { path: 'mine', component: MyPostsPageComponent }
];