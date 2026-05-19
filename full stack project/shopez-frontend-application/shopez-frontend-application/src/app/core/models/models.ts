// User Models
export interface RegisterRequest {
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  email: string;
  role: 'User' | 'Admin';
}

export interface AuthResponse {
  success: boolean;
  message: string;
  validation?: any;
}

// Product Models
export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  stock: number;
  category: string;
  imageUrl: string;
  createdAt?: string;
}

export interface ProductResponse {
  data: Product[];
}

export interface CreateProductRequest {
  name: string;
  description: string;
  price: number;
  stock: number;
  category: string;
  imageUrl: string;
}

// Cart Models
export interface CartItem {
  productId: number;
  name: string;
  price: number;
  quantity: number;
  imageUrl: string;
  stock: number;
  category?: string;
}

// Order Models
export interface OrderItem {
  productId: number;
  quantity: number;
  productName?: string;
  unitPrice?: number;
}

export interface CreateOrderRequest {
  items: OrderItem[];
}

export interface Order {
  orderId: number;
  userId: number;
  orderDate: string;
  totalAmount: number;
  status: string;
  items: OrderItemDetails[];
}

export interface OrderItemDetails {
  productId: number;
  productName: string;
  unitPrice: number;
  quantity: number;
}

export interface UserInfo {
  id: number;
  email: string;
  role: string;
}