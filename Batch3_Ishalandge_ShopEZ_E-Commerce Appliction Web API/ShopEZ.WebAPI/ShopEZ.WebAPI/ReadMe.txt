# ShopEZ – E-Commerce Backend API (Home Decor)

A complete **E-Commerce Backend API** built using **ASP.NET Core Web API (.NET 8)**, simulating a Home Decor Store where users can browse products and admins can manage inventory.

-------------------------------------------------------------------------------------------------------------------------------------------------

##                                                            Table of Contents

- [Features](#features)
- [How to Run the Project](#how-to-run-the-project)
- [Database Setup](#database-setup)
- [Authentication Flow](#authentication-flow)
- [Product APIs](#product-apis)
- [Order APIs](#order-apis)
- [Pagination, Search & Filtering](#pagination-search--filtering)
- [Authorization Rules](#authorization-rules)
- [Common Errors](#common-errors)
- [Useful SQL Commands](#useful-sql-commands-database-verification)
- [Status Code Reference](#status-code-reference)

------------------------------------------------------------------------------------------------------------------------------------------------

##                                                               Features

- JWT Authentication
- Role-Based Authorization (Admin / User)
- Product Management (Home Decor Items)
- Order Management (Cart → Order)
- Pagination, Search & Filtering on Products
- Global Exception Handling
- Clean Layered Architecture

-----------------------------------------------------------------------------------------------------------------------------------------------------

##                                            How to Run the Project
-----------------------------------------------------------------------------------------------------------------------------------------------------
###                                          Option 1 – Run Using ZIP File
-----------------------------------------------------------------------------------------------------------------------------------------------------
1. Extract the ZIP file
2. Open the folder in **Visual Studio**
3. Open the `.slnx` file
4. Wait for all dependencies to restore
-------------------------------------------------------------------------------------------------------------------------------------------------------
###                                            Option 2 – Run Using GitHub
---------------------------------------------------------------------------------------------------------------------------------------------------
```bash
git clone <your-repo-url>
```

Then open the `.slnx` file in Visual Studio.

------------------------------------------------------------------------------------------------------------------------------------------------
##                                            Database Setup
---------------------------------------------------------------------------------------------------------------------------------------------------
###                                    Step 1: Configure Connection String
----------------------------------------------------------------------------------------------------------------------------------------------------
Open `appsettings.json` and replace the server name with your SQL Server instance name (found in SQL Server Management Studio → Connection Properties):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ShopEZDb;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True"
},

"Jwt": {
  "Key": "ThisIsMySuperSecretKey12345577777777777328722103342444343423422",
  "Issuer": "ShopEZ",
  "Audience": "ShopEZUsers",
  "DurationInMinutes": 60
}
```
--------------------------------------------------------------------------------------------------------------------------------------------------
###                                             Step 2: Apply Migrations
---------------------------------------------------------------------------------------------------------------------------------------------------
Go to **Tools → NuGet Package Manager → Package Manager Console**

**If a `Migrations` folder already exists:**

```bash
Update-Database
```

**If no `Migrations` folder exists:**

```bash
Add-Migration InitialCreate
Update-Database
```

--------------------------------------------------------------------------------------------------------------------------------------------------
##                                                   Run the Project
--------------------------------------------------------------------------------------------------------------------------------------------------
Press **F5** or use the Run button in Visual Studio. Swagger UI will open automatically.

> **Base URL:** `https://localhost:7120/`  

----------------------------------------------------------------------------------------------------------------------------------------------------
##                                                  Authentication Flow
--------------------------------------------------------------------------------------------------------------------------------------------------
```
Register → Login → Copy Token → Authorize → Use APIs
```
--------------------------------------------------------------------------------------------------------------------------------------------------
###                                                Sample Credentials
--------------------------------------------------------------------------------------------------------------------------------------------------
####                                                 Admin Account
---------------------------------------------------------------------------------------------------------------------------------------------------
```json
{
  "name": "Isha",
  "email": "ishalandge9811@gmail.com",
  "password": "Isha@9091",
  "role": "Admin"
}
```

> ⚠️ The `"role"` field must be exactly `"Admin"` (case-sensitive).
---------------------------------------------------------------------------------------------------------------------------------------------------
####                                                    User Account

```json
{
  "name": "Nisha Landge",
  "email": "nishalandge48@gmail.com",
  "password": "Nisha@48",
  "role": "User"
}
```

> ⚠️ The `"role"` field must be exactly `"User"` (case-sensitive).

---------------------------------------------------------------------------------------------------------------------------------------------------
### Register – Admin
---------------------------------------------------------------------------------------------------------------------------------------------------
**Method:** `POST`  
**URL:** `https://localhost:7120/api/auth/register`  
**Authorization:** Not required

**Request Body:**
```json
{
  "name": "Isha",
  "email": "ishalandge9811@gmail.com",
  "password": "Isha@9091",
  "role": "Admin"
}
```

**Responses:**

| Status             | Message                              |
|--------------------|------------------------------------- |
| `200 OK`           | `User registered successfully`       |
| `400 Bad Request`  | `User already exists, please login`  |

---------------------------------------------------------------------------------------------------------------------------------------------------
###                                                            Register – User
-------------------------------------------------------------------------------------------------------------------------------------------------
**Method:** `POST`  
**URL:** `https://localhost:7120/api/auth/register`  
**Authorization:** Not required

**Request Body:**
```json
{
  "name": "Nisha Landge",
  "email": "nishalandge48@gmail.com",
  "password": "Nisha@48",
  "role": "User"
}
```

**Responses:**

| Status             | Message                              |
|--------------------|------------------------------------- |
| `200 OK`           | `User registered successfully`       |
| `400 Bad Request`  | `User already exists, please login`  |


---------------------------------------------------------------------------------------------------------------------------------------------------
###                                                      Login – Admin
--------------------------------------------------------------------------------------------------------------------------------------------------
**Method:** `POST`  
**URL:** `https://localhost:7120/api/auth/login`  
**Authorization:** Not required

**Request Body:**
```json
{
  "email": "ishalandge9811@gmail.com",
  "password": "Isha@9091"
}
```

**Response:**
```json
{
  "token": "YOUR_JWT_TOKEN"
}
```

> ⚠️ **Copy this token — it is required for all subsequent API calls.**

--------------------------------------------------------------------------------------------------------------------------------------------------
### Login – User
----------------------------------------------------------------------------------------------------------------------------------------------------
**Method:** `POST`  
**URL:** `https://localhost:7120/api/auth/login`  
**Authorization:** Not required

**Request Body:**
```json
{
  "email": "nishalandge48@gmail.com",
  "password": "Nisha@48"
}
```

**Response:**
```json
{
  "token": "YOUR_JWT_TOKEN"
}
```

> ⚠️ **Copy this token — it is required for all subsequent API calls.**
------------------------------------------------------------------------------------------------------------------------------------------------
**Error Response (invalid credentials):**
---------------------------------------------------------------------------------------------------------------------------------------------------
if the user enters an incorrect email or password during login, the API will return a `401 Unauthorized` status with the following message:
```json
{
  "Message": "Invalid email or password",
  "StatusCode": 401
}
```

------------------------------------------------------------------------------------------------------------------------------------------------

### How to Add the JWT Token in Postman

1. Go to the **Authorization** tab
2. Set **Type** → `Bearer Token`
3. Paste your copied token in the **Token** field

> ❌ Do **NOT** wrap the token in quotes.

-----------------------------------------------------------------------------------------------------------------------------------------------------

##                                                                Product APIs
----------------------------------------------------------------------------------------------------------------------------------------------------
###                                                    Create Product *(Admin Only)*
--------------------------------------------------------------------------------------------------------------------------------------------------
**Method:** `POST`  
**URL:** `https://localhost:7120/api/product`  
**Authorization:** Admin JWT Token required

**Sample Request Bodies:**

```json
{
  "name": "Wooden Wall Shelf",
  "description": "Modern floating shelf for living room decor",
  "price": 2499,
  "imageUrl": "shelf.jpg",
  "stock": 10
}
```

```json
{
  "name": "Modern Wooden Centre Table",
  "description": "Elegant wooden centre table with smooth finish, sturdy legs, perfect for modern living rooms.",
  "price": 24999,
  "imageUrl": "images/centre-table.jpg",
  "stock": 5
}
```

```json
{
  "name": "Modern Floor Lamp",
  "description": "Slim metal stand floor lamp with warm light, perfect for living room corners.",
  "price": 9000,
  "imageUrl": "images/floor-lamp.jpg",
  "stock": 10
}
```

```json
{
  "name": "Indoor Monstera Plant",
  "description": "Low-maintenance indoor plant in ceramic pot, perfect for home decor.",
  "price": 6999,
  "imageUrl": "images/monstera.jpg",
  "stock": 15
}
```

```json
{
  "name": "Premium Sofa Set",
  "description": "Comfortable fabric sofa set with wooden frame and modern design.",
  "price": 32000,
  "imageUrl": "images/sofa.jpg",
  "stock": 3
}
```

```json
{
  "name": "Bedside Table Lamp",
  "description": "Minimal lamp with warm light, ideal for bedroom ambiance.",
  "price": 1899,
  "imageUrl": "images/bedside-lamp.jpg",
  "stock": 20
}
```

```json
{
  "name": "Modern Ceramic Vase",
  "description": "Minimal donut-shaped vase for aesthetic interiors.",
  "price": 799,
  "imageUrl": "images/vase.jpg",
  "stock": 18
}
```

**Responses:**

| Status               | Message                                              |
|----------------------|------------------------------------------------------|
| `201 Created`        | `Product created successfully`                       |
| `401 Unauthorized`   | Missing or incorrect JWT token (must use Admin token)|

---------------------------------------------------------------------------------------------------------------------------------------------------

###                                                   Get All Products *(Admin & User)*
------------------------------------------------------------------------------------------------------------------------------------------------------
**Method:** `GET`  
**URL:** `https://localhost:7120/api/product`  
**Authorization:** Admin or User JWT Token required

**Responses:**

| Status              | Message                        |
|---------------------|--------------------------------|
| `200 OK`            | Returns list of all products   |
| `401 Unauthorized` | Missing or incorrect JWT token  |

---------------------------------------------------------------------------------------------------------------------------------------------------
###                                                   Get Product by ID *(Admin & User)*
-------------------------------------------------------------------------------------------------------------------------------------------------
**Method:** `GET`  
**URL:** `https://localhost:7120/api/product/{id}`  
**Example:** `https://localhost:7120/api/product/1`  
**Authorization:** Admin or User JWT Token required

**Responses:**

| Status               | Message                        |
|----------------------|--------------------------------|
| `200 OK`             | Returns product details        |
| `401 Unauthorized`  | Missing or incorrect JWT token  |

-----------------------------------------------------------------------------------------------------------------------------------------------------
###                                                    Update Product *(Admin Only)*
---------------------------------------------------------------------------------------------------------------------------------------------------
**Method:** `PUT`  
**URL:** `https://localhost:7120/api/product/{id}`  
**Example:** `https://localhost:7120/api/product/1`  
**Authorization:** Admin JWT Token required

**Sample Request Bodies:**

```json
{
  "name": "Decorative Table Lamp",
  "description": "Elegant lamp for bedroom decor",
  "price": 1899,
  "imageUrl": "lamp.jpg",
  "stock": 5
}
```

```json
{
  "name": "LED Bathroom Mirror",
  "description": "Round LED mirror with anti-fog feature and touch controls.",
  "price": 5999,
  "imageUrl": "images/mirror.jpg",
  "stock": 6
}
```

```json
{
  "name": "Wall Mounted Wooden Shelf",
  "description": "Minimal floating shelf perfect for displaying decor items.",
  "price": 2499,
  "imageUrl": "images/shelf.jpg",
  "stock": 12
}
```

**Responses:**

| Status              | Message                       |
|---------------------|-------------------------------|
| `200 OK`            | `Product updated successfully`|
| `404 Not Found`     | `Product with ID not found`   |
| `401 Unauthorized`  | Missing or incorrect JWT token|

----------------------------------------------------------------------------------------------------------------------------------------------------
###                                             Delete Product *(Admin Only)*
----------------------------------------------------------------------------------------------------------------------------------------------------
**Method:** `DELETE`  
**URL:** `https://localhost:7120/api/product/{id}`  
**Example:** `https://localhost:7120/api/product/1`  
**Authorization:** Admin JWT Token required

**Responses:**

| Status               | Message                      |
|----------------------|------------------------------|
| `200 OK`             |`Product deleted successfully`|
| `404 Not Found`      | `Product with ID not found`  |
| `401 Unauthorized`   |Missing or incorrect JWT token|

> 💡 **Note:** Create, Update, and Delete endpoints do not return data (industry standard). Use the GET API to verify changes.

---------------------------------------------------------------------------------------------------------------------------------------------------
##                                                                  Order APIs
---------------------------------------------------------------------------------------------------------------------------------------------------
###                                                           Create Order *(Admin & User)*
---------------------------------------------------------------------------------------------------------------------------------------------------
**Method:** `POST`  
**URL:** `https://localhost:7120/api/order`  
**Authorization:** Admin or User JWT Token required

**Request Body:**
```json
{
  "userId": 1,
  "items": [
    {
      "productId": 1,
      "quantity": 4
    },
    {
      "productId": 2,
      "quantity": 1
    }
  ]
}
```

> ⚠️ Ensure all `productId` values exist. Use the Get All Products API to verify IDs before placing an order.

**Error – Invalid Product ID:**
```json
{
  "StatusCode": 500,
  "Message": "Product with ID 56 not found"
}
```

**Responses:**

| Status             | Message                       |
|--------------------|-------------------------------|
| `201 Created`      | `Order placed successfully`   |
| `401 Unauthorized` | Missing or incorrect JWT token|

--------------------------------------------------------------------------------------------------------------------------------------------------
###                                                Get All Orders *(Admin Only)*
------------------------------------------------------------------------------------------------------------------------------------------------------
**Method:** `GET`  
**URL:** `https://localhost:7120/api/order`  
**Authorization:** Admin JWT Token required

**Responses:**

| Status | Message |
|--------|---------|
| `200 OK` | `All orders fetched successfully` |
| `401 Unauthorized` | Missing or incorrect JWT token |
| `403 Forbidden` | User token used — Admin access required |

-----------------------------------------------------------------------------------------------------------------------------------------------------
###                                           Get Order by ID *(Admin & User)*
------------------------------------------------------------------------------------------------------------------------------------------------------
**Method:** `GET`  
**URL:** `https://localhost:7120/api/order/{id}`  
**Example:** `https://localhost:7120/api/order/1`  
**Authorization:** Admin or User JWT Token required

**Responses:**

| Status             | Message                          |
|--------------------|----------------------------------|
| `200 OK`           | `Order retrieved successfully`   |
| `404 Not Found`    | `Order with ID not found`        |
| `401 Unauthorized` | Missing or incorrect JWT token   |

-----------------------------------------------------------------------------------------------------------------------------------------------------
##                                                  Pagination, Search & Filtering
-------------------------------------------------------------------------------------------------------------------------------------------------------
The Product API supports advanced querying via query parameters.  
**Endpoint:** `GET https://localhost:7120/api/product`

----------------------------------------------------------------------------------------------------------------------------------------------------
###                                             Authorization for Product APIs
----------------------------------------------------------------------------------------------------------------------------------------------------
#### Why Authorization is NOT Required for Pagination, Search, and Filtering

The following operations — **Pagination**, **Search (by product name)**, and **Filtering (by price)** —
are all implemented under the `GET /api/product` endpoint.
These operations do not require authentication or authorization because they are **read-only** operations.
--------------------------------------------------------------------------------------------------------------------------------------------------
####                                                    Reasoning
---------------------------------------------------------------------------------------------------------------------------------------------------
In real-world e-commerce applications:

- Users can browse products without logging in
- Searching and filtering are part of the standard browsing experience
- These operations do not modify any data

Therefore, exposing these APIs publicly improves user experience and aligns with industry standards.
----------------------------------------------------------------------------------------------------------------------------------------------------
####                                         Technical Implementation
----------------------------------------------------------------------------------------------------------------------------------------------------
This is achieved by applying the following attribute on the GET endpoint in `ProductController`:

```csharp
[AllowAnonymous]
```

This attribute overrides the controller-level `[Authorize]` and allows public access to the GET endpoint.

#### Security Consideration

While read operations are public, all write operations remain protected:

- **Create Product** → Admin only
- **Update Product** → Admin only
- **Delete Product** → Admin only

These endpoints are protected using:

```csharp
[Authorize(Roles = "Admin")]
```

#### Authorization Summary

| Operation        | Authorization Required |
|------------------|------------------------|
| View Products    | ❌ No                  |
| Search Products  | ❌ No                  |
| Filter Products  | ❌ No                  |
| Pagination       | ❌ No                  |
| Create Product   | ✅ Admin Only          |
| Update Product   | ✅ Admin Only          |
| Delete Product   | ✅ Admin Only          |

> **Conclusion:** Pagination, search, and filtering are public APIs because they are non-sensitive, read-only operations that enhance usability without compromising security.

------------------------------------------------------------------------------------------------------------------------------------------------------
###                                                          1. Pagination
--------------------------------------------------------------------------------------------------------------------------------------------------
Limits the number of records returned per request.

| Parameter    | Description           | Example |
|--------------|-----------------------|---------|
| `pageNumber` | Page number to fetch  | `1`     |
| `pageSize`   | Records per page      | `3`     |

**Example Request URL:**
```
https://localhost:7120/api/product?pageNumber=1&pageSize=3
```

> ✅ No Authorization required — public endpoint.

---------------------------------------------------------------------------------------------------------------------------------------------------------
###                                                    2. Search (By Product Name)
------------------------------------------------------------------------------------------------------------------------------------------------------
| Parameter | Description              | Example              |
|-----------|--------------------------|----------------------|
| `search` | Keyword to search by name | `sofa`               |

**Example Request URL:**
```
https://localhost:7120/api/product?search=sofa
```

> Search is case-insensitive and supports partial name matching.  
> ✅ No Authorization required — public endpoint.

----------------------------------------------------------------------------------------------------------------------------------------------------
###                                            3. Filtering (By Price Range)
---------------------------------------------------------------------------------------------------------------------------------------------------
| Parameter  | Description ```| Example |
|------------|----------------|---------|
| `minPrice` | Minimum price | `1000`  |
| `maxPrice` | Maximum price | `10000` |

**Example Request URL:**
```
https://localhost:7120/api/product?minPrice=1000&maxPrice=10000
```

> ✅ No Authorization required — public endpoint.

----------------------------------------------------------------------------------------------------------------------------------------------------
###                                         4. Combined Query (Real-World Scenario)
----------------------------------------------------------------------------------------------------------------------------------------------------
All parameters can be combined in a single request:

| Parameter   -| Example |
|--------------|---------|
| `pageNumber` | `1`     |
| `pageSize`   | `2`     |
| `search`     | `lamp`  |
| `minPrice`   | `1000`  |
| `maxPrice`   | `10000` |

**Example Request URL:**
```
https://localhost:7120/api/product?pageNumber=1&pageSize=2&search=lamp&minPrice=1000&maxPrice=10000
```

**Sample Response:**
```json
{
  "success": true,
  "message": "Products fetched successfully",
  "data": {
    "totalRecords": 10,
    "pageNumber": 1,
    "pageSize": 2,
    "data": [
      {
        "productId": 1,
        "name": "Modern Floor Lamp",
        "price": 9000
      }
    ]
  },
  "statusCode": 200
}
```

> **Note:** Pagination is applied **after** filtering and searching. If no parameters are provided, all products are returned.

**Status Codes for Pagination/Search/Filter:**

| Code   | Meaning                        |
|--------|--------------------------------|
| `200`  | Products fetched successfully  |
| `400`  | Invalid query parameters       |
| `500`  | Internal server error          |

-----------------------------------------------------------------------------------------------------------------------------------------------------
###                                                 How to Use in Postman
---------------------------------------------------------------------------------------------------------------------------------------------------
1. Select **GET** method
2. Enter the URL: `https://localhost:7120/api/product`
3. Go to the **Params** tab
4. Add the required query parameters (`pageNumber`, `search`, `minPrice`, etc.)
5. No Authorization token needed for these endpoints
6. Click **Send**
6. Click **Send**

-----------------------------------------------------------------------------------------------------------------------------------------------------

## Authorization Rules

| Role | Permissions                  |
|------|------------------------------|
| Admin| Create, Read, Update, Delete |
| User | Read Only                    |

**If a User attempts an Admin-only action:**

- Status: `403 Forbidden`
- Reason: Insufficient authorization

---------------------------------------------------------------------------------------------------------------------------------------------------
##                                                                   Common Errors
------------------------------------------------------------------------------------------------------------------------------------------------------
###                                                `could not get response – socket hang up`
-----------------------------------------------------------------------------------------------------------------------------------------------------
Most likely causes:
- The API project is not running
- The project crashed due to an unhandled exception
- Wrong port number in the URL
- HTTP vs HTTPS mismatch in the request URL
- A runtime code error stopped the application

-----------------------------------------------------------------------------------------------------------------------------------------------------

## Status Code Reference

| Code  | Meaning               |
|-------|---------------------- |
| `200` | Success               |
| `201` | Created               |
| `400` | Bad Request           |
| `401` | Unauthorized          |
| `403` | Forbidden             |
| `404` | Not Found             |
| `500` | Internal Server Error |

----------------------------------------------------------------------------------------------------------------------------------------------------

## Useful SQL Commands (Database Verification)

The following SQL commands can be run directly in **SQL Server Management Studio (SSMS)** to inspect or reset data in the `ShopEZDb` database during testing.

```sql
USE ShopEZDb;
```

---

### Products Table

```sql
-- View a specific product by ID
SELECT * FROM Products WHERE ProductId = 2;

-- View all products
SELECT * FROM Products;

-- Delete all products
DELETE FROM Products;

-- Reset auto-increment ID back to 0
DBCC CHECKIDENT ('Products', RESEED, 0);
```

---

### Users Table

```sql
-- Insert a new user manually
INSERT INTO Users (Name, Email, Password, Role)
VALUES ('', '', '', '');

-- View all users
SELECT * FROM Users;

-- Delete all users
DELETE FROM Users;

-- Reset auto-increment ID back to 0
DBCC CHECKIDENT ('Users', RESEED, 0);
```

---

### Orders Table

```sql
-- View all orders
SELECT * FROM Orders;

-- Delete all orders
DELETE FROM Orders;

-- Reset auto-increment ID back to 0
DBCC CHECKIDENT ('Orders', RESEED, 0);
```

---

### OrderItems Table

```sql
-- View all order items
SELECT * FROM OrderItems;

-- Delete all order items
DELETE FROM OrderItems;

-- Reset auto-increment ID back to 0
DBCC CHECKIDENT ('OrderItems', RESEED, 0);
```

---

### Quick Reference – View Product IDs

```sql
-- Useful before placing an order to verify valid ProductIds
SELECT ProductId, Name FROM Products;
```

>  **Note:** When clearing all data, delete in this order to avoid foreign key constraint errors:
> `OrderItems` → `Orders` → `Products` → `Users`

---

## Final Flow Summary

```
Register → Login → Copy Token → Authorize → Use APIs
```
