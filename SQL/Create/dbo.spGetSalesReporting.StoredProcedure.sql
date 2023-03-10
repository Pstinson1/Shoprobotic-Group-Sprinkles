/****** Object:  StoredProcedure [dbo].[spGetSalesReporting]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Philip Stinson
-- Create date: 24 Sep 2010
-- Description:	Gets the Sales for Uploading
-- =============================================
Create PROCEDURE [dbo].[spGetSalesReporting] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT     SalesReporting.Id, 
				   SalesReporting.TransactionId, 
				   SalesReporting.Date, 
				   SalesReporting.ProductName,
				   SalesReporting.Price, 
				   SalesReporting.StockLevel, 
				   SalesReporting.ContainerNumber, 
				   Product.ItemID
FROM         SalesReporting INNER JOIN
                      Product ON SalesReporting.ProductId = Product.ProdId
WHERE    [Status] = 1


END
GO
