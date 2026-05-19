# ShopEZ – Full Stack E-Commerce Application (Angular + .NET Microservices)

A complete **Full Stack E-Commerce Web Application** built using **Angular 17** for the frontend and **ASP.NET Core Web API (.NET 8) Microservices Architecture** for the backend.

The application simulates a modern **Home Decor E-Commerce Platform** where users can browse products, manage carts, place orders, and admins can manage inventory, orders, users, and stock through an Admin Dashboard.

The system follows a scalable **Microservices Architecture** using:

* Angular Frontend
* ASP.NET Core Web APIs
* Ocelot API Gateway
* JWT Authentication
* SQL Server
* Entity Framework Core
* Dapper ORM

Based on the uploaded frontend and backend documentation.  

---

# Table of Contents

* Features
* System Architecture
* Frontend Features
* Backend Microservices
* Tech Stack
* Project Structure
* How to Run the Full Stack Project
* Frontend Setup
* Backend Setup
* Database Setup
* API Gateway Configuration
* Authentication Flow
* Frontend Application Flow
* Backend API Modules
* Authorization Rules
* Testing
* Common Errors
* Future Scope
* Final Flow Summary

---

# Features

## Frontend Features

* Angular 17 Single Page Application (SPA)
* Responsive UI using Bootstrap
* JWT Authentication
* Angular Route Guards
* Product Search & Filtering
* Cart Management
* Checkout System
* Order History
* Admin Dashboard
* Dynamic Category Filtering
* Product Sorting
* Toast Notifications
* Quantity Management
* Protected Admin Routes

## Backend Features

* ASP.NET Core Web API (.NET 8)
* Microservices Architecture
* API Gateway using Ocelot
* JWT Authentication
* Role-Based Authorization
* Product CRUD Operations
* Order Management
* Stock Management
* User Role Management
* Pagination & Filtering APIs
* Swagger Documentation
* Global Exception Handling

---

# System Architecture

```text
Angular Frontend
        |
        v
API Gateway (Ocelot)
        |
------------------------------------------------
|                  |                           |
v                  v                           v

Auth Service    Product Service         Order Service
(Dapper)         (EF Core)               (EF Core)

        |                  |                    |
        v                  v                    v

   Auth DB          Product DB            Order DB
```

---

# Frontend Features

## Authentication Module

* User Registration
* User Login
* JWT Token Storage
* Logout Functionality
* Protected Route Access
* Role-Based Navigation

## Product Module

* Product Listing
* Product Details
* Search Functionality
* Category Filtering
* Product Sorting
* Related Products Recommendation

## Cart Module

* Add to Cart
* Remove from Cart
* Quantity Management
* Dynamic Cart Updates
* Shipping Calculation
* GST Calculation

## Checkout Module

* Contact Information Form
* Shipping Address Form
* Order Summary
* Cash on Delivery
* Form Validation

## Order Module

* Place Orders
* View My Orders
* Cancel Orders
* Order History

## Admin Module

* Admin Dashboard
* Product CRUD Operations
* Order Management
* User Management
* Stock Reduction
* Stock Restoration

---

# Backend Microservices

| Service         | Responsibility                   | Port |
| --------------- | -------------------------------- | ---- |
| API Gateway     | Request Routing & Authentication | 7070 |
| Auth Service    | Login, Registration, JWT         | 7182 |
| Product Service | Product & Stock Management       | 7210 |
| Order Service   | Order Processing                 | 7238 |

---

# Tech Stack

## Frontend Technologies

| Technology         | Purpose              |
| ------------------ | -------------------- |
| Angular 17         | Frontend Framework   |
| TypeScript         | Programming Language |
| Bootstrap          | Responsive UI        |
| RxJS               | Reactive Programming |
| Angular Router     | Routing              |
| Angular HttpClient | API Communication    |

## Backend Technologies

| Technology            | Purpose              |
| --------------------- | -------------------- |
| ASP.NET Core Web API  | Backend Development  |
| C#                    | Programming Language |
| SQL Server            | Database             |
| Entity Framework Core | ORM                  |
| Dapper                | Lightweight ORM      |
| Ocelot                | API Gateway          |
| JWT Authentication    | Security             |
| Swagger               | API Documentation    |

---

# Project Structure

## Frontend Structure

