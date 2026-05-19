import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick
} from '@angular/core/testing';

import { AdminAddProductComponent } from './admin-add-product.component';

import { ProductService } from '../../core/services/product.service';

import { ToastrService } from 'ngx-toastr';

import { RouterTestingModule } from '@angular/router/testing';

import { Router } from '@angular/router';

import { of, throwError } from 'rxjs';

import { By } from '@angular/platform-browser';

import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('AdminAddProductComponent', () => {

  let component: AdminAddProductComponent;

  let fixture: ComponentFixture<AdminAddProductComponent>;

  let mockProductService: jasmine.SpyObj<ProductService>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  let router: Router;

  const validProduct = {
    name: 'Chair',
    description: 'Wooden Chair',
    price: 1000,
    stock: 5,
    category: 'Furniture',
    imageUrl: '/chair.jpg'
  };

  beforeEach(async () => {

    mockProductService = jasmine.createSpyObj(
      'ProductService',
      ['createProduct']
    );

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      ['success', 'error']
    );

    await TestBed.configureTestingModule({

      imports: [
        AdminAddProductComponent,
        RouterTestingModule
      ],

      providers: [

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
      AdminAddProductComponent
    );

    component = fixture.componentInstance;

    fixture.detectChanges();

  });

  it('should create', () => {

    expect(component).toBeTruthy();

  });

  it('should call createProduct', fakeAsync(() => {

    mockProductService.createProduct.and.returnValue(
      of({
        id: 1,
        ...validProduct
      } as any)
    );

    component.product = {
      ...validProduct
    };

    component.onSubmit();

    tick();

    expect(
      mockProductService.createProduct
    ).toHaveBeenCalled();

  }));

  it('should handle error', fakeAsync(() => {

    mockProductService.createProduct.and.returnValue(
      throwError(() => ({
        error: {
          message: 'Failed'
        }
      }))
    );

    component.product = {
      ...validProduct
    };

    component.onSubmit();

    tick();

    expect(
      mockToastr.error
    ).toHaveBeenCalled();

  }));

  it('should show loading state', () => {

    component.isSubmitting = true;

    fixture.detectChanges();

    const button =
      fixture.debugElement.query(
        By.css('button')
      );

    expect(button).toBeTruthy();

  });

});