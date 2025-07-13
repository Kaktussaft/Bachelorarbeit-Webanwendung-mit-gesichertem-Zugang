import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './services/authentication-service/authentication-service.service';
import {Observable, take, tap} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.authService.isAuthenticated$.pipe(
      take(1),
      tap((isAuthenticated) => {
        if (!isAuthenticated) {
          this.router.navigate(['/login'], {
            queryParams: { returnUrl: state.url },
          });
        }
      })
    );
  }
}
