/****** Object:  StoredProcedure [dbo].[spLookForNewLayout]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jonathan Campbell Jones
-- Create date: 28 Sept 2010
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spLookForNewLayout]
	@Result int =0 output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @Result = NewData from  TekStatsService
END
GO
