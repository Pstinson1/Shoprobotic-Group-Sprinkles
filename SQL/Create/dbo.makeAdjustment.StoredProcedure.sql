/****** Object:  StoredProcedure [dbo].[makeAdjustment]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[makeAdjustment]

@ProdId int,
@PosId int,
@Adjustment smallint,
@AdjustmentDate datetime

AS
BEGIN

INSERT StockAdjustments (ProdId, PosId, Adjustment, AdjustmentDate)
VALUES (@ProdId, @PosId, @Adjustment, @AdjustmentDate)

END
GO
