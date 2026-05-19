import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../../core/services/order.service';
import { Order } from '../../core/models/models';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-orders',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './admin-orders.component.html'
})
export class AdminOrdersComponent implements OnInit {
  private orderService = inject(OrderService);
  private toastr = inject(ToastrService);
  
  orders: Order[] = [];
  filteredOrders: Order[] = [];
  loading = true;
  searchQuery = '';
  selectedOrder: Order | null = null;

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    this.loading = true;
    this.orderService.getAllOrders().subscribe({
      next: (orders) => {
        this.orders = orders;
        this.filteredOrders = [...orders];
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toastr.error('Failed to load orders');
      }
    });
  }

  filterOrders() {
    if (!this.searchQuery.trim()) {
      this.filteredOrders = [...this.orders];
    } else {
      const query = this.searchQuery.toLowerCase();
      this.filteredOrders = this.orders.filter(o => 
        o.orderId.toString().includes(query) ||
        o.status.toLowerCase().includes(query)
      );
    }
  }

  viewOrderDetails(order: Order) {
    this.selectedOrder = order;
  }

  deleteOrder(orderId: number) {
    if (confirm('Are you sure you want to delete this order?')) {
      this.orderService.deleteOrder(orderId).subscribe({
        next: () => {
          this.toastr.success('Order deleted successfully');
          this.loadOrders();
        },
        error: () => this.toastr.error('Failed to delete order')
      });
    }
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