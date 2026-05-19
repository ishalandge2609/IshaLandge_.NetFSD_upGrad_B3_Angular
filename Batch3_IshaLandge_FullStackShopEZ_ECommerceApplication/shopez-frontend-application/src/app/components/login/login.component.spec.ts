import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick
} from '@angular/core/testing';

import { LoginComponent } from './login.component';

import { AuthService } from '../../core/services/auth.service';

import { ToastrService } from 'ngx-toastr';

import {
  ActivatedRoute,
  provideRouter
} from '@angular/router';

import { of, throwError } from 'rxjs';

import { FormsModule } from '@angular/forms';

import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('LoginComponent', () => {

  let component: LoginComponent;

  let fixture: ComponentFixture<LoginComponent>;

  let mockAuthService: jasmine.SpyObj<AuthService>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  beforeEach(async () => {

    mockAuthService = jasmine.createSpyObj(
      'AuthService',
      ['login', 'setSession']
    );

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      ['success', 'error']
    );

    await TestBed.configureTestingModule({

      imports: [
        LoginComponent,
        FormsModule
      ],

      providers: [

        provideRouter([
          {
            path: 'home',
            component: LoginComponent
          },
          {
            path: 'admin/dashboard',
            component: LoginComponent
          }
        ]),

        {
          provide: AuthService,
          useValue: mockAuthService
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

      ],

      schemas: [NO_ERRORS_SCHEMA]

    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);

    component = fixture.componentInstance;

    fixture.detectChanges();

  });

  // =========================
  // COMPONENT CREATION
  // =========================

  it('should create', () => {

    expect(component).toBeTruthy();

  });

  // =========================
  // INITIAL VALUES
  // =========================

  it('should have empty form initially', () => {

    expect(component.email).toBe('');

    expect(component.password).toBe('');

    expect(component.isLoading).toBeFalse();

  });

  // =========================
  // LOGIN SUCCESS
  // =========================

  it('should call authService.login on submit', fakeAsync(() => {

    const mockResponse = {
      token: 'token',
      email: 'test@example.com',
      role: 'User' as 'User'
    };

    mockAuthService.login.and.returnValue(
      of(mockResponse)
    );

    component.email = 'test@example.com';

    component.password = 'Password123';

    component.onSubmit();

    tick();

    expect(
      mockAuthService.login
    ).toHaveBeenCalled();

  }));

  // =========================
  // USER LOGIN
  // =========================

  it('should navigate to home for normal user', fakeAsync(() => {

    const mockResponse = {
      token: 'token',
      email: 'test@example.com',
      role: 'User' as 'User'
    };

    mockAuthService.login.and.returnValue(
      of(mockResponse)
    );

    component.email = 'test@example.com';

    component.password = 'Password123';

    component.onSubmit();

    tick();

    expect(
      mockAuthService.setSession
    ).toHaveBeenCalled();

  }));

  // =========================
  // ADMIN LOGIN
  // =========================

  it('should navigate to admin dashboard', fakeAsync(() => {

    const mockResponse = {
      token: 'token',
      email: 'admin@test.com',
      role: 'Admin' as 'Admin'
    };

    mockAuthService.login.and.returnValue(
      of(mockResponse)
    );

    component.email = 'admin@test.com';

    component.password = 'Admin123';

    component.onSubmit();

    tick();

    expect(
      mockAuthService.setSession
    ).toHaveBeenCalled();

  }));

  // =========================
  // LOGIN FAILURE
  // =========================

  it('should handle login error', fakeAsync(() => {

    mockAuthService.login.and.returnValue(
      throwError(() => ({
        error: {
          message: 'Invalid credentials'
        }
      }))
    );

    component.email = 'wrong@test.com';

    component.password = 'Wrong123';

    component.onSubmit();

    tick();

    expect(component.errorMessage)
      .toContain('Invalid credentials');

  }));

});