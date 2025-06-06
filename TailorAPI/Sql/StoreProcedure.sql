CREATE PROCEDURE sp_GetTotalCounts123

AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        (SELECT COUNT(*) FROM Customers) AS TotalCustomers,
        (SELECT COUNT(*) FROM Orders) AS TotalOrders,
        (SELECT COUNT(*) FROM Orders WHERE OrderStatus = 'Completed') AS CompletedOrders,
        (SELECT COUNT(*) FROM Orders WHERE OrderStatus = 'Pending') AS PendingOrders,
        (SELECT COUNT(*) FROM Users) AS TotalEmployees,
        (SELECT COUNT(*) FROM Products) AS TotalProducts,
        (SELECT COUNT(*) FROM FabricTypes) AS TotalFabrics -- <-- Add this line
END
