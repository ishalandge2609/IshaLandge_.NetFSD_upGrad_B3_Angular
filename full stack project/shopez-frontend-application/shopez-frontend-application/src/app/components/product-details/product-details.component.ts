import { Component, inject, OnInit } from '@angular/core';

import { CommonModule } from '@angular/common';

import {
  ActivatedRoute,
  RouterModule
} from '@angular/router';

import { FormsModule } from '@angular/forms';

import { ProductService } from '../../core/services/product.service';

import { CartService } from '../../core/services/cart.service';

import { Product } from '../../core/models/models';

import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule
  ],
  templateUrl: './product-details.component.html'
})
export class ProductDetailsComponent
  implements OnInit {

  productService = inject(ProductService);

  cartService = inject(CartService);

  private route = inject(ActivatedRoute);

  private toastr = inject(ToastrService);

  product: Product | null = null;

  relatedProducts: Product[] = [];

  loading = true;

  quantity = 1;

  ngOnInit() {

    // IMPORTANT FIX
    this.route.paramMap.subscribe(params => {

      const id = params.get('id');

      if (id) {

        this.quantity = 1;

        this.loadProduct(+id);
      }
    });
  }

  loadProduct(id: number) {

    this.loading = true;

    this.productService
      .getProductById(id)
      .subscribe({

        next: (response: any) => {

          const product =
            response.data || response;

          this.product = product;

          this.loadRelatedProducts(
            product.category,
            product.id
          );

          this.loading = false;
        },

        error: (error) => {

          console.error(error);

          this.loading = false;

          this.toastr.error(
            'Product not found'
          );
        }
      });
  }

  loadRelatedProducts(
    category: string,
    currentId: number
  ) {

    this.productService
      .getProducts(1, 100)
      .subscribe({

        next: (response: any) => {

          const products =
            response.data || [];

          this.relatedProducts = products
            .filter(
              (p: Product) =>
                p.category === category &&
                p.id !== currentId
            )
            .slice(0, 4);
        },

        error: (error) => {

          console.error(
            'Failed to load related products',
            error
          );
        }
      });
  }

  incrementQty() {

    if (
      this.product &&
      this.quantity < this.product.stock
    ) {

      this.quantity++;
    }
  }

  decrementQty() {

    if (this.quantity > 1) {

      this.quantity--;
    }
  }

  validateQuantity() {

    if (this.product) {

      if (this.quantity < 1) {

        this.quantity = 1;
      }

      if (
        this.quantity >
        this.product.stock
      ) {

        this.quantity =
          this.product.stock;
      }
    }
  }

  addToCart() {

    if (this.product) {

      this.cartService.addToCart(
        this.product,
        this.quantity
      );
    }
  }

  addToCartRelated(product: Product) {

    this.cartService.addToCart(
      product,
      1
    );
  }

  formatPrice(price: number): string {

    return '₹' +
      price.toLocaleString('en-IN');
  }
}