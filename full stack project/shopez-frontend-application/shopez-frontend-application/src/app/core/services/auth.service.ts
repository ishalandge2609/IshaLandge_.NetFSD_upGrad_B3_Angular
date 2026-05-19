import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { RegisterRequest, LoginRequest, LoginResponse, AuthResponse } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;
  private readonly TOKEN_KEY = 'shopez_token';
  private readonly EMAIL_KEY = 'shopez_email';
  private readonly ROLE_KEY = 'shopez_role';

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/api/Auth/register`, data);
  }

  login(data: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/api/Auth/login`, data);
  }

  getProfile(): Observable<any> {
    return this.http.get(`${this.apiUrl}/api/Auth/profile`);
  }

  changeRole(userId: number, data: { role: string }): Observable<any> {
    return this.http.put(`${this.apiUrl}/api/Auth/change-role/${userId}`, data);
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.EMAIL_KEY);
    localStorage.removeItem(this.ROLE_KEY);
  }

  setSession(token: string, email: string, role: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
    localStorage.setItem(this.EMAIL_KEY, email);
    localStorage.setItem(this.ROLE_KEY, role);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  getEmail(): string | null {
    return localStorage.getItem(this.EMAIL_KEY);
  }

  getRole(): string | null {
    return localStorage.getItem(this.ROLE_KEY);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  isAdmin(): boolean {
    return this.getRole() === 'Admin';
  }
}