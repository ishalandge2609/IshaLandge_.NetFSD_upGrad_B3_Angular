import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { CartService } from '../../core/services/cart.service';
import { AuthService } from '../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html'
})
export class NavbarComponent implements OnInit {
  cartService = inject(CartService);
  authService = inject(AuthService);
  private router = inject(Router);
  private toastr = inject(ToastrService);

  ngOnInit() {
    // Bootstrap is already loaded globally from angular.json
  }

  logout() {
    this.authService.logout();
    this.toastr.success('Logged out successfully');
    this.router.navigate(['/home']);
  }
}