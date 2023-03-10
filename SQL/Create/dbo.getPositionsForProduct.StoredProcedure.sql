/****** Object:  StoredProcedure [dbo].[getPositionsForProduct]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getPositionsForProduct]

@ProdId int

AS
BEGIN

SELECT pp.PosId, pp.ProdId, pp.xPos, pp.yPos, pp.vdo, pp.LastVended, pp.pickattempts, pp.fridgeid
FROM ProductPosition pp
WHERE pp.ProdId = @ProdId AND pp.IsActive= 'True' AND Exists (SELECT sa.prodID, Sum(sa.Adjustment) 
FROM StockAdjustments sa
WHERE sa.ProdID = pp.ProdID
Group By sa.prodId
HAVING SUM(sa.Adjustment) > 0)
ORDER BY pp.PosId Asc

END
GO
