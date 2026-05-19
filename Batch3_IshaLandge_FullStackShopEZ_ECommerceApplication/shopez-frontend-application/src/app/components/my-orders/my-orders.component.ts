import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { OrderService } from '../../core/services/order.service';
import { Order } from '../../core/models/models';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './my-orders.component.html'
})
export class MyOrdersComponent implements OnInit {
  private orderService = inject(OrderService);
  private toastr = inject(ToastrService);
  
  orders: Order[] = [];
  loading = true;
  cancellingOrderId: number | null = null;

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    this.loading = true;
    this.orderService.getMyOrders().subscribe({
      next: (orders) => {
        this.orders = orders;
        this.loading = false;
      },
      error: (error) => {
        this.loading = false;
        this.toastr.error('Failed to load orders');
        console.error(error);
      }
    });
  }

  // NEW: Cancel order method
  cancelOrder(orderId: number) {
    if (confirm('Are you sure you want to cancel this order? This action cannot be undone.')) {
      this.cancellingOrderId = orderId;
      
      this.orderService.cancelOrder(orderId).subscribe({
        next: (response) => {
          this.toastr.success(`Order #${orderId} has been cancelled successfully`);
          this.loadOrders(); // Refresh the orders list
          this.cancellingOrderId = null;
        },
        error: (error) => {
          this.toastr.error(error.error?.message || 'Failed to cancel order');
          this.cancellingOrderId = null;
        }
      });
    }
  }

  // Check if order can be cancelled (only allow for non-delivered orders)
  canCancelOrder(status: string): boolean {
    const cancelledStatuses = ['Cancelled', 'Delivered'];
    return !cancelledStatuses.includes(status);
  }

  getStatusBadgeClass(status: string): string {
    const classes: {[key: string]: string} = {
      'Order Placed': 'bg-primary',
      'Processing': 'bg-info',
      'Shipped': 'bg-warning',
      'Delivered': 'bg-success',
      'Cancelled': 'bg-danger'
    };
    return classes[status] || 'bg-secondary';
  }

  formatPrice(price: number): string {
    return '₹' + price.toLocaleString('en-IN');
  }
}