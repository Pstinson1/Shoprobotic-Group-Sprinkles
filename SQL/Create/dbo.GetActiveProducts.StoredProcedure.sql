/****** Object:  StoredProcedure [dbo].[GetActiveProducts]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetActiveProducts]

AS
BEGIN

SELECT p.ProdId, p.ProdName, ISNULL(p.ProdSkuUpc, 'n/a') as 'ProdSkuUpc', 
ISNULL(p.ProdWeight, 0.00) as 'ProdWeight',ISNULL(p.ProdHeight, 0.00)as 'ProdHeight',
ISNULL(p.ProdWidth, 0.00) as 'ProdWidth', ISNULL(p.ProdDepth, 0.00) as 'ProdDepth',
CAST(ROUND(p.prodPrice,2) as Dec(10,2))ProdPrice, ISNULL(p.ProdCost, 0.00) as 'ProdCost', p.ProdDesc, p.ProdImgUrl1, 
p.IsActive, pc.ProdCatId, pc.ProdCatName FROM Product p INNER JOIN ProductCategories pc
ON p.ProdCatId = pc.ProdCatId
WHERE p.IsActive = 'True'

END
GO
