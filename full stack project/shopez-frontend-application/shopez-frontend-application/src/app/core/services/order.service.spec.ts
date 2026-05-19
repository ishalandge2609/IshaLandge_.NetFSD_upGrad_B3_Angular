import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { OrderService } from './order.service';
import { environment } from '../../../environments/environment';

describe('OrderService', () => {
  let service: OrderService;
  let httpMock: HttpTestingController;
  const apiUrl = environment.apiUrl;

  const mockOrder = {
    orderId: 1,
    userId: 1,
    orderDate: '2024-01-01T00:00:00',
    totalAmount: 2000,
    status: 'Order Placed',
    items: [
      { productId: 1, productName: 'Test Product', unitPrice: 1000, quantity: 2 }
    ]
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [OrderService]
    });
    
    service = TestBed.inject(OrderService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('createOrder', () => {
    it('should send POST request to create order', () => {
      const orderRequest = { items: [{ productId: 1, quantity: 2 }] };
      const mockResponse = { success: true, orderId: 1 };
      
      service.createOrder(orderRequest).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });
      
      const req = httpMock.expectOne(`${apiUrl}/api/orders`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(orderRequest);
      req.flush(mockResponse);
    });
  });

  describe('getMyOrders', () => {
    it('should fetch user orders', () => {
      const mockOrders = [mockOrder];
      
      service.getMyOrders().subscribe(orders => {
        expect(orders.length).toBe(1);
        expect(orders[0].orderId).toBe(1);
      });
      
      const req = httpMock.expectOne(`${apiUrl}/api/orders/my-orders`);
      expect(req.request.method).toBe('GET');
      req.flush(mockOrders);
    });
  });

  describe('getAllOrders', () => {
    it('should fetch all orders for admin', () => {
      const mockOrders = [mockOrder, { ...mockOrder, orderId: 2 }];
      
      service.getAllOrders().subscribe(orders => {
        expect(orders.length).toBe(2);
      });
      
      const req = httpMock.expectOne(`${apiUrl}/api/orders/all`);
      expect(req.request.method).toBe('GET');
      req.flush(mockOrders);
    });
  });

  describe('deleteOrder', () => {
    it('should send DELETE request to remove order', () => {
      service.deleteOrder(1).subscribe();
      
      const req = httpMock.expectOne(`${apiUrl}/api/orders/1`);
      expect(req.request.method).toBe('DELETE');
      req.flush({});
    });
  });

  describe('cancelOrder', () => {
    it('should send PUT request to cancel order', () => {
      service.cancelOrder(1).subscribe();
      
      const req = httpMock.expectOne(`${apiUrl}/api/orders/1/cancel`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual({});
      req.flush({ success: true, message: 'Order cancelled' });
    });
  });
});
