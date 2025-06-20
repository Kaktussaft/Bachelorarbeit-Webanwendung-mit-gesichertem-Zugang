import {Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {Router} from '@angular/router';
import {Observable, BehaviorSubject, catchError, throwError, map} from 'rxjs';
import {environment} from '../environment/environment';

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
}

interface RegistrationResponse {
  message: string;
  success: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = environment.apiUrl;
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(
    private readonly http: HttpClient,
    private readonly router: Router
  ) {
  }

  setAuthenticated(isAuthenticated: boolean): void {
    this.isAuthenticatedSubject.next(isAuthenticated);
  }

  async checkInitialAuthtentication(): Promise<boolean> {
    const token = localStorage.getItem('loginToken');
    const isAuthenticated = !!token;
    this.isAuthenticatedSubject.next(isAuthenticated);
    return isAuthenticated;
  }

  register(userData: RegistrationRequest): Observable<RegistrationResponse> {
    return this.http.post<RegistrationResponse>(`${this.apiUrl}/Register`, userData)
      .pipe(
        catchError(this.handleError)
      );
  }

  login(credentials: LoginRequest): Observable<LoginTokenDto> {
    return this.http.post<LoginTokenDto>(`${this.apiUrl}/Login`, credentials)
      .pipe(
        map(response=>{
          localStorage.setItem('loginToken',response.accessToken);
          this.setAuthenticated(true);
          return response;
        }),
        catchError(this.handleError)
      );
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('loginToken');
    return !!token;
  }

  logout(): void {
    localStorage.removeItem('loginToken');
    this.isAuthenticatedSubject.next(false);
    this.router.navigate(['/login']);
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An error occurred';
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else {
      errorMessage = error.error.message || `Error Code: ${error.status}, Message: ${error.message}`;
    }
    return throwError(() => new Error(errorMessage));
  }

}
