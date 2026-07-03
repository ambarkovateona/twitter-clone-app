import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const snackBar = inject(MatSnackBar);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
    const message = error.error?.detail ?? 'An unexpected error occurred. Please try again.';
     snackBar.open(`⚠ ${message}`, 'Close', {
  duration: 5000,
  panelClass: ['app-snackbar-error']
});
      return throwError(() => error);
    })
  );
};