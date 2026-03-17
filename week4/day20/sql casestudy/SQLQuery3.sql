
CREATE DATABASE BookStoreDb;

USE BookStoreDb;

CREATE TABLE Books (
    BookID  INT IDENTITY(1,1) PRIMARY KEY,
    Title   NVARCHAR(150) NOT NULL,
    Stock   INT NOT NULL CHECK (Stock >= 0),
    Price   DECIMAL(10,2) NOT NULL
);

CREATE TABLE Orders (
    OrderID    INT IDENTITY(1,1) PRIMARY KEY,
    BookID     INT NOT NULL,
    Quantity   INT NOT NULL CHECK (Quantity > 0),
    OrderDate  DATETIME2 DEFAULT SYSDATETIME(),
    FOREIGN KEY (BookID) REFERENCES Books(BookID)
);

-- procedure to insert books

CREATE PROCEDURE sp_AddNewBook
@Title NVARCHAR(150),
@Stock INT,
@Price DECIMAL(10,2)
AS
BEGIN
    BEGIN TRY

        INSERT INTO Books(Title, Stock, Price)
        VALUES(@Title, @Stock, @Price);

        PRINT 'Book added successfully.';

    END TRY

    BEGIN CATCH

        PRINT 'Error ' + 
        CAST(ERROR_NUMBER() AS VARCHAR) + 
        ': ' + ERROR_MESSAGE();

    END CATCH
END;

-- insert books manaully

EXEC sp_AddNewBook 'SQL Server Basics', 10, 500;
EXEC sp_AddNewBook 'Advanced SQL Queries', 5, 750;
EXEC sp_AddNewBook 'Frontend Development', 8, 600;
EXEC sp_AddNewBook '.NET Guide', 3, 900;
EXEC sp_AddNewBook 'C# for .NET', 6, 800;

SELECT * FROM Books;

-- procedure to place order

CREATE PROCEDURE sp_PlaceOrder
@BookID INT,
@Quantity INT
AS
BEGIN

SET XACT_ABORT ON;

BEGIN TRY

    BEGIN TRANSACTION;

    -- case 1: book not found
    IF NOT EXISTS
    (
        SELECT 1 
        FROM Books 
        WHERE BookID = @BookID
    )
    BEGIN
        RAISERROR('Book not found.',16,1);
    END

    -- case 2: Not enough stock
    IF EXISTS
    (
        SELECT 1
        FROM Books
        WHERE BookID = @BookID
        AND Stock < @Quantity
    )
    BEGIN
        RAISERROR('Not enough stock available.',16,1);
    END

    -- Reduce stock
    UPDATE Books
    SET Stock = Stock - @Quantity
    WHERE BookID = @BookID;

    -- Insert order
    INSERT INTO Orders(BookID, Quantity)
    VALUES(@BookID, @Quantity);

    COMMIT TRANSACTION;

    PRINT 'Order placed successfully.';

END TRY

BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    PRINT 'Error ' +
    CAST(ERROR_NUMBER() AS VARCHAR) +
    ': ' + ERROR_MESSAGE();

END CATCH

END;
-- test 1

EXEC sp_PlaceOrder 1,2;

-- test 2

EXEC sp_PlaceOrder 2,20;

-- test 3

EXEC sp_PlaceOrder 8,2;

SELECT * FROM Books;
SELECT * FROM Orders;