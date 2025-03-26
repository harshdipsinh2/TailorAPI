
-- Create the stored procedure
CREATE PROCEDURE [dbo].[sp_GetTotalCounts]
AS
BEGIN
    -- Error handling (recommended for stability)
    SET NOCOUNT ON;

    -- Data retrieval logic
    SELECT 
        (SELECT COUNT(*) FROM Customers) AS TotalCustomers,
        (SELECT COUNT(*) FROM Orders) AS TotalOrders,
        (SELECT COUNT(*) FROM Users) AS TotalUsers,
        (SELECT COUNT(*) FROM Products) AS TotalProducts,
        (SELECT COUNT(*) FROM Fabrics) AS TotalFabrics;
END
GO
