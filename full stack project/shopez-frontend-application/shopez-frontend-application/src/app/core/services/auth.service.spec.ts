import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './auth.service';
import { environment } from '../../../environments/environment';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  const apiUrl = environment.apiUrl;

  beforeEach(() => {
    localStorage.clear();
    
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService]
    });
    
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('HTTP Requests', () => {
    it('should send POST request to register', () => {
      const user = { email: 'test@example.com', password: '123456' };
      const mockResponse = { success: true, message: 'User registered successfully' };
      
      service.register(user).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });
      
      const req = httpMock.expectOne(`${apiUrl}/api/Auth/register`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(user);
      req.flush(mockResponse);
    });

    it('should send POST request to login', () => {
      const credentials = { email: 'test@example.com', password: '123456' };
      const mockResponse = { token: 'jwt-token', email: 'test@example.com', role: 'User' };
      
      service.login(credentials).subscribe(response => {
        expect(response.token).toBe('jwt-token');
        expect(response.email).toBe('test@example.com');
        expect(response.role).toBe('User');
      });
      
      const req = httpMock.expectOne(`${apiUrl}/api/Auth/login`);
      expect(req.request.method).toBe('POST');
      req.flush(mockResponse);
    });

    it('should send GET request to profile', () => {
      // Service only sends plain GET. Authorization header is added by interceptor in integration flow.
      service.getProfile().subscribe();
      
      const req = httpMock.expectOne(`${apiUrl}/api/Auth/profile`);
      expect(req.request.method).toBe('GET');
      req.flush({});
    });

    it('should send PUT request to change role', () => {
      const userId = 1;
      const roleData = { role: 'Admin' };
      
      service.changeRole(userId, roleData).subscribe();
      
      const req = httpMock.expectOne(`${apiUrl}/api/Auth/change-role/${userId}`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(roleData);
      req.flush({});
    });
  });

  describe('localStorage Management', () => {
    it('should save session to localStorage', () => {
      service.setSession('token123', 'user@test.com', 'Admin');
      
      expect(localStorage.getItem('shopez_token')).toBe('token123');
      expect(localStorage.getItem('shopez_email')).toBe('user@test.com');
      expect(localStorage.getItem('shopez_role')).toBe('Admin');
    });

    it('should clear session on logout', () => {
      service.setSession('token', 'email', 'User');
      service.logout();
      
      expect(localStorage.getItem('shopez_token')).toBeNull();
      expect(localStorage.getItem('shopez_email')).toBeNull();
      expect(localStorage.getItem('shopez_role')).toBeNull();
    });

    it('should return token from localStorage', () => {
      localStorage.setItem('shopez_token', 'my-token');
      expect(service.getToken()).toBe('my-token');
    });

    it('should return null when token not exists', () => {
      expect(service.getToken()).toBeNull();
    });

    it('should return email from localStorage', () => {
      localStorage.setItem('shopez_email', 'test@example.com');
      expect(service.getEmail()).toBe('test@example.com');
    });

    it('should return role from localStorage', () => {
      localStorage.setItem('shopez_role', 'Admin');
      expect(service.getRole()).toBe('Admin');
    });
  });

  describe('Authentication State', () => {
    it('should return true if user is logged in', () => {
      localStorage.setItem('shopez_token', 'token');
      expect(service.isLoggedIn()).toBeTrue();
    });

    it('should return false if user is not logged in', () => {
      expect(service.isLoggedIn()).toBeFalse();
    });

    it('should return true if user is admin', () => {
      localStorage.setItem('shopez_role', 'Admin');
      expect(service.isAdmin()).toBeTrue();
    });

    it('should return false if user is not admin', () => {
      localStorage.setItem('shopez_role', 'User');
      expect(service.isAdmin()).toBeFalse();
    });
  });
});
