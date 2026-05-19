import {
  HttpClient,
  provideHttpClient,
  withInterceptors
} from '@angular/common/http';

import {
  HttpTestingController,
  provideHttpClientTesting
} from '@angular/common/http/testing';

import { TestBed } from '@angular/core/testing';

import { jwtInterceptor } from './jwt.interceptor';

import { AuthService } from '../services/auth.service';

import { Router, provideRouter } from '@angular/router';

import { ToastrService } from 'ngx-toastr';

describe('jwtInterceptor', () => {

  let httpMock: HttpTestingController;

  let httpClient: HttpClient;

  let mockAuthService: jasmine.SpyObj<AuthService>;

  let mockRouter: jasmine.SpyObj<Router>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  beforeEach(() => {

    mockAuthService = jasmine.createSpyObj(
      'AuthService',
      ['getToken', 'logout']
    );

    mockRouter = jasmine.createSpyObj(
      'Router',
      ['navigate']
    );

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      ['error']
    );

    TestBed.configureTestingModule({
      providers: [

        provideRouter([]),

        provideHttpClient(
          withInterceptors([jwtInterceptor])
        ),

        provideHttpClientTesting(),

        {
          provide: AuthService,
          useValue: mockAuthService
        },

        {
          provide: Router,
          useValue: mockRouter
        },

        {
          provide: ToastrService,
          useValue: mockToastr
        }
      ]
    });

    httpMock = TestBed.inject(HttpTestingController);

    httpClient = TestBed.inject(HttpClient);

  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should add Authorization header when token exists', () => {

    mockAuthService.getToken.and.returnValue(
      'test-token'
    );

    httpClient.get('/api/orders').subscribe();

    const req = httpMock.expectOne('/api/orders');

    expect(
      req.request.headers.get('Authorization')
    ).toBe('Bearer test-token');

    req.flush({});

  });

  it('should not add Authorization header when token does not exist', () => {

    mockAuthService.getToken.and.returnValue(null);

    httpClient.get('/api/orders').subscribe();

    const req = httpMock.expectOne('/api/orders');

    expect(
      req.request.headers.has('Authorization')
    ).toBeFalse();

    req.flush({});

  });

  it('should handle 401 error and redirect to login', () => {

    mockAuthService.getToken.and.returnValue(
      'test-token'
    );

    httpClient.get('/api/orders').subscribe({
      error: () => {}
    });

    const req = httpMock.expectOne('/api/orders');

    req.flush(
      {},
      {
        status: 401,
        statusText: 'Unauthorized'
      }
    );

    expect(
      mockAuthService.logout
    ).toHaveBeenCalled();

    expect(
      mockToastr.error
    ).toHaveBeenCalledWith(
      'Session expired. Please login again.',
      'Unauthorized'
    );

    expect(
      mockRouter.navigate
    ).toHaveBeenCalledWith(['/login']);

  });

  it('should handle 403 error and redirect to home', () => {

    mockAuthService.getToken.and.returnValue(
      'test-token'
    );

    httpClient.get('/api/orders').subscribe({
      error: () => {}
    });

    const req = httpMock.expectOne('/api/orders');

    req.flush(
      {},
      {
        status: 403,
        statusText: 'Forbidden'
      }
    );

    expect(
      mockToastr.error
    ).toHaveBeenCalledWith(
      'You do not have permission to access this resource.',
      'Forbidden'
    );

    expect(
      mockRouter.navigate
    ).toHaveBeenCalledWith(['/home']);

  });

});