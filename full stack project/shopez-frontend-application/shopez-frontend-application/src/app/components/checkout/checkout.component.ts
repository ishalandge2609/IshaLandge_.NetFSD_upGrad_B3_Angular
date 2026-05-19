import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';

import { CartService } from '../../core/services/cart.service';
import { OrderService } from '../../core/services/order.service';
import { ProductService } from '../../core/services/product.service';

import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './checkout.component.html'
})
export class CheckoutComponent implements OnInit {

  cartService = inject(CartService);

  orderService = inject(OrderService);

  productService = inject(ProductService);

  private toastr = inject(ToastrService);

  private router = inject(Router);

  cartItems = this.cartService.getCartItems();

  subtotal = 0;

  shipping = 49;

  tax = 0;

  total = 0;

  isProcessing = false;

  orderPlaced = false;

  orderData = {
    fullName: '',
    email: '',
    phone: '',
    address: '',
    state: '',
    city: '',
    pincode: '',
    paymentMethod: 'cod'
  };

  ngOnInit() {

    if (this.cartItems().length === 0) {

      this.router.navigate(['/cart']);

      this.toastr.warning('Your cart is empty');
    }

    this.calculateTotals();
  }

  calculateTotals() {

    this.subtotal = this.cartService.cartTotal();

    this.shipping = this.subtotal > 3000 ? 0 : 49;

    this.tax = Math.round(this.subtotal * 0.18);

    this.total = this.subtotal + this.shipping + this.tax;
  }

  placeOrder(checkoutForm: NgForm) {

    if (checkoutForm.invalid) {

      Object.keys(checkoutForm.controls).forEach(field => {

        const control = checkoutForm.controls[field];

        control.markAsTouched();
      });

      this.toastr.error('Please fill all mandatory fields');

      return;
    }

    this.isProcessing = true;

    const orderItems = this.cartItems().map(item => ({

      productId: item.productId,

      quantity: item.quantity
    }));

    const orderPayload = {
      items: orderItems
    };

    this.orderService.createOrder(orderPayload)
      .subscribe({

        next: () => {

          this.orderPlaced = true;

          this.cartService.clearCart();

          this.isProcessing = false;

          this.toastr.success('Order placed successfully!');
        },

        error: (error) => {

          this.isProcessing = false;

          let errorMessage = 'Failed to place order';

          if (typeof error.error === 'string') {

            errorMessage = error.error;

          } else if (error.error?.error) {

            errorMessage = error.error.error;

          } else if (error.error?.message) {

            errorMessage = error.error.message;

          } else if (error.message) {

            errorMessage = error.message;
          }

          this.toastr.error(errorMessage);
        }
      });
  }

  formatPrice(price: number): string {

    return '₹' + price.toLocaleString('en-IN');
  }
}