import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { AdminDashboardComponent } from './admin-dashboard.component';
import { ProductService } from '../../core/services/product.service';
import { OrderService } from '../../core/services/order.service';
import { UserService } from '../../core/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { of, throwError } from 'rxjs';
import { provideRouter } from '@angular/router';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('AdminDashboardComponent', () => {

  let component: AdminDashboardComponent;
  let fixture: ComponentFixture<AdminDashboardComponent>;

  let mockProductService: jasmine.SpyObj<ProductService>;
  let mockOrderService: jasmine.SpyObj<OrderService>;
  let mockUserService: jasmine.SpyObj<UserService>;
  let mockToastr: jasmine.SpyObj<ToastrService>;

  const mockProducts = [
    {
      id: 1,
      name: 'Chair',
      description: 'Chair desc',
      price: 1000,
      stock: 5,
      category: 'Furniture',
      imageUrl: '/chair.jpg'
    },
    {
      id: 2,
      name: 'Lamp',
      description: 'Lamp desc',
      price: 2000,
      stock: 10,
      category: 'Lighting',
      imageUrl: '/lamp.jpg'
    },
    {
      id: 3,
      name: 'Plant',
      description: 'Plant desc',
      price: 1500,
      stock: 8,
      category: 'Planters',
      imageUrl: '/plant.jpg'
    }
  ];

  const mockOrders = [
    {
      orderId: 1,
      userId: 1,
      orderDate: '2024-01-01',
      totalAmount: 1000,
      status: 'Delivered',
      items: []
    }
  ];

  beforeEach(async () => {

    mockProductService = jasmine.createSpyObj('ProductService', ['getProducts']);
    mockOrderService = jasmine.createSpyObj('OrderService', ['getAllOrders', 'deleteOrder']);
    mockUserService = jasmine.createSpyObj('UserService', ['getAllUsers']);
    mockToastr = jasmine.createSpyObj('ToastrService', ['success', 'error']);

    mockProductService.getProducts.and.returnValue(
      of({ data: mockProducts })
    );

    mockOrderService.getAllOrders.and.returnValue(
      of(mockOrders)
    );

    mockUserService.getAllUsers.and.returnValue(
      of([
        {
          id: 1,
          email: 'test@test.com',
          role: 'User'
        }
      ])
    );

    await TestBed.configureTestingModule({
      imports: [AdminDashboardComponent],
      providers: [
        provideRouter([]),
        { provide: ProductService, useValue: mockProductService },
        { provide: OrderService, useValue: mockOrderService },
        { provide: UserService, useValue: mockUserService },
        { provide: ToastrService, useValue: mockToastr }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(AdminDashboardComponent);
    component = fixture.componentInstance;

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load dashboard data', fakeAsync(() => {

    component.ngOnInit();
    tick();

    expect(component.productCount).toBe(3);
    expect(component.orderCount).toBe(1);
    expect(component.userCount).toBe(1);

  }));

  it('should delete order', fakeAsync(() => {

    mockOrderService.deleteOrder.and.returnValue(
      of(void 0)
    );

    spyOn(window, 'confirm').and.returnValue(true);

    component.deleteOrder(1);

    tick();

    expect(mockOrderService.deleteOrder).toHaveBeenCalledWith(1);

  }));

  it('should handle order loading error', fakeAsync(() => {

    mockOrderService.getAllOrders.and.returnValue(
      throwError(() => new Error('Failed'))
    );

    component.ngOnInit();
    tick();

    expect(mockToastr.error).toHaveBeenCalled();

  }));

});