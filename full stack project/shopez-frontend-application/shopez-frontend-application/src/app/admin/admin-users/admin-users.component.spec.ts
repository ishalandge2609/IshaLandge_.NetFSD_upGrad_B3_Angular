import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { AdminUsersComponent } from './admin-users.component';
import { UserService } from '../../core/services/user.service';
import { AuthService } from '../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { of, throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { provideRouter } from '@angular/router';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('AdminUsersComponent', () => {

  let component: AdminUsersComponent;
  let fixture: ComponentFixture<AdminUsersComponent>;

  let mockUserService: jasmine.SpyObj<UserService>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockToastr: jasmine.SpyObj<ToastrService>;

  const mockUsers = [
    {
      id: 1,
      email: 'john@example.com',
      role: 'User'
    },
    {
      id: 2,
      email: 'admin@example.com',
      role: 'Admin'
    }
  ];

  beforeEach(async () => {

    mockUserService = jasmine.createSpyObj(
      'UserService',
      ['getAllUsers', 'changeUserRole']
    );

    mockAuthService = jasmine.createSpyObj(
      'AuthService',
      ['getRole']
    );

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      ['success', 'error']
    );

    mockUserService.getAllUsers.and.returnValue(
      of(mockUsers)
    );

    await TestBed.configureTestingModule({
      imports: [
        AdminUsersComponent,
        FormsModule
      ],
      providers: [
        provideRouter([]),
        {
          provide: UserService,
          useValue: mockUserService
        },
        {
          provide: AuthService,
          useValue: mockAuthService
        },
        {
          provide: ToastrService,
          useValue: mockToastr
        }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(AdminUsersComponent);

    component = fixture.componentInstance;

  });

  it('should create', () => {

    expect(component).toBeTruthy();

  });

  it('should load users', fakeAsync(() => {

    component.ngOnInit();

    tick();

    expect(mockUserService.getAllUsers)
      .toHaveBeenCalled();

    expect(component.users.length)
      .toBe(2);

  }));

  it('should filter users', () => {

    component.users = mockUsers;

    component.filteredUsers = mockUsers;

    component.searchQuery = 'john';

    component.filterUsers();

    expect(component.filteredUsers.length)
      .toBe(1);

  });

  it('should change user role', fakeAsync(() => {

    mockUserService.changeUserRole.and.returnValue(
      of(void 0)
    );

    spyOn(window, 'confirm')
      .and.returnValue(true);

    component.changeRole(mockUsers[0]);

    tick();

    expect(mockUserService.changeUserRole)
      .toHaveBeenCalled();

  }));

  it('should handle role change error', fakeAsync(() => {

    mockUserService.changeUserRole.and.returnValue(
      throwError(() => new Error('Failed'))
    );

    spyOn(window, 'confirm')
      .and.returnValue(true);

    component.changeRole(mockUsers[0]);

    tick();

    expect(mockToastr.error)
      .toHaveBeenCalled();

  }));

  it('should return correct badge class', () => {

    expect(component.getRoleBadgeClass('Admin'))
      .toBe('bg-danger');

    expect(component.getRoleBadgeClass('User'))
      .toBe('bg-info');

  });

});