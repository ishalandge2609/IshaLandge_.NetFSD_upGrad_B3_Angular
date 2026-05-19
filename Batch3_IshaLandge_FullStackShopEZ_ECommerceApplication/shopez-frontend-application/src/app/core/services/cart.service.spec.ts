import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CartService } from './cart.service';
import { Product } from '../models/models';
import { AuthService } from './auth.service';

describe('CartService', () => {
  let service: CartService;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockToastr: jasmine.SpyObj<ToastrService>;

  const mockProduct: Product = {
    id: 1,
    name: 'Test Product',
    description: 'Test Description',
    price: 1000,
    stock: 10,
    category: 'Furniture',
    imageUrl: '/images/test.jpg',
    createdAt: '2024-01-01'
  };

  beforeEach(() => {
    // Clear localStorage before each test
    localStorage.clear();

    // Simple spies for injected dependencies used in CartService
    mockAuthService = jasmine.createSpyObj('AuthService', ['isLoggedIn']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);
    mockToastr = jasmine.createSpyObj('ToastrService', ['success', 'warning', 'error']);

    // Most cart tests need a logged-in user
    mockAuthService.isLoggedIn.and.returnValue(true);
    
    TestBed.configureTestingModule({
      providers: [
        CartService,
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter },
        { provide: ToastrService, useValue: mockToastr }
      ]
    });
    
    service = TestBed.inject(CartService);
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should start with empty cart', () => {
    expect(service.cartCount()).toBe(0);
    expect(service.cartTotal()).toBe(0);
    expect(service.getCartItems()().length).toBe(0);
  });

  it('should add product to cart', () => {
    service.addToCart(mockProduct, 2);
    
    const items = service.getCartItems()();
    expect(items.length).toBe(1);
    expect(items[0].productId).toBe(1);
    expect(items[0].quantity).toBe(2);
    expect(service.cartCount()).toBe(2);
    expect(service.cartTotal()).toBe(2000);
  });

  it('should increase quantity when adding same product', () => {
    service.addToCart(mockProduct, 1);
    service.addToCart(mockProduct, 2);
    
    const items = service.getCartItems()();
    expect(items[0].quantity).toBe(3);
    expect(service.cartCount()).toBe(3);
  });

  it('should not exceed stock limit', () => {
    service.addToCart(mockProduct, 9);
    service.addToCart(mockProduct, 5);
    
    const items = service.getCartItems()();
    // Second add should be blocked because it exceeds stock, so quantity stays 9
    expect(items[0].quantity).toBe(9);
  });

  it('should remove product from cart', () => {
    service.addToCart(mockProduct, 2);
    expect(service.getCartItems()().length).toBe(1);
    
    service.removeFromCart(1);
    expect(service.getCartItems()().length).toBe(0);
    expect(service.cartCount()).toBe(0);
  });

  it('should update quantity', () => {
    service.addToCart(mockProduct, 3);
    service.updateQuantity(1, 5);
    
    const items = service.getCartItems()();
    expect(items[0].quantity).toBe(5);
    expect(service.cartTotal()).toBe(5000);
  });

  it('should remove item when quantity set to 0', () => {
    service.addToCart(mockProduct, 2);
    service.updateQuantity(1, 0);
    
    expect(service.getCartItems()().length).toBe(0);
  });

  it('should increase quantity by 1', () => {
    service.addToCart(mockProduct, 3);
    service.increaseQuantity(1);
    
    const items = service.getCartItems()();
    expect(items[0].quantity).toBe(4);
  });

  it('should decrease quantity by 1', () => {
    service.addToCart(mockProduct, 3);
    service.decreaseQuantity(1);
    
    const items = service.getCartItems()();
    expect(items[0].quantity).toBe(2);
  });

  it('should remove item when decreasing from 1 to 0', () => {
    service.addToCart(mockProduct, 1);
    service.decreaseQuantity(1);
    
    expect(service.getCartItems()().length).toBe(0);
  });

  it('should clear entire cart', () => {
    service.addToCart(mockProduct, 2);
    service.addToCart({ ...mockProduct, id: 2, name: 'Product 2' }, 1);
    expect(service.getCartItems()().length).toBe(2);
    
    service.clearCart();
    expect(service.getCartItems()().length).toBe(0);
    expect(service.cartCount()).toBe(0);
    expect(service.cartTotal()).toBe(0);
  });

  it('should persist cart in localStorage', (done) => {
    service.addToCart(mockProduct, 3);

    // Signal effect writes localStorage; check after microtask queue
    setTimeout(() => {
      const saved = localStorage.getItem('shopez_cart');
      expect(saved).toBeTruthy();
      const parsed = JSON.parse(saved!);
      expect(parsed.length).toBe(1);
      expect(parsed[0].quantity).toBe(3);
      done();
    }, 0);
  });

  it('should load cart from localStorage on initialization', () => {
    // Arrange
    const existingCart = [{ productId: 1, name: 'Test', price: 1000, quantity: 2, imageUrl: '', stock: 10 }];
    localStorage.setItem('shopez_cart', JSON.stringify(existingCart));

    // Act - create a fresh service instance by re-creating the testing module
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({
      providers: [
        CartService,
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter },
        { provide: ToastrService, useValue: mockToastr }
      ]
    });
    const newService = TestBed.inject(CartService);

    // Assert
    expect(newService.getCartItems()().length).toBe(1);
    expect(newService.cartCount()).toBe(2);
  });
});
