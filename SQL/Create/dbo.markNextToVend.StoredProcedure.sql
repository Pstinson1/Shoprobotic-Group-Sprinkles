/****** Object:  StoredProcedure [dbo].[markNextToVend]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[markNextToVend]

@ProdId int,
@PosId int

AS

SET NOCOUNT ON

BEGIN TRAN

Update ProductPosition
SET LastVended = 'False'
WHERE ProdId = @ProdId

Update ProductPosition
SET LastVended = 'True'
Where PosId = @PosId

IF @@ERROR <> 0 
ROLLBACK TRAN
ELSE
COMMIT TRAN
GO
