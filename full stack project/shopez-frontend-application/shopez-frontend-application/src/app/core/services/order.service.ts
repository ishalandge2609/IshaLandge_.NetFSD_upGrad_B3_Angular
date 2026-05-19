import { Injectable, inject } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import {
  CreateOrderRequest,
  Order
} from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private http = inject(HttpClient);

  private apiUrl = environment.apiUrl;

  createOrder(
    order: CreateOrderRequest
  ): Observable<any> {

    return this.http.post(
      `${this.apiUrl}/api/orders`,
      order
    );
  }

  getMyOrders(): Observable<Order[]> {

    return this.http.get<Order[]>(
      `${this.apiUrl}/api/orders/my-orders`
    );
  }

  getAllOrders(): Observable<Order[]> {

    return this.http.get<Order[]>(
      `${this.apiUrl}/api/orders/all`
    );
  }

  deleteOrder(orderId: number): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/api/orders/${orderId}`
    );
  }

  cancelOrder(orderId: number): Observable<any> {

    return this.http.put(
      `${this.apiUrl}/api/orders/${orderId}/cancel`,
      {}
    );
  }
}