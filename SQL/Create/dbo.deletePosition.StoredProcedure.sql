/****** Object:  StoredProcedure [dbo].[deletePosition]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[deletePosition]

@PosId int

AS
BEGIN

Delete
FROM ProductPosition
WHERE PosId = @PosId

END
GO
