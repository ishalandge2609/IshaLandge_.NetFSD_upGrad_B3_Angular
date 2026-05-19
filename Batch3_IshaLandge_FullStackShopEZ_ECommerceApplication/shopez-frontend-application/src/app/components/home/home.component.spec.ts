import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick
} from '@angular/core/testing';

import { HomeComponent } from './home.component';

import { ProductService } from '../../core/services/product.service';

import { CartService } from '../../core/services/cart.service';

import { ToastrService } from 'ngx-toastr';

import { provideRouter } from '@angular/router';

import { of, throwError } from 'rxjs';

import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('HomeComponent', () => {
  let component: HomeComponent;

  let fixture: ComponentFixture<HomeComponent>;

  let mockProductService: jasmine.SpyObj<ProductService>;

  let mockCartService: jasmine.SpyObj<CartService>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  const mockProducts = {
    data: [
      {
        id: 1,
        name: 'Product 1',
        description: 'Desc 1',
        price: 1000,
        stock: 10,
        category: 'Furniture',
        imageUrl: '/img1.jpg'
      }
    ]
  };

  beforeEach(async () => {
    mockProductService = jasmine.createSpyObj(
      'ProductService',
      ['getProducts', 'getFullImageUrl']
    );

    mockCartService = jasmine.createSpyObj(
      'CartService',
      ['addToCart']
    );

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      ['success', 'error']
    );

    mockProductService.getProducts.and.returnValue(
      of(mockProducts)
    );

    mockProductService.getFullImageUrl.and.callFake(
      (url: string) => `https://localhost:7070${url}`
    );

    await TestBed.configureTestingModule({
      imports: [HomeComponent],
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
        }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(HomeComponent);

    component = fixture.componentInstance;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load featured products', fakeAsync(() => {
    component.ngOnInit();

    tick();

    expect(
      mockProductService.getProducts
    ).toHaveBeenCalled();

    expect(component.featuredProducts.length)
      .toBeGreaterThan(0);

    expect(component.loading).toBeFalse();
  }));

  it('should handle loading error', fakeAsync(() => {
    mockProductService.getProducts.and.returnValue(
      throwError(() => new Error('Network error'))
    );

    component.ngOnInit();

    tick();

    expect(mockToastr.error).toHaveBeenCalled();
  }));

  it('should add product to cart', () => {
    component.addToCart(mockProducts.data[0]);

    expect(mockCartService.addToCart)
      .toHaveBeenCalledWith(
        mockProducts.data[0],
        1
      );
  });

  it('should format price', () => {
    expect(component.formatPrice(1000))
      .toBe('₹1,000');
  });

  it('should have categories', () => {
    expect(component.categories.length)
      .toBeGreaterThan(0);
  });
});