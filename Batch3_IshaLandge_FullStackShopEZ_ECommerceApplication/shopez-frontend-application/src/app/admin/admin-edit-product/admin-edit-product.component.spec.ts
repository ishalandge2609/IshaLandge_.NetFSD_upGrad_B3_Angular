import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick
} from '@angular/core/testing';

import { AdminEditProductComponent } from './admin-edit-product.component';

import { ProductService } from '../../core/services/product.service';

import { ToastrService } from 'ngx-toastr';

import {
  ActivatedRoute,
  Router,
  convertToParamMap
} from '@angular/router';

import { RouterTestingModule } from '@angular/router/testing';

import { of, throwError } from 'rxjs';

describe('AdminEditProductComponent', () => {

  let component: AdminEditProductComponent;

  let fixture: ComponentFixture<AdminEditProductComponent>;

  let mockProductService: jasmine.SpyObj<ProductService>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  let router: Router;

  const mockProduct = {
    id: 1,
    name: 'Chair',
    description: 'Wooden Chair',
    price: 1000,
    stock: 10,
    category: 'Furniture',
    imageUrl: '/chair.jpg'
  };

  beforeEach(async () => {

    mockProductService = jasmine.createSpyObj(
      'ProductService',
      [
        'getProductById',
        'updateProduct'
      ]
    );

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      [
        'success',
        'error'
      ]
    );

    mockProductService.getProductById.and.returnValue(
      of(mockProduct as any)
    );

    await TestBed.configureTestingModule({

      imports: [
        AdminEditProductComponent,
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
        },

        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: () => '1'
              }
            },
            paramMap: of(convertToParamMap({ id: '1' })),
            params: of({ id: '1' })
          }
        }

      ]

    }).compileComponents();

    router = TestBed.inject(Router);
    spyOn(router, 'navigate').and.resolveTo(true);

    fixture = TestBed.createComponent(
      AdminEditProductComponent
    );

    component = fixture.componentInstance;

    fixture.detectChanges();

  });

  it('should create', () => {

    expect(component).toBeTruthy();

  });

  it('should load product', fakeAsync(() => {

    component.ngOnInit();

    tick();

    expect(
      mockProductService.getProductById
    ).toHaveBeenCalledWith(1);

  }));

  it('should update product', fakeAsync(() => {

    mockProductService.updateProduct.and.returnValue(
      of(mockProduct as any)
    );

    component.product = mockProduct as any;

    component.editData = {
      name: mockProduct.name,
      description: mockProduct.description,
      price: mockProduct.price,
      stock: mockProduct.stock,
      category: mockProduct.category,
      imageUrl: mockProduct.imageUrl
    };

    component.onSubmit();

    tick();

    expect(
      mockProductService.updateProduct
    ).toHaveBeenCalled();

    expect(
      router.navigate
    ).toHaveBeenCalledWith([
      '/admin/products'
    ]);

  }));

  it('should handle update error', fakeAsync(() => {

    mockProductService.updateProduct.and.returnValue(
      throwError(() => ({
        error: {
          message: 'Failed'
        }
      }))
    );

    component.product = mockProduct as any;

    component.editData = {
      name: mockProduct.name,
      description: mockProduct.description,
      price: mockProduct.price,
      stock: mockProduct.stock,
      category: mockProduct.category,
      imageUrl: mockProduct.imageUrl
    };

    component.onSubmit();

    tick();

    expect(
      mockToastr.error
    ).toHaveBeenCalled();

  }));

});
