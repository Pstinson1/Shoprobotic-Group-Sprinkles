/****** Object:  StoredProcedure [dbo].[getProductCountTotal]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getProductCountTotal]

@ProdId int,
@Result int OUT

AS
BEGIN

SET @Result = 
(SELECT ISNULL(SUM(Adjustment), 0)
FROM StockAdjustments
WHERE ProdID = @ProdID)

END
GO
