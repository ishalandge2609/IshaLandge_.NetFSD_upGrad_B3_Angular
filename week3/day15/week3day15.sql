CREATE DATABASE EcommDb;
USE EcommDb;
CREATE TABLE Categories
(
    category_id INT PRIMARY KEY IDENTITY(1,1),
    category_name VARCHAR(100) NOT NULL
);
INSERT INTO Categories (category_name) VALUES
('Mountain Bikes'),
('Road Bikes'),
('Electric Bikes'),
('Kids Bikes'),
('Accessories');

CREATE TABLE Brands
(
    brand_id INT PRIMARY KEY IDENTITY(1,1),
    brand_name VARCHAR(100) NOT NULL
);
INSERT INTO Brands (brand_name) VALUES
('Trek'),
('Giant'),
('Specialized'),
('Cannondale'),
('Scott');

CREATE TABLE Products
(
    product_id INT PRIMARY KEY IDENTITY(1,1),
    product_name VARCHAR(150) NOT NULL,
    brand_id INT,
    category_id INT,
    model_year INT,
    list_price DECIMAL(10,2),

    FOREIGN KEY (brand_id) REFERENCES Brands(brand_id),
    FOREIGN KEY (category_id) REFERENCES Categories(category_id)
);

INSERT INTO Products (product_name, brand_id, category_id, model_year, list_price) VALUES
('Trek Marlin 7',1,1,2023,850),
('Giant Defy Advanced',2,2,2022,1500),
('Specialized Turbo Vado',3,3,2024,3200),
('Cannondale Trail 5',4,1,2023,900),
('Scott Roxter 20',5,4,2021,450);

CREATE TABLE Customers
(
    customer_id INT PRIMARY KEY IDENTITY(1,1),
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    email VARCHAR(150),
    city VARCHAR(100)
);

INSERT INTO Customers (first_name,last_name,email,city) VALUES
('Rahul','Sharma','rahul@gmail.com','Delhi'),
('isha','Landge','anjali@gmail.com','Hyderabad'),
('Gagandeep','singh','kiran@gmail.com','Bangalore'),
('Priya','Mehta','priya@gmail.com','Delhi'),
('Arjun','Varma','arjun@gmail.com','Chennai');


CREATE TABLE Stores
(
    store_id INT PRIMARY KEY IDENTITY(1,1),
    store_name VARCHAR(150),
    city VARCHAR(100)
);

INSERT INTO Stores (store_name,city) VALUES
('Auto Bike Store','Delhi'),
('Speed Wheels','Hyderabad'),
('Urban Riders','Bangalore'),
('Bike World','Chennai'),
('Power Motors','Mumbai');

------- Write SELECT queries to retrieve all products with their brand and category names.

SELECT 
p.product_name,
b.brand_name,
c.category_name,
p.model_year,
p.list_price
FROM Products p
JOIN Brands b
ON p.brand_id = b.brand_id
JOIN Categories c
ON p.category_id = c.category_id;

--------- Retrieve all customers from a specific city.

SELECT * FROM Customers WHERE city = 'Delhi';

-------- Display total number of products available in each category.

SELECT 
c.category_name,
COUNT(p.product_id) AS TotalProducts
FROM Categories c
LEFT JOIN Products p
ON c.category_id = p.category_id
GROUP BY c.category_name;



----------------------2ND QUESTION

--------Create a view that shows product name, brand name, category name, model year and list price.
CREATE VIEW vw_ProductDetails
AS
SELECT 
p.product_name,
b.brand_name,
c.category_name,
p.model_year,
p.list_price
FROM Products p
JOIN Brands b
ON p.brand_id = b.brand_id
JOIN Categories c
ON p.category_id = c.category_id;

SELECT * FROM vw_ProductDetails

----- Create a view that shows order details with customer name, store name and staff name.

CREATE TABLE Staffs
(
    staff_id INT PRIMARY KEY IDENTITY(1,1),
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    email VARCHAR(150),
    store_id INT,

    FOREIGN KEY (store_id) REFERENCES Stores(store_id)
);

INSERT INTO Staffs(first_name,last_name,email,store_id) VALUES
('Ravindra','Landge','ravi@gmail.com',1),
('Anita','Mate','suresh@gmail.com',2),
('Mimansha','jha','meena@gmail.com',3),
('Vikas','kumar','vikram@gmail.com',4),
('Pooja','sharma','pooja@gmail.com',5);

CREATE TABLE Orders
(
    order_id INT PRIMARY KEY IDENTITY(1001,1),
    customer_id INT,
    order_status INT,
    order_date DATE,
    store_id INT,
    staff_id INT,

    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    FOREIGN KEY (store_id) REFERENCES Stores(store_id),
    FOREIGN KEY (staff_id) REFERENCES Staffs(staff_id)
);

INSERT INTO Orders(customer_id,order_status,order_date,store_id,staff_id) VALUES
(1,4,'2023-01-10',1,1),
(2,4,'2023-02-12',2,2),
(3,1,'2023-03-05',3,3),
(4,4,'2023-04-15',4,4),
(5,1,'2023-05-20',5,5);

CREATE TABLE Order_Items
(
    order_id INT,
    item_id INT,
    product_id INT,
    quantity INT,
    list_price DECIMAL(10,2),
    discount DECIMAL(4,2),

    PRIMARY KEY(order_id,item_id),

    FOREIGN KEY(order_id) REFERENCES Orders(order_id),
    FOREIGN KEY(product_id) REFERENCES Products(product_id)
);

INSERT INTO Order_Items(order_id,item_id,product_id,quantity,list_price,discount) VALUES
(1001,1,1,2,850,0.10),
(1002,1,2,1,1500,0.05),
(1003,1,3,1,3200,0.15),
(1004,1,4,3,900,0.10),
(1005,1,5,2,450,0.05);


CREATE VIEW vw_OrderSummary
AS
SELECT
    o.order_id,
    c.first_name + ' ' + c.last_name AS CustomerName,
    s.store_name,
    st.first_name + ' ' + st.last_name AS StaffName,
    o.order_date,
    o.order_status
FROM orders o
JOIN customers c
    ON o.customer_id = c.customer_id
JOIN stores s
    ON o.store_id = s.store_id
JOIN staffs st
    ON o.staff_id = st.staff_id;
GO

SELECT * FROM vw_OrderSummary;


------------ Create appropriate indexes on foreign key columns.


--Index on Orders Table

CREATE INDEX idx_orders_customer_id
ON Orders(customer_id);

CREATE INDEX idx_orders_store_id
ON Orders(store_id);

CREATE INDEX idx_orders_staff_id
ON Orders(staff_id);


---Index on Staffs Table

CREATE INDEX idx_staffs_store_id
ON Staffs(store_id);



-----------unique index on customers table
CREATE UNIQUE INDEX idx_customers_email
ON Customers(email);

--------------------- Test performance improvement using execution plan.


SELECT 
o.order_id,
c.first_name,
s.store_name
FROM Orders o
JOIN Customers c
ON o.customer_id = c.customer_id
JOIN Stores s
ON o.store_id = s.store_id;






