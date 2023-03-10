/****** Object:  StoredProcedure [dbo].[spAddTransactionID]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Philip Stinson
-- Create date: 23 Sept 2010
-- Description:	SP to Add the TransactionID from the paymentsystem to the Sales Record
-- @Result = 1 True/Success
-- @Result = 0 False/ Failed
-- =============================================
CREATE PROCEDURE [dbo].[spAddTransactionID]
	@SaleID int,
	@TransactionID varchar(50),
	@Result int =0 output
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @myERROR int -- Local @@ERROR
	DECLARE @myRowCount int -- Local @@ROWCOUNT
	
	BEGIN TRANSACTION UpdateSalesReporting

	UPDATE SalesReporting set TransactionId = @TransactionID,[Status] = 1  where ID=@SaleID

	SELECT @myERROR = @@ERROR,@myRowCount = @@ROWCOUNT
	
	
    IF @myERROR != 0 or @myRowCount =0 GOTO HANDLE_ERROR
	
	
	-- Set the result to success
	SET @Result = 1
	
	-- If we got to here then commit tran 
	COMMIT TRANSACTION UpdateSalesReporting
	RETURN


HANDLE_ERROR:
    ROLLBACK TRANSACTION UpdateSalesReporting
			-- Set the Reult to Error
		set @Result = 0
		 -- Raise an error and return
		RAISERROR ('Error in Update SalesReporting', 16, 1)
		RETURN -- Bail out of everything and do nothing else
		
END
GO
