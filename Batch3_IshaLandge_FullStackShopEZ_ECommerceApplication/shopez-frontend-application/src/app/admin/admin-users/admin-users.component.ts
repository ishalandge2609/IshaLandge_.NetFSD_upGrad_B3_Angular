import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../core/services/user.service';
import { AuthService } from '../../core/services/auth.service';
import { UserInfo } from '../../core/models/models';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './admin-users.component.html'
})
export class AdminUsersComponent implements OnInit {
  private userService = inject(UserService);
  private authService = inject(AuthService);
  private toastr = inject(ToastrService);
  
  users: UserInfo[] = [];
  filteredUsers: UserInfo[] = [];
  loading = true;
  searchQuery = '';

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.loading = true;
    this.userService.getAllUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.filteredUsers = [...users];
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toastr.error('Failed to load users');
      }
    });
  }

  filterUsers() {
    if (!this.searchQuery.trim()) {
      this.filteredUsers = [...this.users];
    } else {
      const query = this.searchQuery.toLowerCase();
      this.filteredUsers = this.users.filter(u => 
        u.email.toLowerCase().includes(query) ||
        u.role.toLowerCase().includes(query)
      );
    }
  }

  changeRole(user: UserInfo) {
    const newRole = user.role === 'Admin' ? 'User' : 'Admin';
    const action = newRole === 'Admin' ? 'promote to Admin' : 'demote to User';
    
    if (confirm(`Are you sure you want to ${action} ${user.email}?`)) {
      this.userService.changeUserRole(user.id, newRole).subscribe({
        next: () => {
          this.toastr.success(`${user.email} has been ${action}d successfully`);
          this.loadUsers();
        },
        error: () => this.toastr.error('Failed to change user role')
      });
    }
  }

  getRoleBadgeClass(role: string): string {
    return role === 'Admin' ? 'bg-danger' : 'bg-info';
  }
}