import {
  Injectable,
  signal,
  computed,
  effect,
  inject
} from '@angular/core';

import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';

import { CartItem, Product } from '../models/models';

import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private readonly CART_KEY = 'shopez_cart';

  private cartItems = signal<CartItem[]>([]);

  private authService = inject(AuthService);

  private router = inject(Router);

  private toastr = inject(ToastrService);

  cartCount = computed(() =>
    this.cartItems().reduce(
      (sum, item) => sum + item.quantity,
      0
    )
  );

  cartTotal = computed(() =>
    this.cartItems().reduce(
      (sum, item) =>
        sum + (item.price * item.quantity),
      0
    )
  );

  constructor() {

    this.loadCart();

    effect(() => {

      this.saveCart();
    });
  }

  private loadCart(): void {

    const savedCart =
      localStorage.getItem(this.CART_KEY);

    if (savedCart) {

      this.cartItems.set(JSON.parse(savedCart));
    }
  }

  private saveCart(): void {

    localStorage.setItem(
      this.CART_KEY,
      JSON.stringify(this.cartItems())
    );
  }

  getCartItems() {

    return this.cartItems.asReadonly();
  }

  addToCart(
    product: Product,
    quantity: number = 1
  ): void {

    // LOGIN CHECK
    if (!this.authService.isLoggedIn()) {

      this.toastr.warning(
        'Please login to add items to cart'
      );

      this.router.navigate(['/login']);

      return;
    }

    // OUT OF STOCK
    if (product.stock <= 0) {

      this.toastr.error(
        `${product.name} is out of stock`
      );

      return;
    }

    const currentItems = this.cartItems();

    const existingItem = currentItems.find(
      item => item.productId === product.id
    );

    // LOW STOCK WARNING
    if (product.stock <= 5) {

      this.toastr.warning(
        `Only ${product.stock} item(s) left in stock`
      );
    }

    if (existingItem) {

      const newQuantity =
        existingItem.quantity + quantity;

      // STOCK LIMIT REACHED
      if (newQuantity > product.stock) {

        this.toastr.error(
          `Only ${product.stock} item(s) available in stock`
        );

        return;
      }

      existingItem.quantity = newQuantity;

      this.cartItems.set([...currentItems]);

      this.toastr.success(
        `${product.name} quantity updated`
      );
    }
    else {

      const cartItem: CartItem = {

        productId: product.id,

        name: product.name,

        price: product.price,

        quantity: quantity,

        imageUrl: product.imageUrl,

        stock: product.stock,

        category: product.category
      };

      this.cartItems.set([
        ...currentItems,
        cartItem
      ]);

      this.toastr.success(
        `${product.name} added to cart`
      );
    }
  }

  removeFromCart(productId: number): void {

    this.cartItems.set(
      this.cartItems().filter(
        item => item.productId !== productId
      )
    );
  }

  updateQuantity(
    productId: number,
    quantity: number
  ): void {

    const currentItems = this.cartItems();

    const item = currentItems.find(
      i => i.productId === productId
    );

    if (!item) {
      return;
    }

    // STOCK LIMIT
    if (quantity > item.stock) {

      this.toastr.warning(
        `Only ${item.stock} item(s) available`
      );

      return;
    }

    if (quantity > 0) {

      item.quantity = quantity;

      this.cartItems.set([...currentItems]);
    }
    else {

      this.removeFromCart(productId);
    }
  }

  increaseQuantity(productId: number): void {

    const currentItems = this.cartItems();

    const item = currentItems.find(
      i => i.productId === productId
    );

    if (!item) {
      return;
    }

    if (item.quantity >= item.stock) {

      this.toastr.warning(
        `Only ${item.stock} item(s) left in stock`
      );

      return;
    }

    item.quantity++;

    this.cartItems.set([...currentItems]);
  }

  decreaseQuantity(productId: number): void {

    const currentItems = this.cartItems();

    const item = currentItems.find(
      i => i.productId === productId
    );

    if (
      item &&
      item.quantity > 1
    ) {

      item.quantity--;

      this.cartItems.set([...currentItems]);
    }
    else if (
      item &&
      item.quantity === 1
    ) {

      this.removeFromCart(productId);
    }
  }

  clearCart(): void {

    this.cartItems.set([]);
  }
}