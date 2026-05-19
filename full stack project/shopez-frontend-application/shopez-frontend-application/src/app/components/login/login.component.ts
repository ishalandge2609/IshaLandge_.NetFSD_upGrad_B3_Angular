import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {

  private authService = inject(AuthService);
  private router = inject(Router);
  private toastr = inject(ToastrService);

  email = '';
  password = '';

  isLoading = false;

  errorMessage = '';

  // Matches backend validation
  passwordPattern =
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/;

  get isPasswordValid(): boolean {

    return this.passwordPattern.test(
      this.password
    );

  }

  onSubmit() {

    // EMAIL VALIDATION

    if (!this.email) {

      this.errorMessage = 'Email is required';

      return;

    }

    // PASSWORD VALIDATION

    if (!this.password) {

      this.errorMessage = 'Password is required';

      return;

    }

    // PASSWORD RULE VALIDATION

    if (!this.isPasswordValid) {

      this.errorMessage =
        'Password must contain uppercase, lowercase and number.';

      return;

    }

    this.isLoading = true;

    this.errorMessage = '';

    this.authService.login({
      email: this.email,
      password: this.password
    }).subscribe({

      next: (response) => {

        this.authService.setSession(
          response.token,
          response.email,
          response.role
        );

        this.toastr.success(
          `Welcome back, ${response.email}!`
        );

        if (response.role === 'Admin') {

          this.router.navigate([
            '/admin/dashboard'
          ]);

        } else {

          this.router.navigate([
            '/home'
          ]);

        }

        this.isLoading = false;

      },

      error: (error) => {

        this.isLoading = false;

        this.errorMessage =
          error.error?.message ||
          'Invalid email or password';

        this.toastr.error(
          this.errorMessage
        );

      }

    });

  }

}