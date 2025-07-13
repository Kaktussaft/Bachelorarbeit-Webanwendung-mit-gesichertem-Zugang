import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/authentication-service/authentication-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  passwordType: string = 'password';
  eyeIcon: string = 'fa-eye-slash';
  showLoginForm: boolean = true;
  loginData = { email: '', password: '' };
  registrationFormData = {
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
  };
  registrationData = { username: '', email: '', password: '' };
  isLoading = false;
  errorMessage = '';

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  async ngOnInit() {
    await this.authService.refreshCSRFToken();
  }

  login() {
    if (!this.loginData.email || !this.loginData.password) {
      this.errorMessage = 'Please fill in all fields';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.loginData).subscribe({
      next: (response) => {
        if (response.LoginResponse.success) {
          this.router.navigate(['/dashboard']);
        } else {
          this.errorMessage = response.LoginResponse.message || 'Login failed';
        }
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Login failed';
        this.isLoading = false;
      },
    });
  }

  register() {
    if (
      !this.registrationFormData.username ||
      !this.registrationFormData.email ||
      !this.registrationFormData.password ||
      !this.registrationFormData.confirmPassword
    ) {
      this.errorMessage = 'Please fill in all fields';
      return;
    }

    if (
      this.registrationFormData.password !==
      this.registrationFormData.confirmPassword
    ) {
      this.errorMessage = 'Passwords do not match';
      return;
    }

    this.registrationData = {
      username: this.registrationFormData.username,
      email: this.registrationFormData.email,
      password: this.registrationFormData.password,
    };

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.register(this.registrationData).subscribe({
      next: (response) => {
        if (response.success) {
          this.showLoginForm = true;
          this.errorMessage = '';
        } else {
          this.errorMessage = response.message || 'Registration failed';
        }
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Registration failed';
        this.isLoading = false;
      },
    });
  }

  toggleLoginAndRegisterForm() {
    this.showLoginForm = !this.showLoginForm;
  }

  togglePasswordVisibility() {
    this.passwordType = this.passwordType === 'password' ? 'text' : 'password';
    this.eyeIcon = this.eyeIcon === 'fa-eye-slash' ? 'fa-eye' : 'fa-eye-slash';
  }
}
