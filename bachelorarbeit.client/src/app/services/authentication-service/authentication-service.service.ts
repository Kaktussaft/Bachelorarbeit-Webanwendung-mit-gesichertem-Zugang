import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Router } from '@angular/router';
import {
  Observable,
  BehaviorSubject,
  catchError,
  throwError,
  map,
  firstValueFrom,
} from 'rxjs';
import { environment } from '../../environment/environment';

interface LoginRequest {
  email: string;
  password: string;
}

interface RegistrationRequest {
  username: string;
  email: string;
  password: string;
}

interface LoginTokenDto {
  accessToken: string;
  refreshToken: string;
  expiration: Date;
  LoginResponse: LoginResponse;
}

interface LoginResponse {
  message: string;
  success: boolean;
}

interface RegistrationResponse {
  message: string;
  success: boolean;
}

interface CSRFTokenResponse {
  token: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = environment.apiUrl;
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
  private csrfToken: string | null = null;

  constructor(
    private readonly http: HttpClient,
    private readonly router: Router
  ) {
    this.initializeCSRFToken();
  }

  private async initializeCSRFToken(): Promise<void> {
    try {
      const response = await firstValueFrom(
        this.http.get<CSRFTokenResponse>(`${this.apiUrl}/csrf-token`)
      );
      this.csrfToken = response?.token || null;
    } catch (error) {
      console.error('Failed to get CSRF token:', error);
    }
  }

  private getCSRFHeaders(): HttpHeaders {
    return new HttpHeaders({
      'X-CSRF-Token': this.csrfToken || '',
      'Content-Type': 'application/json',
      'X-Requested-Width': 'XMLHttpRequest',
    });
  }

  private getCSRFToken(): string | null {
    if (this.csrfToken) {
      return this.csrfToken;
    }
    const metaTag = document.querySelector(
      'meta[name="csrf-token"]'
    ) as HTMLMetaElement;
    return metaTag?.getAttribute('content') || null;
  }

  async refreshCSRFToken(): Promise<void> {
    try {
      const response = await firstValueFrom(
        this.http.get<CSRFTokenResponse>(`${this.apiUrl}/csrf-token`)
      );
      this.csrfToken = response?.token || null;
    } catch (error) {
      console.error('Failed to refresh CSRF token:', error);
    }
  }

  async checkInitialAuthentication(): Promise<boolean> {
    try {
      const response = await firstValueFrom(
        this.http.get<{ isAuthenticated: boolean }>(
          `${this.apiUrl}/auth-status`,
          {
            withCredentials: true,
          }
        )
      );
      const isAuthenticated = response?.isAuthenticated || false;
      this.isAuthenticatedSubject.next(isAuthenticated);
      return isAuthenticated;
    } catch (error) {
      this.isAuthenticatedSubject.next(false);
      return false;
    }
  }

  refreshToken(): Observable<{ success: boolean }> {
    const headers = this.getCSRFHeaders();
    return this.http
      .post<{ success: boolean }>(
        `${this.apiUrl}/refresh-token`,
        {},
        {
          headers,
          withCredentials: true,
        }
      )
      .pipe(catchError(this.handleError));
  }

  register(userData: RegistrationRequest): Observable<RegistrationResponse> {
    const headers = this.getCSRFHeaders();
    return this.http
      .post<RegistrationResponse>(`${this.apiUrl}/Register`, userData, {
        headers,
        withCredentials: true,
      })
      .pipe(catchError(this.handleError));
  }

  login(credentials: LoginRequest): Observable<LoginTokenDto> {
    const headers = this.getCSRFHeaders();
    return this.http
      .post<LoginTokenDto>(`${this.apiUrl}/Login`, credentials, {
        headers,
        withCredentials: true,
      })
      .pipe(
        map((response) => {
          if (response.LoginResponse.success) {
            this.setAuthenticated(true);
          }
          return response;
        }),
        catchError(this.handleError)
      );
  }

  setAuthenticated(isAuthenticated: boolean): void {
    this.isAuthenticatedSubject.next(isAuthenticated);
  }

  isAuthenticated(): boolean {
    return this.isAuthenticatedSubject.value;
  }

  logout(): Observable<{ success: boolean }> {
    const headers = this.getCSRFHeaders();
    return this.http
      .post<{ success: boolean }>(
        `${this.apiUrl}/logout`,
        {},
        {
          headers,
          withCredentials: true,
        }
      )
      .pipe(
        map((response) => {
          this.setAuthenticated(false);
          this.router.navigate(['/login']);
          return response;
        }),
        catchError(this.handleError)
      );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An error occurred';
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else {
      errorMessage =
        error.error.message ||
        `Error Code: ${error.status}, Message: ${error.message}`;
    }
    return throwError(() => new Error(errorMessage));
  }
}
