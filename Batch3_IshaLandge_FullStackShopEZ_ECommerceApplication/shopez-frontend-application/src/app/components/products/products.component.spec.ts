import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick
} from '@angular/core/testing';

import { ProductsComponent } from './products.component';

import { ProductService } from '../../core/services/product.service';

import { CartService } from '../../core/services/cart.service';

import { ToastrService } from 'ngx-toastr';

import {
  ActivatedRoute,
  provideRouter
} from '@angular/router';

import { of, throwError } from 'rxjs';

import { By } from '@angular/platform-browser';

import { FormsModule } from '@angular/forms';

import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('ProductsComponent', () => {

  let component: ProductsComponent;

  let fixture: ComponentFixture<ProductsComponent>;

  let mockProductService: jasmine.SpyObj<ProductService>;

  let mockCartService: jasmine.SpyObj<CartService>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  const mockAllProducts = {
    data: [
      {
        id: 1,
        name: 'Wooden Table',
        description: 'Beautiful wooden table',
        price: 15000,
        stock: 5,
        category: 'Furniture',
        imageUrl: '/table.jpg'
      },
      {
        id: 2,
        name: 'LED Lamp',
        description: 'Modern LED lamp',
        price: 2500,
        stock: 10,
        category: 'Lighting',
        imageUrl: '/lamp.jpg'
      },
      {
        id: 3,
        name: 'Ceramic Planter',
        description: 'Handmade ceramic planter',
        price: 800,
        stock: 15,
        category: 'Planters',
        imageUrl: '/planter.jpg'
      },
      {
        id: 4,
        name: 'Tea Set',
        description: 'Premium tea set',
        price: 3500,
        stock: 8,
        category: 'Kitchen & Dining',
        imageUrl: '/tea.jpg'
      },
      {
        id: 5,
        name: 'Bamboo Shelf',
        description: 'Wall mounted shelf',
        price: 4500,
        stock: 6,
        category: 'Furniture',
        imageUrl: '/shelf.jpg'
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

    mockProductService.getFullImageUrl.and.callFake(
      (url: string) =>
        `https://localhost:7070${url}`
    );

    await TestBed.configureTestingModule({

      imports: [
        ProductsComponent,
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
            queryParams: of({})
          }
        }

      ],

      schemas: [NO_ERRORS_SCHEMA]

    }).compileComponents();

    fixture = TestBed.createComponent(
      ProductsComponent
    );

    component = fixture.componentInstance;

  });

  it('should create', () => {

    expect(component).toBeTruthy();

  });

  it('should load products on init', fakeAsync(() => {

    mockProductService.getProducts.and.returnValue(
      of(mockAllProducts)
    );

    component.ngOnInit();

    tick();

    expect(
      mockProductService.getProducts
    ).toHaveBeenCalledWith(1, 100);

    expect(component.allProducts.length)
      .toBe(5);

    expect(component.filteredProducts.length)
      .toBe(5);

    expect(component.loading)
      .toBeFalse();

  }));

  it('should handle error when loading products fails', fakeAsync(() => {

    mockProductService.getProducts.and.returnValue(
      throwError(() => new Error('Failed'))
    );

    component.ngOnInit();

    tick();

    expect(component.loading)
      .toBeFalse();

    expect(mockToastr.error)
      .toHaveBeenCalledWith('Failed to load products');

  }));

  it('should filter products by category', fakeAsync(() => {

    mockProductService.getProducts.and.returnValue(
      of(mockAllProducts)
    );

    component.ngOnInit();

    tick();

    component.filterByCategory('Furniture');

    expect(component.activeCategory)
      .toBe('Furniture');

    expect(component.filteredProducts.length)
      .toBe(2);

    expect(component.filteredProducts[0].category)
      .toBe('Furniture');

  }));

  it('should filter products by search query', fakeAsync(() => {

    mockProductService.getProducts.and.returnValue(
      of(mockAllProducts)
    );

    component.ngOnInit();

    tick();

    component.searchQuery = 'table';

    component.onSearchChange();

    tick(300);

    expect(component.filteredProducts.length)
      .toBe(1);

    expect(component.filteredProducts[0].name)
      .toContain('Table');

  }));

  it('should sort products by price ascending', fakeAsync(() => {

    mockProductService.getProducts.and.returnValue(
      of(mockAllProducts)
    );

    component.ngOnInit();

    tick();

    component.sortOrder = 'asc';

    component.applySort();

    expect(component.filteredProducts[0].price)
      .toBe(800);

    expect(component.filteredProducts[4].price)
      .toBe(15000);

  }));

  it('should sort products by price descending', fakeAsync(() => {

    mockProductService.getProducts.and.returnValue(
      of(mockAllProducts)
    );

    component.ngOnInit();

    tick();

    component.sortOrder = 'desc';

    component.applySort();

    expect(component.filteredProducts[0].price)
      .toBe(15000);

    expect(component.filteredProducts[4].price)
      .toBe(800);

  }));

  it('should combine category filter and search', fakeAsync(() => {

    mockProductService.getProducts.and.returnValue(
      of(mockAllProducts)
    );

    component.ngOnInit();

    tick();

    component.filterByCategory('Furniture');

    component.searchQuery = 'wooden';

    component.onSearchChange();

    tick(300);

    expect(component.filteredProducts.length)
      .toBe(1);

    expect(component.filteredProducts[0].name)
      .toBe('Wooden Table');

  }));

  it('should show empty state when no products match', fakeAsync(() => {

    mockProductService.getProducts.and.returnValue(
      of(mockAllProducts)
    );

    component.ngOnInit();

    tick();

    component.searchQuery = 'nonexistent';

    component.onSearchChange();

    tick(300);

    expect(component.filteredProducts.length)
      .toBe(0);

    fixture.detectChanges();

    const emptyState =
      fixture.debugElement.query(
        By.css('.empty-state')
      );

    expect(emptyState)
      .toBeTruthy();

  }));

  it('should add product to cart', () => {

    const product = mockAllProducts.data[0];

    component.addToCart(product);

    expect(
      mockCartService.addToCart
    ).toHaveBeenCalledWith(product, 1);

  });

  it('should return correct category icon', () => {

    expect(
      component.getCategoryIcon('Furniture')
    ).toBe('bi bi-lamp');

    expect(
      component.getCategoryIcon('Lighting')
    ).toBe('bi bi-lightbulb');

    expect(
      component.getCategoryIcon('Unknown')
    ).toBe('bi bi-tag');

  });

});