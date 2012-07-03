/****** Object:  StoredProcedure [dbo].[updateProdPrice]    Script Date: 10/20/2011 09:30:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[updateProdPrice]

@ProdId int,
@ProdPrice money

AS
BEGIN

UPDATE Product
SET ProdPrice = @ProdPrice
WHERE ProdId = @ProdId

END
GO
