import { AbstractControl, ValidationErrors } from '@angular/forms';

export function contentValidator(control: AbstractControl): ValidationErrors | null {
  const rawValue = (control.value as string) ?? '';
  const trimmedLength = rawValue.trim().length;

  if (trimmedLength === 0) {
    return { required: true };
  }
  if (trimmedLength < 12) {
    return { minlength: { requiredLength: 12, actualLength: trimmedLength } };
  }
  if (trimmedLength > 140) {
    return { maxlength: { requiredLength: 140, actualLength: trimmedLength } };
  }
  return null;
}