import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { ProductService } from '../../core/services/product.service';
import { CartService } from '../../core/services/cart.service';

import { Product } from '../../core/models/models';

import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

  productService = inject(ProductService);

  cartService = inject(CartService);

  private toastr = inject(ToastrService);

  featuredProducts: Product[] = [];

  loading = true;

  categories = [
    { name: 'Furniture', icon: 'bi bi-lamp' },
    { name: 'Lighting', icon: 'bi bi-lightbulb' },
    { name: 'Planters', icon: 'bi bi-flower1' },
    { name: 'Kitchen & Dining', icon: 'bi bi-cup-hot' },
    { name: 'Bathroom Decor', icon: 'bi bi-droplet' },
    { name: 'Decorative Items', icon: 'bi bi-palette' }
  ];

  ngOnInit() {
    this.loadFeaturedProducts();
  }

  loadFeaturedProducts() {

    this.productService.getProducts(1, 4).subscribe({

      next: (response) => {

        this.featuredProducts = response.data;

        this.loading = false;
      },

      error: () => {

        this.loading = false;

        this.toastr.error('Failed to load products');
      }
    });
  }

  formatPrice(price: number): string {

    return '₹' + price.toLocaleString('en-IN');
  }

  addToCart(product: Product) {

    this.cartService.addToCart(product, 1);
  }

  scrollToFeatured() {

    const element = document.getElementById('featured');

    if (element) {

      element.scrollIntoView({
        behavior: 'smooth',
        block: 'start'
      });
    }
  }
}