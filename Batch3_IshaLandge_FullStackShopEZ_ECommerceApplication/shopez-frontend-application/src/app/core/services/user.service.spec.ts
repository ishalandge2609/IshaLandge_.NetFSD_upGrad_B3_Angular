import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { UserService } from './user.service';
import { environment } from '../../../environments/environment';

describe('UserService', () => {
  let service: UserService;
  let httpMock: HttpTestingController;
  const apiUrl = environment.apiUrl;

  const mockUsers = [
    { id: 1, email: 'user1@test.com', role: 'User' },
    { id: 2, email: 'admin@test.com', role: 'Admin' }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [UserService]
    });
    
    service = TestBed.inject(UserService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getAllUsers', () => {
    it('should fetch all users', () => {
      service.getAllUsers().subscribe(users => {
        expect(users.length).toBe(2);
        expect(users[0].email).toBe('user1@test.com');
        expect(users[1].role).toBe('Admin');
      });
      
      const req = httpMock.expectOne(`${apiUrl}/api/Auth/users`);
      expect(req.request.method).toBe('GET');
      req.flush(mockUsers);
    });
  });

  describe('changeUserRole', () => {
    it('should send PUT request to change user role', () => {
      const userId = 1;
      const newRole = 'Admin';
      
      service.changeUserRole(userId, newRole).subscribe(response => {
        expect(response).toEqual({ success: true });
      });
      
      const req = httpMock.expectOne(`${apiUrl}/api/Auth/change-role/${userId}`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual({ role: newRole });
      req.flush({ success: true });
    });

    it('should demote admin to user', () => {
      const userId = 2;
      const newRole = 'User';
      
      service.changeUserRole(userId, newRole).subscribe();
      
      const req = httpMock.expectOne(`${apiUrl}/api/Auth/change-role/${userId}`);
      expect(req.request.body).toEqual({ role: 'User' });
      req.flush({ success: true });
    });
  });
});