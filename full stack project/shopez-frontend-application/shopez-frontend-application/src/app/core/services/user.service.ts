import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UserInfo } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getAllUsers(): Observable<UserInfo[]> {
    return this.http.get<UserInfo[]>(`${this.apiUrl}/api/Auth/users`);
  }

  changeUserRole(userId: number, role: 'Admin' | 'User'): Observable<any> {
    return this.http.put(`${this.apiUrl}/api/Auth/change-role/${userId}`, { role });
  }
}