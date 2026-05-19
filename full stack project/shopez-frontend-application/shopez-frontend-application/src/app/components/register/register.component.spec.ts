import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick
} from '@angular/core/testing';

import { RegisterComponent } from './register.component';

import { AuthService } from '../../core/services/auth.service';

import { ToastrService } from 'ngx-toastr';

import {
  ActivatedRoute,
  provideRouter
} from '@angular/router';

import { of, throwError } from 'rxjs';

import { FormsModule } from '@angular/forms';

import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('RegisterComponent', () => {

  let component: RegisterComponent;

  let fixture: ComponentFixture<RegisterComponent>;

  let mockAuthService: jasmine.SpyObj<AuthService>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  beforeEach(async () => {

    mockAuthService = jasmine.createSpyObj(
      'AuthService',
      ['register']
    );

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      ['success', 'error']
    );

    await TestBed.configureTestingModule({

      imports: [
        RegisterComponent,
        FormsModule
      ],

      providers: [

        provideRouter([
          {
            path: 'login',
            component: RegisterComponent
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

    fixture = TestBed.createComponent(
      RegisterComponent
    );

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

    expect(component.confirmPassword).toBe('');

  });

  // =========================
  // PASSWORD VALIDATION
  // =========================

  it('should validate password correctly', () => {

    component.password = 'Test123';

    expect(component.isPasswordValid)
      .toBeTrue();

  });

  it('should reject invalid password', () => {

    component.password = '123456';

    expect(component.isPasswordValid)
      .toBeFalse();

  });

  // =========================
  // PASSWORD MATCH VALIDATION
  // =========================

  it('should show password mismatch error', () => {

    component.password = 'Test123';

    component.confirmPassword = 'Test456';

    component.onSubmit();

    expect(component.errorMessage)
      .toContain('Passwords do not match');

  });

  // =========================
  // REGISTER SUCCESS
  // =========================

  it('should call register service successfully', fakeAsync(() => {

    mockAuthService.register.and.returnValue(
      of({
        success: true,
        message: 'Registered'
      })
    );

    component.email = 'test@test.com';

    component.password = 'Test123';

    component.confirmPassword = 'Test123';

    component.onSubmit();

    tick();

    expect(
      mockAuthService.register
    ).toHaveBeenCalled();

    expect(
      mockToastr.success
    ).toHaveBeenCalled();

  }));

  // =========================
  // REGISTER FAILURE
  // =========================

  it('should handle register error', fakeAsync(() => {

    mockAuthService.register.and.returnValue(
      throwError(() => ({
        error: {
          message: 'Registration failed'
        }
      }))
    );

    component.email = 'test@test.com';

    component.password = 'Test123';

    component.confirmPassword = 'Test123';

    component.onSubmit();

    tick();

    expect(component.errorMessage)
      .toContain('Registration failed');

    expect(
      mockToastr.error
    ).toHaveBeenCalled();

  }));

});