import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CartComponent } from './cart.component';

import { CartService } from '../../core/services/cart.service';

import { ProductService } from '../../core/services/product.service';

import { ToastrService } from 'ngx-toastr';

import { By } from '@angular/platform-browser';

import { provideRouter } from '@angular/router';

import { signal } from '@angular/core';

import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('CartComponent', () => {

  let component: CartComponent;

  let fixture: ComponentFixture<CartComponent>;

  let mockCartService: jasmine.SpyObj<CartService>;

  let mockProductService: jasmine.SpyObj<ProductService>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  const mockCartItems = [
    {
      productId: 1,
      name: 'Product 1',
      price: 1000,
      quantity: 2,
      imageUrl: '/img1.jpg',
      stock: 10,
      category: 'Furniture'
    },
    {
      productId: 2,
      name: 'Product 2',
      price: 500,
      quantity: 3,
      imageUrl: '/img2.jpg',
      stock: 5,
      category: 'Lighting'
    }
  ];

  beforeEach(async () => {

    mockCartService = jasmine.createSpyObj(
      'CartService',
      [
        'getCartItems',
        'cartCount',
        'cartTotal',
        'increaseQuantity',
        'decreaseQuantity',
        'updateQuantity',
        'removeFromCart'
      ]
    );

    mockProductService = jasmine.createSpyObj(
      'ProductService',
      ['getFullImageUrl']
    );

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      ['info', 'success', 'error']
    );

    mockCartService.getCartItems.and.returnValue(
      signal(mockCartItems)
    );

    mockCartService.cartCount.and.returnValue(5);

    mockCartService.cartTotal.and.returnValue(3500);

    mockProductService.getFullImageUrl.and.callFake(
      (url: string) =>
        `https://localhost:7070${url}`
    );

    await TestBed.configureTestingModule({
      imports: [
        CartComponent
      ],
      providers: [
        provideRouter([]),
        {
          provide: CartService,
          useValue: mockCartService
        },
        {
          provide: ProductService,
          useValue: mockProductService
        },
        {
          provide: ToastrService,
          useValue: mockToastr
        }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(
      CartComponent
    );

    component = fixture.componentInstance;

    fixture.detectChanges();

  });

  it('should create', () => {

    expect(component).toBeTruthy();

  });

  it('should calculate totals correctly', () => {

    component.calculateTotals();

    expect(component.subtotal).toBe(3500);

    expect(component.shipping).toBe(0);

    expect(component.tax).toBe(630);

    expect(component.total).toBe(4130);

  });

  it('should have free shipping when subtotal > 3000', () => {

    component.calculateTotals();

    expect(component.shipping).toBe(0);

  });

  it('should increase quantity', () => {

    component.increaseQty(1);

    expect(
      mockCartService.increaseQuantity
    ).toHaveBeenCalledWith(1);

  });

  it('should decrease quantity', () => {

    component.decreaseQty(1);

    expect(
      mockCartService.decreaseQuantity
    ).toHaveBeenCalledWith(1);

  });

  it('should update quantity from input', () => {

    const mockEvent = {
      target: {
        value: '5'
      }
    } as any;

    component.updateQty(1, mockEvent);

    expect(
      mockCartService.updateQuantity
    ).toHaveBeenCalledWith(1, 5);

  });

  it('should default to 1 when quantity is invalid', () => {

    const mockEvent = {
      target: {
        value: 'invalid'
      }
    } as any;

    component.updateQty(1, mockEvent);

    expect(
      mockCartService.updateQuantity
    ).toHaveBeenCalledWith(1, 1);

  });

  it('should remove item from cart', () => {

    component.removeItem(1);

    expect(
      mockCartService.removeFromCart
    ).toHaveBeenCalledWith(1);

    expect(
      mockToastr.info
    ).toHaveBeenCalledWith(
      'Item removed from cart'
    );

  });

  it('should format price correctly', () => {

    expect(component.formatPrice(1000))
      .toBe('₹1,000');

    expect(component.formatPrice(25000))
      .toBe('₹25,000');

  });

  it('should handle empty cart', () => {

    mockCartService.getCartItems.and.returnValue(
      signal([])
    );

    fixture = TestBed.createComponent(
      CartComponent
    );

    component = fixture.componentInstance;

    fixture.detectChanges();

    expect(component.cartItems().length)
      .toBe(0);

  });

});