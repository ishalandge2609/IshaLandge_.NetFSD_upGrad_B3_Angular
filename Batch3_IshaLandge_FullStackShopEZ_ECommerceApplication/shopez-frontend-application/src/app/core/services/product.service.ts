import { Injectable, inject } from '@angular/core';

import {
  HttpClient,
  HttpParams
} from '@angular/common/http';

import {
  Observable,
  map
} from 'rxjs';

import { environment }
from '../../../environments/environment';

import {
  Product,
  CreateProductRequest,
  ProductResponse
} from '../models/models';

@Injectable({
  providedIn: 'root'
})

export class ProductService {

  private http = inject(HttpClient);

  private apiUrl = environment.apiUrl;


  // GET PRODUCTS


  getProducts(
    page: number = 1,
    pageSize: number = 10
  ): Observable<ProductResponse> {

    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<ProductResponse>(
      `${this.apiUrl}/api/products`,
      { params }
    );

  }

  // =========================
  // GET PRODUCT BY ID
  // =========================

  getProductById(
    id: number
  ): Observable<Product> {

    return this.http.get<any>(
      `${this.apiUrl}/api/products/${id}`
    ).pipe(

      map(response => response.data)

    );

  }

  // =========================
  // CREATE PRODUCT
  // =========================

  createProduct(
    product: CreateProductRequest
  ): Observable<Product> {

    return this.http.post<Product>(
      `${this.apiUrl}/api/products`,
      product
    );

  }

  // =========================
  // UPDATE PRODUCT
  // =========================

  updateProduct(
    id: number,
    product: CreateProductRequest
  ): Observable<Product> {

    return this.http.put<Product>(
      `${this.apiUrl}/api/products/${id}`,
      product
    );

  }

  // =========================
  // DELETE PRODUCT
  // =========================

  deleteProduct(
    id: number
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/api/products/${id}`
    );

  }

  // =========================
  // REDUCE STOCK
  // =========================

  reduceStock(
    id: number,
    quantity: number
  ): Observable<void> {

    return this.http.put<void>(
      `${this.apiUrl}/api/products/${id}/stock/reduce`,
      { quantity }
    );

  }

  // =========================
  // RESTORE STOCK
  // =========================

  restoreStock(
    id: number,
    quantity: number
  ): Observable<void> {

    return this.http.put<void>(
      `${this.apiUrl}/api/products/${id}/stock/restock`,
      { quantity }
    );

  }

  // =========================
  // IMAGE URL
  // =========================

  getFullImageUrl(
    imageUrl: string
  ): string {

    // NO IMAGE

    if (!imageUrl) {

      return 'assets/placeholder.jpg';

    }

    // ALREADY FULL URL

    if (
      imageUrl.startsWith('http://') ||
      imageUrl.startsWith('https://')
    ) {

      return imageUrl;

    }

    // RELATIVE IMAGE PATH

    return `${this.apiUrl}${imageUrl}`;

  }

}