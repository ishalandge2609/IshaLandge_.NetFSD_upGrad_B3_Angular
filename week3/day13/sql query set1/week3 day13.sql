--problem statement1
/* Pre-Requisites: Before starting with problem solving, please make sure that you have created a database and store sample data	
Level-1: Problem 1 - Basic Customer Order Report
Scenario:
The store manager wants a simple report showing customer orders along with their order dates and status. This report will help track pending and completed orders.
📌 Requirements
1. Retrieve customer first name, last name, order_id, order_date, and order_status.
2. Display only orders with status Pending (1) or Completed (4).
3. Sort the results by order_date in descending order.

🛠️ Technical Constraints
- Use SELECT statement.
- Use WHERE clause with logical operators (AND/OR).
- Use ORDER BY clause.
- Use INNER JOIN between customers and orders tables.

Expectations:
Students should write a correct query using joins and filters, and properly order the result set.
🎯 Learning Outcome 
Understand basic SELECT queries, filtering using WHERE conditions, logical operators, and sorting using ORDER BY clause with INNER JOIN.
 */
CREATE DATABASE StoreDb;

USE StoreDb;



CREATE TABLE Customers
(
    customer_id INT PRIMARY KEY IDENTITY(1,1),
    first_name VARCHAR(50),
    last_name VARCHAR(50)
);


CREATE TABLE Orders
(
    order_id INT PRIMARY KEY IDENTITY(1,1),
    customer_id INT,
    order_date DATE,
    order_status INT,

    FOREIGN KEY (customer_id)
    REFERENCES Customers(customer_id)
);

-- INSERT SAMPLE DATA INTO CUSTOMERS
INSERT INTO Customers (first_name, last_name)
VALUES
('isha ','singh'),
('nisha','landge'),
('harsh','shukla'),
('gagandeep','singh biling');

-- INSERT SAMPLE DATA INTO ORDERS
INSERT INTO Orders (customer_id, order_date, order_status)
VALUES
(1,'2026-03-01',1),
(2,'2026-03-02',4),
(3,'2026-03-03',2),
(1,'2026-03-04',4),
(4,'2026-03-05',1);


SELECT 
    c.first_name,
    c.last_name,
    o.order_id,
    o.order_date,
    o.order_status
FROM Customers c
INNER JOIN Orders o
ON c.customer_id = o.customer_id
WHERE o.order_status = 1 OR o.order_status = 4
ORDER BY o.order_date DESC;



--problemstatement2

/* Level-1: Problem 2 - Product Price Listing by Category
Scenario:
The sales team wants a product listing categorized by product category along with brand details to understand product distribution.
📌 Requirements
1. Display product_name, brand_name, category_name, model_year, and list_price.
2. Filter products with list_price greater than 500.
3. Sort results by list_price in ascending order.

🛠️ Technical Constraints
- Use SELECT statement.
- Use WHERE clause.
- Use ORDER BY clause.
- Use INNER JOIN between products, brands, and categories tables.

Expectations:
Students should correctly join multiple tables and apply filtering and sorting logic.
🎯 Learning Outcome 
Learn multi-table joins, filtering numeric conditions, and sorting query results
 */


CREATE DATABASE ProductStoreDb;


USE ProductStoreDb;



CREATE TABLE Categories
(
    category_id INT PRIMARY KEY IDENTITY(1,1),
    category_name VARCHAR(50)
);


CREATE TABLE Brands
(
    brand_id INT PRIMARY KEY IDENTITY(1,1),
    brand_name VARCHAR(50)
);


CREATE TABLE Products
(
    product_id INT PRIMARY KEY IDENTITY(1,1),
    product_name VARCHAR(100),
    brand_id INT,
    category_id INT,
    model_year INT,
    list_price DECIMAL(10,2),

    FOREIGN KEY (brand_id) REFERENCES Brands(brand_id),
    FOREIGN KEY (category_id) REFERENCES Categories(category_id)
);




INSERT INTO Categories (category_name)
VALUES
('Electronics'),
('Furniture'),
('Sports');


INSERT INTO Brands (brand_name)
VALUES
('Samsung'),
('Nike'),
('Ikea'),
('Apple');


INSERT INTO Products (product_name, brand_id, category_id, model_year, list_price)
VALUES
('Samsung TV',1,1,2024,800),
('Nike Running Shoes',2,3,2023,150),
('Ikea Sofa',3,2,2022,600),
('Apple iPhone',4,1,2024,1200),
('Samsung Refrigerator',1,1,2023,450);



SELECT 
    p.product_name,
    b.brand_name,
    c.category_name,
    p.model_year,
    p.list_price
FROM Products p
INNER JOIN Brands b
ON p.brand_id = b.brand_id
INNER JOIN Categories c
ON p.category_id = c.category_id
WHERE p.list_price > 500
ORDER BY p.list_price ASC;




