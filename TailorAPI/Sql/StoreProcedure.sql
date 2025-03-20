USE TailorAPIDB
GO

CREATE PROCEDURE sp_GetTotalCounts
AS
BEGIN
    SELECT 
        (SELECT COUNT(*) FROM Customers) AS TotalCustomers,
        (SELECT COUNT(*) FROM Orders) AS TotalOrders

END
