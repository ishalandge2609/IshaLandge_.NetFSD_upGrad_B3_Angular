import { Component, inject, OnInit } from '@angular/core';

import { CommonModule } from '@angular/common';

import {
  RouterModule,
  Router,
  ActivatedRoute
} from '@angular/router';

import { FormsModule } from '@angular/forms';

import { ProductService }
from '../../core/services/product.service';

import {
  CreateProductRequest,
  Product
} from '../../core/models/models';

import { ToastrService }
from 'ngx-toastr';

@Component({
  selector: 'app-admin-edit-product',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule
  ],
  templateUrl:
    './admin-edit-product.component.html'
})

export class AdminEditProductComponent
implements OnInit {

  private productService =
    inject(ProductService);

  private router =
    inject(Router);

  private route =
    inject(ActivatedRoute);

  private toastr =
    inject(ToastrService);

  product: Product | null = null;

  editData: CreateProductRequest = {

    name: '',
    description: '',
    price: 0,
    stock: 0,
    category: '',
    imageUrl: ''

  };

  categories = [

    'Furniture',
    'Lighting',
    'Planters',
    'Kitchen & Dining',
    'Bathroom Decor',
    'Decorative Items'

  ];

  loading = true;

  isSubmitting = false;

  ngOnInit() {

    const id =
      this.route.snapshot.paramMap.get('id');

    if (id) {

      this.loadProduct(+id);

    }

  }

  
  // LOAD PRODUCT
  
  loadProduct(id: number) {

    this.productService
      .getProductById(id)
      .subscribe({

      next: (product) => {

        this.product = product;

        // REMOVE API URL IF EXISTS

        let cleanImageUrl =
          product.imageUrl || '';

        cleanImageUrl =
          cleanImageUrl.replace(
            this.productService['apiUrl'],
            ''
          );

        this.editData = {

          name: product.name,

          description:
            product.description,

          price: product.price,

          stock: product.stock,

          category: product.category,

          imageUrl: cleanImageUrl

        };

        this.loading = false;

      },

      error: () => {

        this.loading = false;

        this.toastr.error(
          'Product not found'
        );

        this.router.navigate([
          '/admin/products'
        ]);

      }

    });

  }

  
  // UPDATE PRODUCT
  

  onSubmit() {

    if (!this.product) return;

    this.isSubmitting = true;

    // ENSURE ONLY RELATIVE PATH SAVED

    if (
      this.editData.imageUrl.startsWith(
        'http'
      )
    ) {

      this.editData.imageUrl =
        this.editData.imageUrl.replace(
          this.productService['apiUrl'],
          ''
        );

    }

    this.productService.updateProduct(
      this.product.id,
      this.editData
    ).subscribe({

      next: () => {

        this.toastr.success(
          'Product updated successfully!'
        );

        this.router.navigate([
          '/admin/products'
        ]);

        this.isSubmitting = false;

      },

      error: (error) => {

        this.toastr.error(
          error.error?.message ||
          'Failed to update product'
        );

        this.isSubmitting = false;

      }

    });

  }

}