```bash
frontend/
│
├── src/
│   ├── app/
│   │   ├── admin/
│   │   ├── auth/
│   │   ├── cart/
│   │   ├── orders/
│   │   ├── products/
│   │   ├── shared/
│   │   ├── services/
│   │   ├── guards/
│   │   ├── interceptors/
│   │   └── models/
│   │
│   ├── assets/
│   └── environments/
│
├── angular.json
├── package.json
└── README.md
```

## Backend Structure

```bash
backend/
│
├── ApiGateway/
├── AuthService/
├── ProductService/
├── OrderService/
└── Shared/
```

---

# How to Run the Full Stack Project

## Step 1 – Extract ZIP Files

Extract:

* Frontend ZIP
* Backend ZIP

---

# Frontend Setup

## Install Dependencies

Open terminal inside frontend folder:

```bash
npm install
```

## Run Angular Frontend

```bash
ng serve
```

Frontend runs on:

```text
http://localhost:4200
```

---

# Backend Setup

## Open Solution in Visual Studio

Open the backend `.sln` file using:

* Visual Studio 2022

## Restore NuGet Packages

Wait for all dependencies to restore automatically.

---

# Database Setup

## Configure SQL Server Connection

Open `appsettings.json` in all services and update SQL Server connection strings.

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ShopEZDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

# Apply Database Migrations

Open:

```text
Tools → NuGet Package Manager → Package Manager Console
```

Run:

```bash
Update-Database
```

If migrations do not exist:

```bash
Add-Migration InitialCreate
Update-Database
```

---

# Run Backend Services

Start these projects:

* API Gateway
* Auth Service
* Product Service
* Order Service

---

# API Gateway Configuration

Base URL:

```text
https://localhost:7070
```

All frontend requests pass through the API Gateway.

Example:

```text
https://localhost:7070/api/products
```

---

# Authentication Flow

```text
Register → Login → JWT Token Generated → Token Stored →
Protected APIs Accessed → Role-Based Navigation Enabled
```

---

# Frontend Application Flow

```text
Homepage →
Browse Products →
Product Details →
Add To Cart →
Checkout →
Place Order →
My Orders
```

Admin Flow:

```text
Admin Login →
Dashboard →
Manage Products →
Manage Orders →
Manage Users →
Manage Inventory
```

---

# Backend API Modules

## Auth Service

* Register User
* Login User
* JWT Token Generation
* Get User Profile
* Change User Role

## Product Service

* Create Product
* Update Product
* Delete Product
* Get Products
* Search Products
* Pagination
* Stock Management

## Order Service

* Place Order
* Get Orders
* Cancel Orders
* Order History

---

# Authorization Rules

| Feature          | User  | Admin   |
| ---------------- | ----  | -----   |
| View Products    | ✅    | ✅     |
| Search Products  | ✅    | ✅     |
| Add To Cart      | ✅    | ✅     |
| Place Orders     | ✅    | ✅     |
| View My Orders   | ✅    | ✅     |
| Create Product   | ❌    | ✅     |
| Update Product   | ❌    | ✅     |
| Delete Product   | ❌    | ✅     |
| Manage Users     | ❌    | ✅     |
| Manage Orders    | ❌    | ✅     |
| Stock Management | ❌    | ✅     |

---

# Testing

## Frontend Testing

* Karma
* Jasmine
* Route Guard Testing
* Form Validation Testing
* Service Layer Testing

## Backend Testing

* xUnit Testing
* Moq Framework
* Swagger Testing
* Postman API Testing

---

# Common Errors

## `Could not get response`

Possible reasons:

* Backend services are not running
* Wrong API Gateway URL
* SQL Server connection issue
* JWT token missing
* Angular frontend not running

---

# Future Scope

* Payment Gateway Integration
* Docker Deployment
* Kubernetes Deployment
* CI/CD Pipeline
* Redis Caching
* Cloud Deployment
* Email Notifications
* Product Reviews & Ratings
* Wishlist Functionality

---

# Final Flow Summary

```text
Start Backend Services →
Run Angular Frontend →
Register/Login →
Browse Products →
Add To Cart →
Checkout →
Place Order →
Track Orders
```

---

# Developed By

**Isha Landge**
Cognizant uGE_Dot Net FSD (Angular)
Batch 3

Under the Guidance of
**Narasimha Sir**
