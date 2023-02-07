CREATE PROCEDURE dbo.Products_Insert
    @ProductId INT,
    @ProductName NVARCHAR,
    @Supplier NVARCHAR,
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
