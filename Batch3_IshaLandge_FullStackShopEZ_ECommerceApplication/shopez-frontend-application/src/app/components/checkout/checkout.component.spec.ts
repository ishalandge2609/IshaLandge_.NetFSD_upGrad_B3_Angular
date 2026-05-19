import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick
} from '@angular/core/testing';

import { signal } from '@angular/core';

import { CheckoutComponent } from './checkout.component';

import { CartService } from '../../core/services/cart.service';

import { OrderService } from '../../core/services/order.service';

import { ProductService } from '../../core/services/product.service';

import { ToastrService } from 'ngx-toastr';

import { Router } from '@angular/router';

import { RouterTestingModule } from '@angular/router/testing';

import { of, throwError } from 'rxjs';

import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('CheckoutComponent', () => {

  let component: CheckoutComponent;

  let fixture: ComponentFixture<CheckoutComponent>;

  let mockCartService: jasmine.SpyObj<CartService>;

  let mockOrderService: jasmine.SpyObj<OrderService>;

  let mockProductService: jasmine.SpyObj<ProductService>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  let router: Router;

  const mockCartItems = [
    {
      productId: 1,
      name: 'Chair',
      price: 1000,
      quantity: 2,
      imageUrl: '/chair.jpg',
      stock: 10,
      category: 'Furniture'
    }
  ];

  beforeEach(async () => {

    mockCartService = jasmine.createSpyObj(
      'CartService',
      [
        'getCartItems',
        'cartTotal',
        'clearCart'
      ]
    );

    mockOrderService = jasmine.createSpyObj(
      'OrderService',
      ['createOrder']
    );

    mockProductService = jasmine.createSpyObj(
      'ProductService',
      ['getFullImageUrl']
    );

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      ['success', 'error', 'warning']
    );

    mockCartService.getCartItems.and.returnValue(
      signal(mockCartItems)
    );

    mockCartService.cartTotal.and.returnValue(
      2000
    );

    await TestBed.configureTestingModule({

      imports: [
        CheckoutComponent,
        RouterTestingModule
      ],

      providers: [

        {
          provide: CartService,
          useValue: mockCartService
        },

        {
          provide: OrderService,
          useValue: mockOrderService
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

    router = TestBed.inject(Router);

    spyOn(router, 'navigate');

    fixture = TestBed.createComponent(
      CheckoutComponent
    );

    component = fixture.componentInstance;

    fixture.detectChanges();

  });

  it('should create', () => {

    expect(component).toBeTruthy();

  });

  it('should calculate totals', () => {

    component.ngOnInit();

    expect(component.subtotal)
      .toBe(2000);

    expect(component.tax)
      .toBe(360);

  });

  it('should redirect if cart empty', () => {

    mockCartService.getCartItems.and.returnValue(
      signal([])
    );

    component.cartItems =
      mockCartService.getCartItems();

    component.ngOnInit();

    expect(router.navigate)
      .toHaveBeenCalledWith(['/cart']);

  });

  it('should place order successfully', fakeAsync(() => {

    mockOrderService.createOrder.and.returnValue(
      of({ success: true })
    );

    const form: any = {
      invalid: false,
      controls: {}
    };

    component.placeOrder(form);

    tick();

    expect(
      mockOrderService.createOrder
    ).toHaveBeenCalled();

    expect(
      mockCartService.clearCart
    ).toHaveBeenCalled();

  }));

  it('should handle order error', fakeAsync(() => {

    mockOrderService.createOrder.and.returnValue(
      throwError(() => ({
        error: {
          message: 'Failed'
        }
      }))
    );

    const form: any = {
      invalid: false,
      controls: {}
    };

    component.placeOrder(form);

    tick();

    expect(
      mockToastr.error
    ).toHaveBeenCalled();

  }));

});