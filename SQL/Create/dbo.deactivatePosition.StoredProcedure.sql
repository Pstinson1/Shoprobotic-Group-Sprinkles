/****** Object:  StoredProcedure [dbo].[deactivatePosition]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[deactivatePosition]

@PosId int

AS
BEGIN

Update ProductPosition
SET IsActive = 'False'
WHERE PosId = @PosId

END
GO
