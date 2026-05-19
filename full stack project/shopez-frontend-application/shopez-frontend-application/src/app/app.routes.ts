import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', loadComponent: () => import('./components/home/home.component').then(m => m.HomeComponent) },
  { path: 'login', loadComponent: () => import('./components/login/login.component').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./components/register/register.component').then(m => m.RegisterComponent) },
  { path: 'products', loadComponent: () => import('./components/products/products.component').then(m => m.ProductsComponent) },
  { path: 'product/:id', loadComponent: () => import('./components/product-details/product-details.component').then(m => m.ProductDetailsComponent) },
  { path: 'cart', loadComponent: () => import('./components/cart/cart.component').then(m => m.CartComponent), canActivate: [authGuard] },
  { path: 'checkout', loadComponent: () => import('./components/checkout/checkout.component').then(m => m.CheckoutComponent), canActivate: [authGuard] },
  { path: 'my-orders', loadComponent: () => import('./components/my-orders/my-orders.component').then(m => m.MyOrdersComponent), canActivate: [authGuard] },
  
  // Admin Routes
  { 
    path: 'admin', 
    canActivate: [authGuard, adminGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', loadComponent: () => import('./admin/admin-dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent) },
      { path: 'products', loadComponent: () => import('./admin/admin-products/admin-products.component').then(m => m.AdminProductsComponent) },
      { path: 'add-product', loadComponent: () => import('./admin/admin-add-product/admin-add-product.component').then(m => m.AdminAddProductComponent) },
      { path: 'edit-product/:id', loadComponent: () => import('./admin/admin-edit-product/admin-edit-product.component').then(m => m.AdminEditProductComponent) },
      { path: 'orders', loadComponent: () => import('./admin/admin-orders/admin-orders.component').then(m => m.AdminOrdersComponent) },
      { path: 'users', loadComponent: () => import('./admin/admin-users/admin-users.component').then(m => m.AdminUsersComponent) }
    ]
  },
  
  { path: '**', loadComponent: () => import('./components/page-not-found/page-not-found.component').then(m => m.PageNotFoundComponent) }
];