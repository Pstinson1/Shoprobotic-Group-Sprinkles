/****** Object:  StoredProcedure [dbo].[spCheckUserCredentials]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jonathan Campbell Jones
-- Create date:  28 Sept 2010
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE [dbo].[spCheckUserCredentials] 
	@UserId int,
	@Passcode varchar(50),
	@Result int =0 output
AS
BEGIN

	DECLARE @myERROR int -- Local @@ERROR
	DECLARE @myRowCount int -- Local @@ROWCOUNT

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET @Result = 0
	
	SELECT @Result  = Users.AccessType  
	FROM     Users    where Users.Id = @UserId and Users.Passcode= @Passcode
	
	
	INSERT INTO UserAccess  (DateChecked, UserId, Passcode, Result)
	VALUES (getdate(), @UserId, @Passcode, @Result)
	
	SELECT @myERROR = @@ERROR,@myRowCount = @@ROWCOUNT
	IF @myERROR != 0 or @myRowCount =0 GOTO HANDLE_ERROR
	
		
		
		
	RETURN
     	
HANDLE_ERROR:
		--	Set the Reult to Error
	set @Result = 0
		                
END
GO
