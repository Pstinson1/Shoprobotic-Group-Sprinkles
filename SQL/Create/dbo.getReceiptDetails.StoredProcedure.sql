/****** Object:  StoredProcedure [dbo].[getReceiptDetails]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getReceiptDetails]

@ProdId int

AS
BEGIN

SELECT ProdName, preName, keyVend
FROM Product
Where ProdId = @ProdId

END
GO
