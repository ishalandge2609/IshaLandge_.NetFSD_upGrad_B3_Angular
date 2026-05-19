import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../core/services/product.service';
import { CreateProductRequest } from '../../core/models/models';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-add-product',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './admin-add-product.component.html'
})
export class AdminAddProductComponent {
  private productService = inject(ProductService);
  private router = inject(Router);
  private toastr = inject(ToastrService);
  
  product: CreateProductRequest = {
    name: '',
    description: '',
    price: 0,
    stock: 0,
    category: '',
    imageUrl: ''
  };
  
  categories = ['Furniture', 'Lighting', 'Planters', 'Kitchen & Dining', 'Bathroom Decor', 'Decorative Items'];
  isSubmitting = false;

  onSubmit() {
    this.isSubmitting = true;
    
    this.productService.createProduct(this.product).subscribe({
      next: () => {
        this.toastr.success('Product created successfully!');
        this.router.navigate(['/admin/products']);
        this.isSubmitting = false;
      },
      error: (error) => {
        this.toastr.error(error.error?.message || 'Failed to create product');
        this.isSubmitting = false;
      }
    });
  }
}