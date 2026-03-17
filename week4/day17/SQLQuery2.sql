
CREATE DATABASE productDB
USE productDB;

CREATE TABLE products
(
product_id INT PRIMARY KEY,
product_name VARCHAR(100),
stock_quantity INT
);

INSERT INTO products VALUES
(1,'Laptop',50),
(2,'Mobile',100),
(3,'Headphones',30);

CREATE TABLE orders
(
order_id INT PRIMARY KEY,
order_date DATETIME DEFAULT GETDATE(),
order_status INT DEFAULT 1
);

CREATE TABLE order_items
(
item_id INT IDENTITY PRIMARY KEY,
order_id INT,
product_id INT,
quantity INT,

FOREIGN KEY(order_id) REFERENCES orders(order_id),
FOREIGN KEY(product_id) REFERENCES products(product_id)
);

CREATE TRIGGER trg_reduce_stock
ON order_items
AFTER INSERT
AS
BEGIN

    -- Check stock availability
    IF EXISTS (
        SELECT 1
        FROM products p
        JOIN inserted i
        ON p.product_id = i.product_id
        WHERE p.stock_quantity < i.quantity
    )
    BEGIN
        RAISERROR('Insufficient Stock',16,1)
        ROLLBACK TRANSACTION
        RETURN
    END

    -- Reduce stock quantity
    UPDATE p
    SET p.stock_quantity = p.stock_quantity - i.quantity
    FROM products p
    JOIN inserted i
    ON p.product_id = i.product_id

END




-- Write a transaction to insert data into orders and order_items tables.

BEGIN TRY

BEGIN TRANSACTION

-- Insert order
INSERT INTO orders(order_id, order_status)
VALUES(102, 1)


-- Insert order items
INSERT INTO order_items(order_id,product_id,quantity)
VALUES
(102,3,14)

COMMIT

PRINT 'Order placed successfully'

END TRY


BEGIN CATCH

ROLLBACK

PRINT 'Order failed: ' + ERROR_MESSAGE()

END CATCH

SELECT * FROM products;
SELECT * FROM orders;
SELECT * FROM order_items;

SELECT o.order_id, o.order_status, oi.product_id, oi.quantity
FROM orders o
JOIN order_items oi
ON o.order_id = oi.order_id;

-- Atomic Order Cancellation with SAVEPOINT

BEGIN TRY

BEGIN TRANSACTION

-- Savepoint before stock restore
SAVE TRANSACTION BeforeStockRestore


-- Restore stock from order_items
UPDATE p
SET p.stock_quantity = p.stock_quantity + oi.quantity
FROM products p
JOIN order_items oi
ON p.product_id = oi.product_id
WHERE oi.order_id = 101


-- Update order status to Rejected
UPDATE orders
SET order_status = 3
WHERE order_id = 101


COMMIT TRANSACTION

PRINT 'Order cancelled successfully'

END TRY


BEGIN CATCH

PRINT 'Error occurred: ' + ERROR_MESSAGE()

-- rollback to savepoint
IF @@TRANCOUNT > 0
ROLLBACK TRANSACTION BeforeStockRestore

-- rollback full transaction
IF @@TRANCOUNT > 0
ROLLBACK TRANSACTION

END CATCH