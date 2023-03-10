/****** Object:  StoredProcedure [dbo].[chkPositionIsOrphan]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[chkPositionIsOrphan]

@PosId int

AS
BEGIN

IF EXISTS (SELECT sa.posid FROM StockAdjustments sa
WHERE sa.PosId = @PosId)
RETURN -1
ELSE IF EXISTS(Select oi.PosId FROM OrderItems oi
WHERE oi.PosId = @PosId)
Return -1
ELSE 
RETURN 1

END
GO
