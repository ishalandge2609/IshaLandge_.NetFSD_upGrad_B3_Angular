import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick
} from '@angular/core/testing';

import { AdminProductsComponent } from './admin-products.component';

import { ProductService } from '../../core/services/product.service';

import { ToastrService } from 'ngx-toastr';

import { provideRouter } from '@angular/router';

import { of } from 'rxjs';

import { By } from '@angular/platform-browser';

import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('AdminProductsComponent', () => {

  let component: AdminProductsComponent;

  let fixture: ComponentFixture<AdminProductsComponent>;

  let mockProductService: jasmine.SpyObj<ProductService>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  const mockProducts = {
    data: [
      {
        id: 1,
        name: 'Chair',
        description: 'Wooden Chair',
        price: 1000,
        stock: 5,
        category: 'Furniture',
        imageUrl: '/chair.jpg'
      }
    ]
  };

  beforeEach(async () => {

    mockProductService = jasmine.createSpyObj(
      'ProductService',
      [
        'getProducts',
        'deleteProduct',
        'getFullImageUrl'
      ]
    );

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      ['success', 'error']
    );

    mockProductService.getProducts.and.returnValue(
      of(mockProducts as any)
    );

    mockProductService.deleteProduct.and.returnValue(
      of(void 0)
    );

    mockProductService.getFullImageUrl.and.callFake(
      (url: string) => `https://localhost:7070${url}`
    );

    await TestBed.configureTestingModule({
      imports: [
        AdminProductsComponent
      ],
      providers: [

        provideRouter([]),

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
      AdminProductsComponent
    );

    component = fixture.componentInstance;

    fixture.detectChanges();

  });

  it('should create', () => {

    expect(component).toBeTruthy();

  });

  it('should load products', fakeAsync(() => {

    component.ngOnInit();

    tick();

    expect(
      mockProductService.getProducts
    ).toHaveBeenCalled();

  }));

  it('should delete product', fakeAsync(() => {

    spyOn(window, 'confirm')
      .and.returnValue(true);

    component.deleteProduct(1);

    tick();

    expect(
      mockProductService.deleteProduct
    ).toHaveBeenCalledWith(1);

  }));

  it('should show products table', () => {

    fixture.detectChanges();

    const table =
      fixture.debugElement.query(
        By.css('table')
      );

    expect(table).toBeTruthy();

  });

});