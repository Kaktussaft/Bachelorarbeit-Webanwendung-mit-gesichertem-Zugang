import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../authentication-service/authentication-service.service';

@Injectable()
export class CSRFInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (req.method === 'GET' || !req.url.startsWith('/api/')) {
      return next.handle(req);
    }

    const csrfReq = req.clone({
      setHeaders: {
        'X-Requested-With': 'XMLHttpRequest',
      },
      withCredentials: true,
    });

    return next.handle(csrfReq);
  }
}
