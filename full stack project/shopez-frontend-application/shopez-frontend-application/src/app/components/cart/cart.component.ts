import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CartService } from '../../core/services/cart.service';
import { ProductService } from '../../core/services/product.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './cart.component.html'
})
export class CartComponent implements OnInit {
  cartService = inject(CartService);
  productService = inject(ProductService);
  private toastr = inject(ToastrService);
  
  cartItems = this.cartService.getCartItems();
  subtotal = 0;
  shipping = 49;
  tax = 0;
  total = 0;

  ngOnInit() {
    this.calculateTotals();
  }

  calculateTotals() {
    this.subtotal = this.cartService.cartTotal();
    this.shipping = this.subtotal > 3000 ? 0 : 49;
    this.tax = Math.round(this.subtotal * 0.18);
    this.total = this.subtotal + this.shipping + this.tax;
  }

  increaseQty(productId: number) {
    this.cartService.increaseQuantity(productId);
    this.calculateTotals();
  }

  decreaseQty(productId: number) {
    this.cartService.decreaseQuantity(productId);
    this.calculateTotals();
  }

  updateQty(productId: number, event: Event) {
    const input = event.target as HTMLInputElement;
    let qty = parseInt(input.value);
    if (isNaN(qty)) qty = 1;
    this.cartService.updateQuantity(productId, qty);
    this.calculateTotals();
  }

  removeItem(productId: number) {
    this.cartService.removeFromCart(productId);
    this.calculateTotals();
    this.toastr.info('Item removed from cart');
  }

  formatPrice(price: number): string {
    return '₹' + price.toLocaleString('en-IN');
  }
}