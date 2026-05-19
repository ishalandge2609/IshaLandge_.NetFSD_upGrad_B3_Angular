import {
  ComponentFixture,
  TestBed
} from '@angular/core/testing';

import { NavbarComponent } from './navbar.component';

import { AuthService } from '../../core/services/auth.service';

import { CartService } from '../../core/services/cart.service';

import { ToastrService } from 'ngx-toastr';

import { signal } from '@angular/core';

import {
  ActivatedRoute,
  provideRouter
} from '@angular/router';

import { of } from 'rxjs';

describe('NavbarComponent', () => {

  let component: NavbarComponent;

  let fixture: ComponentFixture<NavbarComponent>;

  let mockAuthService: jasmine.SpyObj<AuthService>;

  let mockCartService: jasmine.SpyObj<CartService>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  beforeEach(async () => {

    mockAuthService = jasmine.createSpyObj(
      'AuthService',
      [
        'isLoggedIn',
        'isAdmin',
        'logout',
        'getRole'
      ]
    );

    mockCartService = jasmine.createSpyObj(
      'CartService',
      [
        'cartCount',
        'getCartItems'
      ]
    );

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      [
        'success',
        'error'
      ]
    );

    mockAuthService.isLoggedIn.and.returnValue(
      true
    );

    mockAuthService.isAdmin.and.returnValue(
      false
    );

    mockCartService.cartCount.and.returnValue(
      3
    );

    mockCartService.getCartItems.and.returnValue(
      signal([])
    );

    await TestBed.configureTestingModule({

      imports: [
        NavbarComponent
      ],

      providers: [

        provideRouter([]),

        {
          provide: AuthService,
          useValue: mockAuthService
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
            snapshot: {},
            params: of({})
          }
        }

      ]

    }).compileComponents();

    fixture = TestBed.createComponent(
      NavbarComponent
    );

    component = fixture.componentInstance;

    fixture.detectChanges();

  });

  it('should create', () => {

    expect(component).toBeTruthy();

  });

  it('should logout user', () => {

    component.logout();

    expect(
      mockAuthService.logout
    ).toHaveBeenCalled();

  });

  it('should display cart count', () => {

    expect(
      mockCartService.cartCount()
    ).toBe(3);

  });

});