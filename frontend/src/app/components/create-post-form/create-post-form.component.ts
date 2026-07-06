import { Component, OnDestroy, output, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { contentValidator } from '../../validators/content.validator';

export interface CreatePostFormValue {
  content: string;
  image: File | null;
}

const MAX_IMAGE_SIZE_BYTES = 5_000_000;
const ALLOWED_IMAGE_TYPES = ['image/jpeg', 'image/png', 'image/webp'];

@Component({
  selector: 'app-create-post-form',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule ,],
  templateUrl: './create-post-form.component.html',
  styleUrl: './create-post-form.component.scss'
})
export class CreatePostFormComponent implements OnDestroy {
  createPost = output<CreatePostFormValue>();

  protected readonly form = new FormGroup({
    content: new FormControl('', {
      nonNullable: true,
      validators: [contentValidator]
    })
  });

  protected readonly selectedImage = signal<File | null>(null);
  protected readonly imagePreviewUrl = signal<string | null>(null);
  protected readonly imageError = signal<string | null>(null);

  protected get contentControl() {
    return this.form.controls.content;
  }

  protected onImageSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    input.value = '';

    if (!file) {
      return;
    }

    if (!ALLOWED_IMAGE_TYPES.includes(file.type)) {
  this.imageError.set('Only JPG, PNG and WEBP images are allowed.');
  return;
}

if (file.size > MAX_IMAGE_SIZE_BYTES) {
  this.imageError.set('The image must not exceed 5MB.');
  return;
}

    this.imageError.set(null);
    this.selectedImage.set(file);
    this.imagePreviewUrl.set(URL.createObjectURL(file));
  }

  protected removeImage(): void {
    this.revokePreviewUrl();
    this.selectedImage.set(null);
    this.imagePreviewUrl.set(null);
    this.imageError.set(null);
  }

  protected onSubmit(): void {
    if (this.form.invalid || this.imageError()) {
      this.form.markAllAsTouched();
      return;
    }

    this.createPost.emit({
      content: this.form.controls.content.value.trim(),
      image: this.selectedImage()
    });

    this.removeImage();
    this.form.reset();
  }

  ngOnDestroy(): void {
    this.revokePreviewUrl();
  }

  private revokePreviewUrl(): void {
    const url = this.imagePreviewUrl();
    if (url) {
      URL.revokeObjectURL(url);
    }
  }
}