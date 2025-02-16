import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { AuthClient, LoginRequest, TokenResponse } from '../../../generate-api';

@Injectable({
  providedIn: 'root'
})
export class AuthService { // 🔹 Replace with your actual API endpoint

  constructor(private authClient: AuthClient) {}

  /** Call API to authenticate user */
  login(credentials: { email: string; password: string }): Observable<TokenResponse> {
    return this.authClient.login(credentials).pipe(
      tap((response: TokenResponse) => {
       
        if (response?.token) {
          localStorage.setItem('token', response.token);
        } else {
          throw new Error('🚨 No token received');
        }
      }),
      catchError(err => {
        return throwError(() => new Error(err.error?.message || 'Invalid credentials'));
      })
    );
  }
  

  /** Check if user is authenticated */
  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return !!token; // Ensure it properly returns true when a token exists
  }
  
  /** Logout the user */
  logout() {
    localStorage.removeItem('token');
  }
}
