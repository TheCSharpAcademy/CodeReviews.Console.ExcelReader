USE master
GO
DROP DATABASE IF EXISTS ExcelDb
GO
CREATE DATABASE ExcelDb
GO
USE ExcelDb
GO
BEGIN
  CREATE TABLE Products
  (
    ProductId INT,
    ProductName NVARCHAR(100),
    Supplier NVARCHAR
(100),
    ProductCost FLOAT
  );
END
GO
CREATE PROCEDURE dbo.Products_Insert
  @ProductId INT,
  @ProductName NVARCHAR(100),
  @Supplier NVARCHAR(100),
  @ProductCost FLOAT
AS
BEGIN
  SET NOCOUNT ON
  INSERT INTO Products
    (
    ProductId,
    ProductName,
    Supplier,
    ProductCost
    )
  VALUES
    (
      @ProductId, @ProductName,
      @Supplier, @ProductCost
    )
END
GO
CREATE PROCEDURE dbo.Products_Select
AS
BEGIN
  SELECT *
  FROM Products
END
GO
