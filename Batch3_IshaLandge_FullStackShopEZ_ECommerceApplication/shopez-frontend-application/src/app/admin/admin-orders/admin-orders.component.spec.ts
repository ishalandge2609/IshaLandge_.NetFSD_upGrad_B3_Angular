import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { AdminOrdersComponent } from './admin-orders.component';
import { OrderService } from '../../core/services/order.service';
import { ToastrService } from 'ngx-toastr';
import { of, throwError } from 'rxjs';
import { By } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { provideRouter } from '@angular/router';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('AdminOrdersComponent', () => {

  let component: AdminOrdersComponent;
  let fixture: ComponentFixture<AdminOrdersComponent>;

  let mockOrderService: jasmine.SpyObj<OrderService>;
  let mockToastr: jasmine.SpyObj<ToastrService>;

  const mockOrders = [
    {
      orderId: 101,
      userId: 1,
      orderDate: '2024-01-15T10:30:00',
      totalAmount: 2500,
      status: 'Order Placed',
      items: [
        {
          productId: 1,
          productName: 'Product A',
          unitPrice: 1000,
          quantity: 2
        }
      ]
    },
    {
      orderId: 102,
      userId: 2,
      orderDate: '2024-01-16T14:20:00',
      totalAmount: 5000,
      status: 'Processing',
      items: []
    }
  ];

  beforeEach(async () => {

    mockOrderService = jasmine.createSpyObj('OrderService', [
      'getAllOrders',
      'deleteOrder'
    ]);

    mockToastr = jasmine.createSpyObj('ToastrService', [
      'success',
      'error'
    ]);

    mockOrderService.getAllOrders.and.returnValue(
      of(mockOrders)
    );

    await TestBed.configureTestingModule({
      imports: [
        AdminOrdersComponent,
        FormsModule
      ],
      providers: [
        provideRouter([]),
        {
          provide: OrderService,
          useValue: mockOrderService
        },
        {
          provide: ToastrService,
          useValue: mockToastr
        }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(AdminOrdersComponent);
    component = fixture.componentInstance;

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load orders', fakeAsync(() => {

    component.ngOnInit();
    tick();

    expect(component.orders.length).toBe(2);
    expect(component.filteredOrders.length).toBe(2);

  }));

  it('should filter orders', () => {

    component.orders = mockOrders;
    component.searchQuery = '101';

    component.filterOrders();

    expect(component.filteredOrders.length).toBe(1);

  });

  it('should view order details', () => {

    component.viewOrderDetails(mockOrders[0]);

    expect(component.selectedOrder).toEqual(mockOrders[0]);

  });

  it('should delete order', fakeAsync(() => {

    mockOrderService.deleteOrder.and.returnValue(
      of(void 0)
    );

    spyOn(window, 'confirm').and.returnValue(true);

    component.deleteOrder(101);

    tick();

    expect(mockOrderService.deleteOrder).toHaveBeenCalledWith(101);

  }));

  it('should handle delete error', fakeAsync(() => {

    mockOrderService.deleteOrder.and.returnValue(
      throwError(() => new Error('Failed'))
    );

    spyOn(window, 'confirm').and.returnValue(true);

    component.deleteOrder(101);

    tick();

    expect(mockToastr.error).toHaveBeenCalled();

  }));

  it('should show orders table', fakeAsync(() => {

    component.ngOnInit();
    tick();

    fixture.detectChanges();

    const table = fixture.debugElement.query(By.css('.table'));

    expect(table).toBeTruthy();

  }));

});