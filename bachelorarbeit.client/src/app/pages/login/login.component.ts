import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/authentication-service.service';
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
  loginData = { username: '', password: '' };
  registerData = { username: '', email: '', password: '', confirmPassword: '' };

  constructor(private readonly authService: AuthService, private readonly router: Router) {}

  toggleLoginAndRegisterForm() {
    this.showLoginForm = !this.showLoginForm;
  }

  login() {
    this.authService.login(this.loginData).subscribe({
      next: (response) => {
        localStorage.setItem('loginToken', response.token);
        this.authService.setAuthenticated(true);
        this.router.navigate(['/home']);
      },
      error: (error) => {
        console.error('Login failed', error);
      },
    });
  }

  register(){
    this.authService.register(this.registerData).subscribe({
      next: (response) => {
        console.log('Registration successful', response);
        this.toggleLoginAndRegisterForm();
      },
      error: (error) => {
        console.error('Registration failed', error);
      },
    })
  }
}


