import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { authGuard } from './auth.guard';
import { AuthService } from '../services/auth.service';

describe('authGuard', () => {
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(() => {
    mockAuthService = jasmine.createSpyObj('AuthService', ['isLoggedIn']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter }
      ]
    });
  });

  it('should allow access when user is logged in', () => {
    // Arrange
    mockAuthService.isLoggedIn.and.returnValue(true);

    // Act - Functional guards don't take parameters
    const result = TestBed.runInInjectionContext(() => authGuard());

    // Assert
    expect(result).toBeTrue();
    expect(mockRouter.navigate).not.toHaveBeenCalled();
  });

  it('should redirect to login when user is not logged in', () => {
    // Arrange
    mockAuthService.isLoggedIn.and.returnValue(false);

    // Act
    const result = TestBed.runInInjectionContext(() => authGuard());

    // Assert
    expect(result).toBeFalse();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
  });
});