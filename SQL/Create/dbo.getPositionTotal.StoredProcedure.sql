/****** Object:  StoredProcedure [dbo].[getPositionTotal]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getPositionTotal]

@PosID int,
@Result int OUT

AS
BEGIN

SET @Result = 
(SELECT ISNULL(SUM(Adjustment), 0) As [Position Total]
FROM StockAdjustments
WHERE PosId = @PosID)

END
GO
