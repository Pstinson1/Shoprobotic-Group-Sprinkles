/****** Object:  StoredProcedure [dbo].[activateProduct]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[activateProduct]

@ProdId int

AS
BEGIN

UPDATE Product Set IsActive = 'True'
WHERE ProdId = @ProdID

END
GO
