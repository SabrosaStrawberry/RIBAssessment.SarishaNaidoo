import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../shared/services/auth.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-user-login',
  standalone: false,
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css'] 
})
export class UserLoginComponent {
  loginForm: FormGroup;
  loading: boolean = false;
  errorMessage: string = '';
  isFormValid: boolean = false;
  isRegistration: boolean = false; 

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar,
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });

    this.loginForm.statusChanges.subscribe(status => {
      this.isFormValid = status === 'VALID';
    });

    this.loginForm.valueChanges.subscribe(() => {
      this.errorMessage = '';
    });
  }

  /** Handle form submission for login or registration */
  login(): void {
    if (this.loginForm.invalid) return;

    this.loading = true;
    this.errorMessage = '';

    if (this.isRegistration) {
      // Call signup if in registration mode.
      this.authService.register(this.loginForm.value).subscribe({
        next: (response: any) => {
          console.log('Registration successful:', response);
          this.isRegistration = false;

          this.router.navigate(['/login']);

          this.snackBar.open(
            'Registration Successful',
            'Close',
            { duration: 3000 }
          );
        },
        error: (err:any) => {
          this.snackBar.open(
            'An error occurred. Please try again.',
            'Close',
            { duration: 3000 }
          );
          this.errorMessage = err.error?.message || 'Registration failed. Please try again.';
          this.loading = false;
        },
        complete: () => {
          this.loading = false;
        }
      });
    } else {
      // Normal login flow
      this.authService.login(this.loginForm.value).subscribe({
        next: (response: any) => {
          if (response.token) {
            localStorage.setItem('token', response.token);
            this.router.navigate(['/employees']);
          }
        },
        error: (err) => {
          this.errorMessage = err.error?.message || 'Invalid email or password';
          this.loading = false;
        },
        complete: () => {
          this.loading = false;
        }
      });
    }
  }

  /** Toggle between login and registration modes */
  setRegistrationFlag(event: Event): void {
    event.preventDefault(); // Prevent default anchor behavior
    this.isRegistration = !this.isRegistration;
    console.log('🔄 Toggled mode. isRegistration:', this.isRegistration);
  }
}
