/****** Object:  StoredProcedure [dbo].[spSetNewData]    Script Date: 10/20/2011 09:30:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Philip Stinson
-- Create date: 27 Sept 2010
-- Description:	Set the newDataFlag
-- @Result = 1 True/Success
-- @Result = 0 False/ Failed
-- =============================================
CREATE PROCEDURE [dbo].[spSetNewData]
	
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Update tekstatsservice set NewData = 1,lastdownload = GETDATE()
	if @@ROWCOUNT =0
	begin
	insert into tekstatsservice (NewData,LastDownload) values ( 1,GETDATE())
	end
	
END
GO
