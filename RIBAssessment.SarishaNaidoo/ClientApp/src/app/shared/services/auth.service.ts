import { Injectable } from '@angular/core';;
import { catchError, Observable, tap, throwError } from 'rxjs';
import { AuthClient, LoginRequest, TokenResponse } from '../../../generate-api';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  exp: number;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private logoutTimer: any;

  constructor(private authClient: AuthClient, private router: Router) { }

  /** Call API to authenticate user */
  login(credentials: { email: string; password: string }): Observable<TokenResponse> {
    return this.authClient.login(credentials).pipe(
      tap((response: TokenResponse) => {

        if (response?.token) {
          localStorage.setItem('token', response.token);
          this.scheduleAutoLogout(response.token);
        } else {
          throw new Error('No token received');
        }
      }),
      catchError(err => {
        return throwError(() => new Error(err.error?.message || 'Invalid credentials'));
      })
    );
  }

  register(credentials: { email: string; password: string }): Observable<any> {
    let LoginRequest = credentials as LoginRequest;
    return this.authClient.register(LoginRequest).pipe();
  }

  /** Check if user is authenticated */
  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return !!token; // Ensure it properly returns true when a token exists
  }

  /** Logout the user */
  logout(): void {
    localStorage.removeItem('token');
    if (this.logoutTimer) {
      clearTimeout(this.logoutTimer);
    }
    this.router.navigate(['/login']);
  }

  private scheduleAutoLogout(token: string): void {
    try {
      const decoded = jwtDecode<JwtPayload>(token);
      const expiryTime = decoded.exp * 1000;
      const currentTime = Date.now();
      const timeoutDuration = expiryTime - currentTime;

      if (this.logoutTimer) {
        clearTimeout(this.logoutTimer);
      }

      this.logoutTimer = setTimeout(() => {
        this.logout();
      }, timeoutDuration);
    } catch (error) {
      console.error('Error decoding token for auto logout:', error);
    }
  }
}
