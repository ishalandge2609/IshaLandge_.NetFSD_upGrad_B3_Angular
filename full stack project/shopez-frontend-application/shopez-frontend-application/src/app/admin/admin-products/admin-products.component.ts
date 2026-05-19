import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../core/services/product.service';
import { Product } from '../../core/models/models';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-products',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './admin-products.component.html'
})
export class AdminProductsComponent implements OnInit {
  productService = inject(ProductService);
  private toastr = inject(ToastrService);
  
  products: Product[] = [];
  filteredProducts: Product[] = [];
  loading = true;
  searchQuery = '';
  
  selectedProduct: Product | null = null;
  stockQuantity: number = 1;
  showStockModal = false;
  stockAction: 'reduce' | 'restore' = 'reduce';

  ngOnInit() {
    this.loadProducts();
  }

  loadProducts() {
    this.loading = true;
    this.productService.getProducts(1, 100).subscribe({
      next: (response) => {
        this.products = response.data;
        this.filteredProducts = [...this.products];
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toastr.error('Failed to load products');
      }
    });
  }

  filterProducts() {
    if (!this.searchQuery.trim()) {
      this.filteredProducts = [...this.products];
    } else {
      const query = this.searchQuery.toLowerCase();
      this.filteredProducts = this.products.filter(p => 
        p.name.toLowerCase().includes(query) || 
        p.category.toLowerCase().includes(query)
      );
    }
  }

  deleteProduct(id: number) {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.deleteProduct(id).subscribe({
        next: () => {
          this.toastr.success('Product deleted successfully');
          this.loadProducts();
        },
        error: () => this.toastr.error('Failed to delete product')
      });
    }
  }

  openStockModal(product: Product, action: 'reduce' | 'restore') {
    this.selectedProduct = product;
    this.stockAction = action;
    this.stockQuantity = 1;
    this.showStockModal = true;
  }

  updateStock() {
    if (!this.selectedProduct) return;
    
    const obs = this.stockAction === 'reduce' 
      ? this.productService.reduceStock(this.selectedProduct.id, this.stockQuantity)
      : this.productService.restoreStock(this.selectedProduct.id, this.stockQuantity);
    
    obs.subscribe({
      next: () => {
        this.toastr.success(`Stock ${this.stockAction === 'reduce' ? 'reduced' : 'restored'} successfully`);
        this.showStockModal = false;
        this.loadProducts();
      },
      error: () => this.toastr.error('Failed to update stock')
    });
  }

  formatPrice(price: number): string {
    return '₹' + price.toLocaleString('en-IN');
  }
}