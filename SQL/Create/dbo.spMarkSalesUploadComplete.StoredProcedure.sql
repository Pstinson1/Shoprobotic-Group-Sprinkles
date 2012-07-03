/****** Object:  StoredProcedure [dbo].[spMarkSalesUploadComplete]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Philip Stinson
-- Create date: 24 Sep 2010
-- Description:	Sets that all the sales have been uploaded
-- This needs to be improved so that if a sale happens when the upload
-- is taking place, the sale does not get missed
-- =============================================
Create PROCEDURE [dbo].[spMarkSalesUploadComplete]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	update SalesReporting set 
	Status = 3 where Status = 1



END
GO
