--problem3
CREATE DATABASE ProductsStoreDb;
USE ProductsStoreDb;




-- STORES TABLE
CREATE TABLE stores
(
    storeId INT PRIMARY KEY IDENTITY(1,1),
    storeName VARCHAR(100)
);

-- PRODUCTS TABLE
CREATE TABLE products
(
    productId INT PRIMARY KEY IDENTITY(1,1),
    productName VARCHAR(100)
);

-- ORDERS TABLE
CREATE TABLE orders
(
    orderId INT PRIMARY KEY IDENTITY(1,1),
    storeId INT,
    orderDate DATE,
    FOREIGN KEY (storeId) REFERENCES stores(storeId)
);

-- ORDER ITEMS TABLE
CREATE TABLE order_items
(
    itemId INT PRIMARY KEY IDENTITY(1,1),
    orderId INT,
    productId INT,
    quantity INT,
    list_price DECIMAL(10,2),
    discount DECIMAL(10,2),

    FOREIGN KEY (orderId) REFERENCES orders(orderId),
    FOREIGN KEY (productId) REFERENCES products(productId)
);

-- STOCKS TABLE
CREATE TABLE stocks
(
    storeId INT,
    productId INT,
    quantity INT,

    PRIMARY KEY (storeId, productId),

    FOREIGN KEY (storeId) REFERENCES stores(storeId),
    FOREIGN KEY (productId) REFERENCES products(productId)
);




INSERT INTO stores (storeName)
VALUES
('Mumbai Store'),
('Pune Store');

INSERT INTO products (productName)
VALUES
('Car Battery'),
('Car Tyre'),
('Car Stereo'),
('Car Oil');

INSERT INTO orders (storeId, orderDate)
VALUES
(1,'2024-01-10'),
(1,'2024-02-10'),
(2,'2024-03-12');

INSERT INTO order_items (orderId, productId, quantity, list_price, discount)
VALUES
(1,1,2,3000,100),
(1,2,1,2500,50),
(2,3,3,4000,200),
(3,1,1,3000,100);

INSERT INTO stocks (storeId, productId, quantity)
VALUES
(1,1,0),
(1,2,10),
(1,3,0),
(2,1,5),
(2,4,20);




-- 1️⃣ IDENTIFY PRODUCTS SOLD IN EACH STORE
-- (Nested Query in FROM)


SELECT 
    s.storeName,
    p.productName,
    sales.total_quantity_sold,
    sales.total_revenue
FROM
(
    SELECT 
        o.storeId,
        oi.productId,
        SUM(oi.quantity) AS total_quantity_sold,
        SUM((oi.quantity * oi.list_price) - oi.discount) AS total_revenue
    FROM orders o
    JOIN order_items oi
        ON o.orderId = oi.orderId
    GROUP BY o.storeId, oi.productId
) AS sales
JOIN stores s
    ON sales.storeId = s.storeId
JOIN products p
    ON sales.productId = p.productId;




-- 2️⃣ COMPARE SOLD PRODUCTS AND ZERO STOCK
-- USING INTERSECT


SELECT productId
FROM order_items

INTERSECT

SELECT productId
FROM stocks
WHERE quantity = 0;



-- 3️⃣ PRODUCTS SOLD BUT NOT ZERO STOCK
-- USING EXCEPT


SELECT productId
FROM order_items

EXCEPT

SELECT productId
FROM stocks
WHERE quantity = 0;




-- 4️⃣ DISPLAY STORE, PRODUCT AND
-- TOTAL QUANTITY SOLD


SELECT 
    s.storeName,
    p.productName,
    SUM(oi.quantity) AS total_quantity_sold
FROM stores s
JOIN orders o
    ON s.storeId = o.storeId
JOIN order_items oi
    ON o.orderId = oi.orderId
JOIN products p
    ON oi.productId = p.productId
WHERE oi.productId IN
(
    SELECT productId
    FROM order_items
    INTERSECT
    SELECT productId
    FROM stocks
    WHERE quantity = 0
)
GROUP BY s.storeName, p.productName;




-- 5️⃣ CALCULATE TOTAL REVENUE


SELECT 
    s.storeName,
    p.productName,
    SUM(oi.quantity) AS total_quantity_sold,
    SUM((oi.quantity * oi.list_price) - oi.discount) AS total_revenue
FROM stores s
JOIN orders o
    ON s.storeId = o.storeId
JOIN order_items oi
    ON o.orderId = oi.orderId
JOIN products p
    ON oi.productId = p.productId
GROUP BY s.storeName, p.productName;


-- 6️⃣ UPDATE STOCK FOR DISCONTINUED PRODUCTS


UPDATE stocks
SET quantity = 0
WHERE productId IN
(
SELECT productId
FROM order_items

EXCEPT

SELECT productId
FROM stocks
WHERE quantity = 0
);


--problem4

CREATE DATABASE Problem4Db;


USE Problem4Db;




-- CUSTOMERS TABLE
CREATE TABLE customers
(
    customerId INT PRIMARY KEY IDENTITY(1,1),
    firstName VARCHAR(50),
    lastName VARCHAR(50)
);

-- ORDERS TABLE
CREATE TABLE orders
(
    orderId INT PRIMARY KEY IDENTITY(1,1),
    customerId INT,
    orderDate DATE,
    required_date DATE,
    shipped_date DATE,
    order_status INT,

    FOREIGN KEY (customerId) REFERENCES customers(customerId)
);

-- ARCHIVED ORDERS TABLE
CREATE TABLE archived_orders
(
    orderId INT,
    customerId INT,
    orderDate DATE,
    required_date DATE,
    shipped_date DATE,
    order_status INT
);



INSERT INTO customers(firstName,lastName)
VALUES
('John','Smith'),
('Emma','Brown'),
('David','Wilson'),
('Sophia','Taylor');

INSERT INTO orders(customerId,orderDate,required_date,shipped_date,order_status)
VALUES
(1,'2023-01-10','2023-01-15','2023-01-14',4),
(1,'2022-02-12','2022-02-18','2022-02-20',3),
(2,'2024-03-01','2024-03-06','2024-03-05',4),
(3,'2023-06-10','2023-06-15','2023-06-18',3),
(4,'2024-04-05','2024-04-10','2024-04-09',4);



-- 1. INSERT ARCHIVED RECORDS
-- USING INSERT INTO SELECT


INSERT INTO archived_orders
SELECT *
FROM orders
WHERE order_status = 3
AND orderDate < DATEADD(YEAR,-1,GETDATE());


-- 2. DELETE REJECTED ORDERS
-- OLDER THAN 1 YEAR


DELETE FROM orders
WHERE orderId IN
(
    SELECT orderId
    FROM archived_orders
);



-- 3. CUSTOMERS WHOSE ALL ORDERS ARE COMPLETED
-- USING NESTED QUERY


SELECT customerId
FROM orders
GROUP BY customerId
HAVING COUNT(*) =
(
    SELECT COUNT(*)
    FROM orders o2
    WHERE o2.customerId = orders.customerId
    AND o2.order_status = 4
);



-- 4. DISPLAY ORDER PROCESSING DELAY
-- USING DATEDIFF
SELECT
    orderId,
    orderDate,
    shipped_date,
    DATEDIFF(day, orderDate, shipped_date) AS processing_delay_days
FROM orders;



-- MARK ORDERS AS DELAYED OR ON TIME
-- USING CASE


SELECT
    orderId,
    orderDate,
    required_date,
    shipped_date,

    CASE
        WHEN shipped_date > required_date
        THEN 'Delayed'
        ELSE 'On Time'
    END AS delivery_status

FROM orders;
