/****** Object:  StoredProcedure [dbo].[spGetInventory]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spGetInventory] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT     ProductPosition.ContainerNumber, ProductPosition.ItemsInStock, Product.ProdName, Product.PreName, ProductCategories.ProdCatDesc, Product.KeyVend, 
                      Product.ItemID
FROM         ProductPosition INNER JOIN
                      Product ON ProductPosition.ProdId = Product.ProdId INNER JOIN
                      ProductCategories ON Product.ProdCatId = ProductCategories.ProdCatId
WHERE     (ProductPosition.IsActive = 1)
ORDER BY ProductPosition.ContainerNumber

END
GO
