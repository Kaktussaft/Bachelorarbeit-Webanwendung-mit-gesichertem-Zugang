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
  loginData = { email: '', password: '' };
  registrationFormData = { username: '', email: '', password: '', confirmPassword: '' };
  registrationData = { username: '', email: '', password: '' };

  constructor(private readonly authService: AuthService, private readonly router: Router) {}

  toggleLoginAndRegisterForm() {
    this.showLoginForm = !this.showLoginForm;
  }

  login() {
    this.authService.login(this.loginData).subscribe({
      next: () => {
        this.router.navigate(['/home']);
      },
      error: (error) => {
        console.error('Login failed', error);
      },
    });
  }

  register(){
    if (this.registrationFormData.password !== this.registrationFormData.confirmPassword){
      console.error('Passwords do not match');
      return;
    }

    this.registrationData.username = this.registrationFormData.username;
    this.registrationData.email = this.registrationFormData.email;
    this.registrationData.password = this.registrationFormData.password;

    this.authService.register(this.registrationData).subscribe({
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


