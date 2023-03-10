/****** Object:  StoredProcedure [dbo].[spDeleteDownloadedData]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Philip Stinson
-- Create date: 27 Sept 2010
-- Description:	SP to delete the old data from server
-- @Result = 1 True/Success
-- @Result = 0 False/ Failed
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteDownloadedData]
	
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	delete from ReceivedProductCategories
	delete from ReceivedItem
	delete from	ReceivedLayoutIem
END
GO
