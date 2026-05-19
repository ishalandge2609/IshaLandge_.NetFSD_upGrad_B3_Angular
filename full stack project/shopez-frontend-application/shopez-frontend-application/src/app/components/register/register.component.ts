import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './register.component.html'
})
export class RegisterComponent {

  private authService = inject(AuthService);
  private router = inject(Router);
  private toastr = inject(ToastrService);

  email = '';
  password = '';
  confirmPassword = '';

  isLoading = false;

  errorMessage = '';
  successMessage = '';

  // Matches backend validation:
  // Minimum 6 chars + uppercase + lowercase + number
  passwordPattern =
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/;

  get isPasswordValid(): boolean {
    return this.passwordPattern.test(this.password);
  }

  get doPasswordsMatch(): boolean {
    return this.password === this.confirmPassword;
  }

  onSubmit() {

    if (!this.isPasswordValid) {

      this.errorMessage =
        'Password must contain uppercase, lowercase and number.';

      return;
    }

    if (!this.doPasswordsMatch) {

      this.errorMessage = 'Passwords do not match';

      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.authService.register({
      email: this.email,
      password: this.password
    }).subscribe({

      next: (response) => {

        if (response.success) {

          this.successMessage =
            response.message ||
            'Registration successful! Redirecting to login...';

          this.toastr.success(
            'Registration successful! Please login.'
          );

          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 2000);

        } else {

          this.errorMessage = response.message;
          this.toastr.error(this.errorMessage);
        }

        this.isLoading = false;
      },

      error: (error) => {

        this.isLoading = false;

        this.errorMessage =
          error.error?.message ||
          'Registration failed. Please try again.';

        this.toastr.error(this.errorMessage);
      }
    });
  }
}