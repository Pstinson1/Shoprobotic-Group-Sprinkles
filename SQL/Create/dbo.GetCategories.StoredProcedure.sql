/****** Object:  StoredProcedure [dbo].[GetCategories]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCategories]

AS
BEGIN

SELECT ProdCatId, ProdCatName
FROM ProductCategories

END
GO
