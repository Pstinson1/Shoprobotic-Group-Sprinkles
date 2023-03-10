/****** Object:  StoredProcedure [dbo].[spUpdateStock]    Script Date: 10/20/2011 09:30:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Philip Stinson
-- Create date: 23 Sept 2010
-- Description:	SP to update the stock level in the machine to the correct level as required by the user
-- @Result = 1 True/Success
-- @Result = 0 False/ Failed
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateStock] 
	@PosID int,
	@Qty int,
	@Result int =0 output
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @ProdID int
	DEClARE @AdjustmentAmount int
	deCLARE @myERROR int -- Local @@ERROR
	DECLARE @myRowCount int -- Local @@ROWCOUNT
	
	BEGIN TRANSACTION UpdateStock

	UPDATE    ProductPosition
	SET              ItemsInStock = @Qty, ModDate = GETDATE()
	WHERE     (PosId = @PosId)
	
	SELECT @myERROR = @@ERROR,@myRowCount = @@ROWCOUNT
	IF @myERROR != 0 or @myRowCount =0 GOTO HANDLE_ERROR
	
	-- Get the Product
	select @ProdID = ProdID from ProductPosition where (PosId = @PosId)
	
	SELECT @myERROR = @@ERROR,@myRowCount = @@ROWCOUNT
	IF @myERROR != 0 or @myRowCount =0 GOTO HANDLE_ERROR
	
	-- Get how much we need to adjust the StockAdjustments table to get it back in line.
	
	SELECT @AdjustmentAmount = @Qty -  SUM(adjustment)
	FROM StockAdjustments 
	WHERE PosID = @PosID 
	GROUP BY ProdID
	
	-- If there are no rows in Stock Adjustments, create a new row.
	IF @AdjustmentAmount is null
	Begin
	set @AdjustmentAmount =@Qty
	End

	INSERT INTO StockAdjustments
                      (ProdID, PosID, Adjustment, AdjustmentDate,Transmitted)
	VALUES     (@ProdID, @PosId, @AdjustmentAmount, GETDATE(),0)
	SELECT @myERROR = @@ERROR,@myRowCount = @@ROWCOUNT
	IF @myERROR != 0 GOTO HANDLE_ERROR
	
	-- Set the result to success
	SET @Result = 1
	
	-- If we got to here then commit tran 
	COMMIT TRANSACTION UpdateStock
	RETURN
	
HANDLE_ERROR:
    ROLLBACK TRANSACTION UpdateStock
			-- Set the Reult to Error
		set @Result = 0
		 -- Raise an error and return
		RAISERROR ('Error in Update SalesReporting', 16, 1)
		RETURN -- Bail out of everything and do nothing else
END
GO
