import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../core/services/product.service';
import { CartService } from '../../core/services/cart.service';
import { Product } from '../../core/models/models';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './products.component.html'
})
export class ProductsComponent implements OnInit, OnDestroy {

  productService = inject(ProductService);

  cartService = inject(CartService);

  private toastr = inject(ToastrService);

  private route = inject(ActivatedRoute);

  allProducts: Product[] = [];

  filteredProducts: Product[] = [];

  loading = true;

  activeCategory = 'All';

  searchQuery = '';

  sortOrder = '';

  private searchSubject = new Subject<string>();

  categories = [
    'Furniture',
    'Lighting',
    'Planters',
    'Kitchen & Dining',
    'Bathroom Decor',
    'Decorative Items'
  ];

  ngOnInit() {

    this.route.queryParams.subscribe(params => {

      if (params['cat']) {

        this.activeCategory = params['cat'];
      }

      this.loadProducts();
    });

    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(() => {

      this.applyFilters();
    });
  }

  ngOnDestroy() {

    this.searchSubject.complete();
  }

  loadProducts() {

    this.loading = true;

    this.productService.getProducts(1, 100).subscribe({

      next: (response) => {

        this.allProducts = response.data;

        this.applyFilters();

        this.loading = false;
      },

      error: () => {

        this.loading = false;

        this.toastr.error('Failed to load products');
      }
    });
  }

  filterByCategory(category: string) {

    this.activeCategory = category;

    this.applyFilters();
  }

  onSearchChange() {

    this.searchSubject.next(this.searchQuery);
  }

  applyFilters() {

    let filtered = [...this.allProducts];

    if (this.activeCategory !== 'All') {

      filtered = filtered.filter(
        p => p.category === this.activeCategory
      );
    }

    if (this.searchQuery.trim()) {

      const query = this.searchQuery.toLowerCase();

      filtered = filtered.filter(
        p =>
          p.name.toLowerCase().includes(query) ||
          p.description.toLowerCase().includes(query)
      );
    }

    if (this.sortOrder === 'asc') {

      filtered.sort((a, b) => a.price - b.price);
    }
    else if (this.sortOrder === 'desc') {

      filtered.sort((a, b) => b.price - a.price);
    }

    this.filteredProducts = filtered;
  }

  applySort() {

    this.applyFilters();
  }

  getCategoryIcon(category: string): string {

    const icons: { [key: string]: string } = {

      'Furniture': 'bi bi-lamp',

      'Lighting': 'bi bi-lightbulb',

      'Planters': 'bi bi-flower1',

      'Kitchen & Dining': 'bi bi-cup-hot',

      'Bathroom Decor': 'bi bi-droplet',

      'Decorative Items': 'bi bi-palette'
    };

    return icons[category] || 'bi bi-tag';
  }

  formatPrice(price: number): string {

    return '₹' + price.toLocaleString('en-IN');
  }

  addToCart(product: Product) {

    this.cartService.addToCart(product, 1);
  }
}