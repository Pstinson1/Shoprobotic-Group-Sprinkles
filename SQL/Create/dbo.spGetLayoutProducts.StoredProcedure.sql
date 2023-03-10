/****** Object:  StoredProcedure [dbo].[spGetLayoutProducts]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Philip Stinson
-- Create date: 24 Sept 2010
-- Description:	Get the Products for the QtyApp
-- =============================================
create PROCEDURE [dbo].[spGetLayoutProducts] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

  SELECT     Product.ProdName, 
			Product.PreName, 
			Product.KeyVend, 
			Product.ProdPrice, 
			ProductPosition.PosId, 
            ProductPosition.xVisualPosition, 
            ProductPosition.yVisualPosition,
            productPosition.ItemsInStock
FROM         Product INNER JOIN
                      ProductPosition ON Product.ProdId = ProductPosition.ProdId
                      order by xVisualPosition,yVisualPosition
END
GO
