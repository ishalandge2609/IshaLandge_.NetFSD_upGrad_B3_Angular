--problem statement1
/* Pre-Requisites: Before starting with problem solving, please make sure that you have created a database and restored data  
AutoDb – SQL Problem Definitions Based on Advanced Querying and Data Manipulation

Level-1: Problem 1 – Product Analysis Using Nested Queries

Scenario:
You are working as a database developer for an automobile retail company. Management wants to identify products that are priced higher than the average price of products in their respective categories.

📌 Requirements
1. Retrieve product details (product_name, model_year, list_price).
2. Compare each product’s price with the average price of products in the same category using a nested query.
3. Display only those products whose price is greater than the category average.
4. Show calculated difference between product price and category average.
5. Concatenate product name and model year as a single column (e.g., 'ProductName (2017)').

🛠️ Technical Constraints
• Use subquery in WHERE clause.
• Use aggregate function (AVG).
• Use string manipulation functions.
• Use arithmetic expressions for price difference calculation.


Expectations:
• Proper use of nested query.
• Correct calculation of average and difference.
• Clean and readable SQL query.

🎯 Learning Outcome
• Understand nested queries with aggregate functions.
• Perform calculations inside SELECT statement.
• Apply string concatenation in SQL.
 */
CREATE DATABASE AutoDb;



USE AutoDb;




CREATE TABLE Categories
(
    category_id INT PRIMARY KEY IDENTITY(1,1),
    category_name VARCHAR(100)
);


CREATE TABLE Products
(
    product_id INT PRIMARY KEY IDENTITY(1,1),
    product_name VARCHAR(100),
    category_id INT,
    model_year INT,
    list_price DECIMAL(10,2),

    FOREIGN KEY (category_id) REFERENCES Categories(category_id)
);





INSERT INTO Categories (category_name)
VALUES
('Sedan'),
('SUV'),
('Hatchback');


INSERT INTO Products (product_name, category_id, model_year, list_price)
VALUES
('Honda City',1,2022,15000),
('Toyota Corolla',1,2021,18000),
('Hyundai Verna',1,2023,16000),
('Toyota Fortuner',2,2022,35000),
('Ford Endeavour',2,2021,32000),
('Mahindra XUV700',2,2023,30000),
('Maruti Swift',3,2022,9000),
('Hyundai i20',3,2023,11000),
('Tata Altroz',3,2021,10000);



SELECT 
    p.product_name + ' (' + CAST(p.model_year AS VARCHAR) + ')' AS product_details,
    p.model_year,
    p.list_price,
    
    -- Calculate price difference from category average
    p.list_price - 
    (
        SELECT AVG(p2.list_price)
        FROM Products p2
        WHERE p2.category_id = p.category_id
    ) AS price_difference

FROM Products p

-- NESTED QUERY IN WHERE CLAUSE
WHERE p.list_price >
(
    SELECT AVG(p3.list_price)
    FROM Products p3
    WHERE p3.category_id = p.category_id
);



--problemstatement2

/* Pre-Requisites: Before starting with problem solving, please make sure that you have created a database and restored data  
AutoDb – SQL Problem Definitions Based on Advanced Querying and Data Manipulation

Level-1: Problem 1 – Product Analysis Using Nested Queries

Scenario:
You are working as a database developer for an automobile retail company. Management wants to identify products that are priced higher than the average price of products in their respective categories.

📌 Requirements
1. Retrieve product details (product_name, model_year, list_price).
2. Compare each product’s price with the average price of products in the same category using a nested query.
3. Display only those products whose price is greater than the category average.
4. Show calculated difference between product price and category average.
5. Concatenate product name and model year as a single column (e.g., 'ProductName (2017)').

🛠️ Technical Constraints
• Use subquery in WHERE clause.
• Use aggregate function (AVG).
• Use string manipulation functions.
• Use arithmetic expressions for price difference calculation.


Expectations:
• Proper use of nested query.
• Correct calculation of average and difference.
• Clean and readable SQL query.

🎯 Learning Outcome
• Understand nested queries with aggregate functions.
• Perform calculations inside SELECT statement.
• Apply string concatenation in SQL.
 */



CREATE DATABASE problemstatementDb;


USE problemstatementDb;




-- CUSTOMERS TABLE
CREATE TABLE customers
(
    customer_id INT PRIMARY KEY IDENTITY(1,1),
    first_name VARCHAR(50),
    last_name VARCHAR(50)
);

-- ORDERS TABLE
CREATE TABLE orders
(
    order_id INT PRIMARY KEY IDENTITY(1,1),
    customer_id INT,
    order_date DATE,

    FOREIGN KEY (customer_id) REFERENCES customers(customer_id)
);

-- ORDER ITEMS TABLE
CREATE TABLE order_items
(
    item_id INT PRIMARY KEY IDENTITY(1,1),
    order_id INT,
    product_name VARCHAR(100),
    quantity INT,
    list_price DECIMAL(10,2),
    discount DECIMAL(4,2),

    FOREIGN KEY (order_id) REFERENCES orders(order_id)
);




-- INSERT CUSTOMERS
INSERT INTO customers (first_name, last_name)
VALUES
('John','Smith'),
('Emma','Brown'),
('David','Wilson'),
('Sophia','Taylor');

-- INSERT ORDERS
INSERT INTO orders (customer_id, order_date)
VALUES
(1,'2024-01-10'),
(1,'2024-02-15'),
(2,'2024-03-05'),
(3,'2024-04-20');

-- INSERT ORDER ITEMS
INSERT INTO order_items (order_id, product_name, quantity, list_price, discount)
VALUES
(1,'Car Battery',2,3000,0.10),
(1,'Car Oil',1,2000,0.05),
(2,'Car Tyre',4,2500,0.15),
(3,'Car Seat Cover',3,1500,0.05),
(4,'Car Stereo',1,4000,0.10);



-- Customers with Orders
SELECT 
    c.customer_id,
    c.first_name + ' ' + c.last_name AS full_name,

    (
        SELECT SUM(oi.quantity * oi.list_price * (1 - oi.discount))
        FROM orders o
        INNER JOIN order_items oi
        ON o.order_id = oi.order_id
        WHERE o.customer_id = c.customer_id
    ) AS total_order_value,

    CASE
        WHEN (
            SELECT SUM(oi.quantity * oi.list_price * (1 - oi.discount))
            FROM orders o
            INNER JOIN order_items oi
            ON o.order_id = oi.order_id
            WHERE o.customer_id = c.customer_id
        ) > 10000 THEN 'Premium'

        WHEN (
            SELECT SUM(oi.quantity * oi.list_price * (1 - oi.discount))
            FROM orders o
            INNER JOIN order_items oi
            ON o.order_id = oi.order_id
            WHERE o.customer_id = c.customer_id
        ) BETWEEN 5000 AND 10000 THEN 'Regular'

        ELSE 'Basic'
    END AS customer_category

FROM customers c
WHERE EXISTS (
    SELECT 1
    FROM orders o
    WHERE o.customer_id = c.customer_id
)

UNION

-- Customers without Orders
SELECT 
    c.customer_id,
    c.first_name + ' ' + c.last_name AS full_name,
    NULL AS total_order_value,
    'No Orders' AS customer_category
FROM customers c
WHERE NOT EXISTS (
    SELECT 1
    FROM orders o
    WHERE o.customer_id = c.customer_id
);


--problem3

