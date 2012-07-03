/****** Object:  StoredProcedure [dbo].[updateRestockLevel]    Script Date: 10/20/2011 09:30:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[updateRestockLevel]

@PosId int,
@RestockLevel int,
@ModDate smalldatetime

AS
BEGIN

Update ProductPosition
SET RestockLevel = @RestockLevel, ModDate = @ModDate
WHERE PosId = @PosId

END
GO
