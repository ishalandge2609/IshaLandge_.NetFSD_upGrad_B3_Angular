import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick
} from '@angular/core/testing';

import { ProductDetailsComponent } from './product-details.component';

import { ProductService } from '../../core/services/product.service';

import { CartService } from '../../core/services/cart.service';

import { ToastrService } from 'ngx-toastr';

import {
  ActivatedRoute,
  provideRouter
} from '@angular/router';

import { of } from 'rxjs';

import { FormsModule } from '@angular/forms';

import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('ProductDetailsComponent', () => {

  let component: ProductDetailsComponent;

  let fixture: ComponentFixture<ProductDetailsComponent>;

  let mockProductService: jasmine.SpyObj<ProductService>;

  let mockCartService: jasmine.SpyObj<CartService>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  const mockProduct = {
    id: 1,
    name: 'Test Product',
    description: 'Test Description',
    price: 1000,
    stock: 10,
    category: 'Furniture',
    imageUrl: '/test.jpg'
  };

  const mockAllProducts = {
    data: [
      {
        id: 1,
        name: 'Test Product',
        price: 1000,
        stock: 10,
        category: 'Furniture',
        description: 'Desc',
        imageUrl: '/img1.jpg'
      },
      {
        id: 2,
        name: 'Related Product',
        price: 900,
        stock: 5,
        category: 'Furniture',
        description: 'Desc',
        imageUrl: '/img2.jpg'
      }
    ]
  };

  beforeEach(async () => {

    mockProductService = jasmine.createSpyObj(
      'ProductService',
      [
        'getProductById',
        'getProducts',
        'getFullImageUrl'
      ]
    );

    mockCartService = jasmine.createSpyObj(
      'CartService',
      ['addToCart']
    );

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      ['success', 'error']
    );

    mockProductService.getProductById.and.returnValue(
      of(mockProduct)
    );

    mockProductService.getProducts.and.returnValue(
      of(mockAllProducts)
    );

    mockProductService.getFullImageUrl.and.callFake(
      (url: string) =>
        `https://localhost:7070${url}`
    );

    await TestBed.configureTestingModule({

      imports: [
        ProductDetailsComponent,
        FormsModule
      ],

      providers: [

        provideRouter([]),

        {
          provide: ProductService,
          useValue: mockProductService
        },

        {
          provide: CartService,
          useValue: mockCartService
        },

        {
          provide: ToastrService,
          useValue: mockToastr
        },

        {
          provide: ActivatedRoute,
          useValue: {
            paramMap: of({
              get: () => '1'
            })
          }
        }

      ],

      schemas: [NO_ERRORS_SCHEMA]

    }).compileComponents();

    fixture = TestBed.createComponent(
      ProductDetailsComponent
    );

    component = fixture.componentInstance;

    fixture.detectChanges();

  });

  it('should create', () => {

    expect(component).toBeTruthy();

  });

  it('should load product on init', fakeAsync(() => {

    component.ngOnInit();

    tick();

    expect(
      mockProductService.getProductById
    ).toHaveBeenCalledWith(1);

    expect(
      component.product?.name
    ).toBe('Test Product');

  }));

  it('should load related products', fakeAsync(() => {

    component.ngOnInit();

    tick();

    expect(
      component.relatedProducts.length
    ).toBeGreaterThan(0);

  }));

  it('should handle product response', fakeAsync(() => {

    mockProductService.getProductById.and.returnValue(
      of(mockProduct)
    );

    component.ngOnInit();

    tick();

    expect(
      component.product
    ).toEqual(mockProduct as any);

  }));

  it('should increase quantity', () => {

    component.product = mockProduct as any;

    component.quantity = 1;

    component.incrementQty();

    expect(component.quantity).toBe(2);

  });

  it('should decrease quantity', () => {

    component.quantity = 5;

    component.decrementQty();

    expect(component.quantity).toBe(4);

  });

  it('should add to cart', () => {

    component.product = mockProduct as any;

    component.quantity = 3;

    component.addToCart();

    expect(
      mockCartService.addToCart
    ).toHaveBeenCalledWith(
      mockProduct,
      3
    );

  });

  it('should format price correctly', () => {

    expect(
      component.formatPrice(1000)
    ).toBe('₹1,000');

  });

});