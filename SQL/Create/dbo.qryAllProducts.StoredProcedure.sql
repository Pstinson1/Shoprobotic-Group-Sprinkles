/****** Object:  StoredProcedure [dbo].[qryAllProducts]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[qryAllProducts]

AS
BEGIN

SELECT p.ProdId, p.ProdName As 'name', ISNULL(p.preName, '') as 'preName', ISNULL(p.keyVend, '') as 'keyVend', CAST(ROUND(prodPrice,2) as Dec(10,2))price,
ISNULL(CAST(ROUND(p.fullPrice,2) as Dec(10,2)), '-1')fullPrice, p.ProdDesc as 'desc', p.ProdImgUrl1 as 'url',
ISNULL(p.ProdimgUrl2, 'images/blank.jpg') as 'url2', ISNULL(p.ProdimgUrl3, 'images/blank.jpg') as 'url3',
ISNULL(p.ProdimgUrl4, 'images/blank.jpg') as 'url4', pc.ProdCatName as 'ProdCatId',
ISNULL(p.xPos, 0) as 'xPos', ISNULL(p.yPos, 0) as 'yPos'
FROM Product p
INNER JOIN ProductCategories pc on p.ProdCatId = pc.ProdCatId
WHERE p.IsActive = 'true' AND Exists (SELECT sa.prodID, Sum(sa.Adjustment) 
FROM StockAdjustments sa
WHERE sa.ProdID = p.ProdID
Group By sa.prodId
HAVING SUM(sa.Adjustment) > 0)
ORDER BY pc.ProdCatId

END
GO
