import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../shared/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-login',
  standalone: false,
  templateUrl: './user-login.component.html',
  styleUrl: './user-login.component.css',
})
export class UserLoginComponent {

  loginForm: FormGroup;
  loading: boolean = false;
  errorMessage: string = '';
  isFormValid: boolean = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
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

  /** Handle user login */
  login() {
    if (this.loginForm.invalid) return;

    this.loading = true;
    this.errorMessage = '';

    this.authService.login(this.loginForm.value).subscribe({
      next: (response: any) => {

        if (response.token) {
          localStorage.setItem('token', response.token);
          this.router.navigate(['/employees'])
        }
      },
      error: () => {
        this.errorMessage = 'Invalid email or password';
      }
    });
  }


}