import {
  HttpInterceptorFn,
  HttpErrorResponse
} from '@angular/common/http';

import { inject } from '@angular/core';

import { Router } from '@angular/router';

import { catchError } from 'rxjs/operators';

import { throwError } from 'rxjs';

import { AuthService } from '../services/auth.service';

import { ToastrService } from 'ngx-toastr';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {

  const authService = inject(AuthService);

  const router = inject(Router);

  const toastr = inject(ToastrService);

  const token = authService.getToken();

  let authReq = req;

  // Add token only if available
  if (token) {

    authReq = req.clone({

      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(authReq).pipe(

    catchError((error: HttpErrorResponse) => {

      // ONLY handle auth errors for protected APIs
      const protectedRoutes = [
        '/api/orders',
        '/api/Auth/profile',
        '/api/Auth/change-role',
        '/api/Auth/users'
      ];

      const isProtectedApi = protectedRoutes.some(route =>
        req.url.includes(route)
      );

      // Unauthorized
      if (
        error.status === 401 &&
        token &&
        isProtectedApi
      ) {

        toastr.error(
          'Session expired. Please login again.',
          'Unauthorized'
        );

        authService.logout();

        router.navigate(['/login']);
      }

      // Forbidden
      else if (
        error.status === 403 &&
        isProtectedApi
      ) {

        toastr.error(
          'You do not have permission to access this resource.',
          'Forbidden'
        );

        router.navigate(['/home']);
      }

      return throwError(() => error);
    })
  );
};