import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick
} from '@angular/core/testing';

import { MyOrdersComponent } from './my-orders.component';

import { OrderService } from '../../core/services/order.service';

import { ToastrService } from 'ngx-toastr';

import { provideRouter } from '@angular/router';

import { of } from 'rxjs';

import { By } from '@angular/platform-browser';

import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('MyOrdersComponent', () => {

  let component: MyOrdersComponent;

  let fixture: ComponentFixture<MyOrdersComponent>;

  let mockOrderService: jasmine.SpyObj<OrderService>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  const mockOrders = [
    {
      orderId: 1,
      userId: 1,
      orderDate: '2024-01-01',
      totalAmount: 2000,
      status: 'Order Placed',
      items: []
    }
  ];

  beforeEach(async () => {

    mockOrderService = jasmine.createSpyObj(
      'OrderService',
      ['getMyOrders', 'cancelOrder']
    );

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      ['success', 'error']
    );

    mockOrderService.getMyOrders.and.returnValue(
      of(mockOrders)
    );

    mockOrderService.cancelOrder.and.returnValue(
      of(void 0)
    );

    await TestBed.configureTestingModule({
      imports: [
        MyOrdersComponent
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

    fixture = TestBed.createComponent(
      MyOrdersComponent
    );

    component = fixture.componentInstance;

    fixture.detectChanges();

  });

  it('should create', () => {

    expect(component).toBeTruthy();

  });

  it('should load orders', fakeAsync(() => {

    component.ngOnInit();

    tick();

    expect(component.orders.length)
      .toBe(1);

    expect(component.loading)
      .toBeFalse();

  }));

  it('should handle load gracefully', fakeAsync(() => {

    mockOrderService.getMyOrders.and.returnValue(
      of([])
    );

    component.ngOnInit();

    tick();

    expect(component.orders.length)
      .toBe(0);

  }));

  it('should show loading spinner', () => {

    component.loading = true;

    fixture.detectChanges();

    const spinner =
      fixture.debugElement.query(
        By.css('.spinner-border')
      );

    expect(spinner).toBeTruthy();

  });

  it('should cancel order', fakeAsync(() => {

    spyOn(window, 'confirm')
      .and.returnValue(true);

    component.cancelOrder(1);

    tick();

    expect(
      mockOrderService.cancelOrder
    ).toHaveBeenCalledWith(1);

  }));

  it('should format price', () => {

    expect(
      component.formatPrice(2000)
    ).toBe('₹2,000');

  });

});