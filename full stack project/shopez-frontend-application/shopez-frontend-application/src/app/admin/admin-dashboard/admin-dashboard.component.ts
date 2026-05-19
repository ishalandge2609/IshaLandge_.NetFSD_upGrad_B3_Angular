import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProductService } from '../../core/services/product.service';
import { OrderService } from '../../core/services/order.service';
import { UserService } from '../../core/services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './admin-dashboard.component.html',
  styles: [`
    .dashboard-card {
      border-left: 4px solid;
      transition: transform 0.2s;
    }
    .dashboard-card:hover { transform: translateY(-3px); }
    .dashboard-card-primary { border-left-color: #4e73df; }
    .dashboard-card-success { border-left-color: #1cc88a; }
    .dashboard-card-info { border-left-color: #36b9cc; }
    .nav-link.active {
      background-color: rgba(255,255,255,0.1);
      color: white !important;
    }
  `]
})
export class AdminDashboardComponent implements OnInit {
  private productService = inject(ProductService);
  private orderService = inject(OrderService);
  private userService = inject(UserService);
  private toastr = inject(ToastrService);
  
  productCount = 0;
  orderCount = 0;
  userCount = 0;
  recentOrders: any[] = [];
  loading = true;

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    this.loading = true;
    
    this.productService.getProducts(1, 100).subscribe({
      next: (products) => {
        this.productCount = products.data.length;
      }
    });
    
    this.orderService.getAllOrders().subscribe({
      next: (orders) => {
        this.orderCount = orders.length;
        this.recentOrders = orders.slice(-5).reverse();
      },
      error: () => this.toastr.error('Failed to load orders')
    });
    
    this.userService.getAllUsers().subscribe({
      next: (users) => {
        this.userCount = users.length;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toastr.error('Failed to load users');
      }
    });
  }

  deleteOrder(orderId: number) {
    if (confirm('Are you sure you want to delete this order?')) {
      this.orderService.deleteOrder(orderId).subscribe({
        next: () => {
          this.toastr.success('Order deleted successfully');
          this.loadDashboardData();
        },
        error: () => this.toastr.error('Failed to delete order')
      });
    }
  }

  formatPrice(price: number): string {
    return '₹' + price.toLocaleString('en-IN');
  }
}