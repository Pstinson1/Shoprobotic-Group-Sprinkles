/****** Object:  StoredProcedure [dbo].[updatePosition]    Script Date: 10/20/2011 09:30:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[updatePosition]

@PosId int,
@xPos int,
@yPos int,
@vdo int

AS
BEGIN

UPDATE ProductPosition
SET xPos = @xPos, yPos = @yPos, vdo = @vdo
WHERE PosId = @PosId

END
GO
