CREATE DATABASE SaleDB;
USE SaleDB;
CREATE TABLE Stores
(
    StoreID INT PRIMARY KEY,
    StoreName VARCHAR(100),
    Location VARCHAR(100)
);

INSERT INTO Stores VALUES
(1,'Reliance Store','Mumbai'),
(2,'Dmart','Delhi'),
(3,'More Store','Hyderabad');

CREATE TABLE Products
(
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100),
    Price DECIMAL(10,2)
);

INSERT INTO Products VALUES
(1,'Laptop',50000),
(2,'Mobile',20000),
(3,'Headphones',2000),
(4,'Keyboard',1500),
(5,'Mouse',800),
(6,'Monitor',12000);

CREATE TABLE Orders
(
    OrderID INT PRIMARY KEY,
    StoreID INT,
    OrderDate DATE,
    DiscountPercent DECIMAL(5,2) NULL,
    FOREIGN KEY (StoreID) REFERENCES Stores(StoreID)
);


INSERT INTO Orders VALUES
(101,1,'2026-03-01',10),
(102,2,'2026-03-02',5),
(103,1,'2026-03-03',NULL),
(104,3,'2026-03-04',8),
(105,2,'2026-03-05',12);

CREATE TABLE OrderDetails
(
    OrderDetailID INT PRIMARY KEY,
    OrderID INT,
    ProductID INT,
    Quantity INT,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

INSERT INTO OrderDetails VALUES
(1,101,1,2),
(2,101,3,5),
(3,102,2,1),
(4,103,4,3),
(5,104,5,10),
(6,105,6,2),
(7,105,3,4);

----Create a stored procedure to generate total sales amount per store.

CREATE PROCEDURE sp_TotalSalesPerStore
AS
BEGIN

SELECT 
    s.StoreID,
    s.StoreName,
    SUM(p.Price * od.Quantity) AS TotalSales
FROM Stores s
JOIN Orders o
ON s.StoreID = o.StoreID
JOIN OrderDetails od
ON o.OrderID = od.OrderID
JOIN Products p
ON od.ProductID = p.ProductID
GROUP BY s.StoreID, s.StoreName;

END;

EXEC sp_TotalSalesPerStore;

 ----Create a stored procedure to retrieve orders by date range.

 CREATE PROCEDURE sp_GetOrdersByDateRange
(
    @StartDate DATE,
    @EndDate DATE
)
AS
BEGIN

SELECT 
    OrderID,
    StoreID,
    OrderDate,
    ISNULL(DiscountPercent,0) AS DiscountPercent
FROM Orders
WHERE OrderDate BETWEEN @StartDate AND @EndDate;

END;

EXEC sp_GetOrdersByDateRange 
'2026-03-01','2026-03-05';


---- Create a scalar function to calculate total price after discount.

CREATE FUNCTION fn_CalculateDiscountPrice
(
    @Price DECIMAL(10,2),
    @DiscountPercent DECIMAL(5,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN

DECLARE @FinalPrice DECIMAL(10,2);

SET @DiscountPercent = ISNULL(@DiscountPercent,0);

SET @FinalPrice = @Price - (@Price * @DiscountPercent / 100);

RETURN @FinalPrice;

END;

SELECT dbo.fn_CalculateDiscountPrice(1000,10) AS DiscountPrice;

---- Create a table-valued function to return top 5 selling products.

CREATE FUNCTION fn_Top5SellingProducts()
RETURNS TABLE
AS
RETURN
(
    SELECT TOP 5
        p.ProductID,
        p.ProductName,
        SUM(od.Quantity) AS TotalSold
    FROM Products p
    JOIN OrderDetails od
    ON p.ProductID = od.ProductID
    GROUP BY p.ProductID, p.ProductName
    ORDER BY TotalSold DESC
);

SELECT * 
FROM fn_Top5SellingProducts();
