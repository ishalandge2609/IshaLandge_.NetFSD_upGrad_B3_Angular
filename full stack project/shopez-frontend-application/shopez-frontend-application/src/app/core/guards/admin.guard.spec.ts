import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { adminGuard } from './admin.guard';
import { AuthService } from '../services/auth.service';

describe('adminGuard', () => {
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(() => {
    mockAuthService = jasmine.createSpyObj('AuthService', ['isLoggedIn', 'isAdmin']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter }
      ]
    });
  });

  it('should allow access when user is logged in and is admin', () => {
    // Arrange
    mockAuthService.isLoggedIn.and.returnValue(true);
    mockAuthService.isAdmin.and.returnValue(true);

    // Act - Functional guards don't take parameters
    const result = TestBed.runInInjectionContext(() => adminGuard());

    // Assert
    expect(result).toBeTrue();
    expect(mockRouter.navigate).not.toHaveBeenCalled();
  });

  it('should redirect to home when user is logged in but not admin', () => {
    // Arrange
    mockAuthService.isLoggedIn.and.returnValue(true);
    mockAuthService.isAdmin.and.returnValue(false);

    // Act
    const result = TestBed.runInInjectionContext(() => adminGuard());

    // Assert
    expect(result).toBeFalse();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/home']);
  });

  it('should redirect to home when user is not logged in', () => {
    // Arrange
    mockAuthService.isLoggedIn.and.returnValue(false);
    mockAuthService.isAdmin.and.returnValue(false);

    // Act
    const result = TestBed.runInInjectionContext(() => adminGuard());

    // Assert
    expect(result).toBeFalse();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/home']);
  });
});
