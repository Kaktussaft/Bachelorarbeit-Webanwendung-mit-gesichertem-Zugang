import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = 'YOUR_BACKEND_API_URL';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(
    private readonly http: HttpClient,
    private readonly router: Router
  ) {}

  setAuthenticated(isAuthenticated: boolean): void {
    this.isAuthenticatedSubject.next(isAuthenticated);
  }

  async checkInitialAuthtentication(): Promise<boolean> {
    const token = localStorage.getItem('loginToken');
    const isAuthenticated = !!token;
    this.isAuthenticatedSubject.next(isAuthenticated);
    return isAuthenticated;
  }

  register(userData: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, userData);
  }

  login(credentials: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials);
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
}
