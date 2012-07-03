/****** Object:  StoredProcedure [dbo].[getPositions]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getPositions]

@ProdId int

AS
BEGIN

SELECT PosId, ProdId, xPos, yPos, vdo, RestockLevel, IsActive, LastVended
FROM ProductPosition
WHERE ProdId = @ProdId AND IsActive= 'True'
ORDER BY PosId Asc

END
GO
