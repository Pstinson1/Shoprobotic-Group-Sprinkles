/****** Object:  StoredProcedure [dbo].[insertPosition]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[insertPosition]

@ProdId int,
@xPos int,
@yPos int,
@vdo int,
@RestockLevel int,
@CreateDate smalldatetime,
@IsActive bit,
@LastVended bit

AS
BEGIN

INSERT ProductPosition (ProdId, xPos, yPos, vdo, RestockLevel, CreateDate, IsActive, LastVended)

VALUES (@ProdId, @xPos, @yPos, @vdo, @RestockLevel, @CreateDate, @IsActive, @LastVended)

END
GO
