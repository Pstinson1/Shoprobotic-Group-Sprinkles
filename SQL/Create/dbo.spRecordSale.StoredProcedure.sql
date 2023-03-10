/****** Object:  StoredProcedure [dbo].[spRecordSale]    Script Date: 10/20/2011 09:30:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Philip Stinson
-- Create date: 23 Sept 2010
-- Description:	SP to Record the Sale of an item.
-- @Result = 1 True/Success
-- @Result = 0 False/ Failed
-- =============================================
CREATE PROCEDURE [dbo].[spRecordSale]
	@PosID int,
	@SaleID int output,
	@Result int =0 output
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	
	DECLARE @ProdID int
	DECLARE @ContainerNumber varchar(50)
	DEClARE @NewQty int
	DECLARE @myERROR int -- Local @@ERROR
	
	BEGIN TRANSACTION RecordSale

	--Decrement the Stock
	UPDATE    ProductPosition
	SET              ItemsInStock = ItemsInStock-1, ModDate = GETDATE()
	WHERE     (PosId = @PosId)
	
	SELECT  @NewQty = ItemsInStock ,@ContainerNumber = ContainerNumber ,@ProdID = ProdID 
	FROM ProductPosition 
	WHERE     (PosId = @PosId)
	
		
	-- Adjust the Stock by -1 to record the sale.
	INSERT INTO StockAdjustments
                      (ProdID, PosID, Adjustment, AdjustmentDate,Transmitted)
	VALUES     (@ProdID, @PosId, -1, GETDATE(),0)
	
	SELECT @myERROR = @@ERROR
    IF @myERROR != 0 GOTO HANDLE_ERROR
    
    
	-- Add the Sale to the reporting table and return 
	INSERT INTO SalesReporting
                      ([TransactionID],[Date], ProductId, containernumber, ProductName, Barcode, Price, StockLevel, [Status])
	(SELECT 
			-1 as [TransactionID],
			Getdate(),
			ProdId,
			@ContainerNumber as containernumber,
			ProdName,
			Barcode,
			cast(prodprice * 100 as int),
			@NewQty as stocklevel,
			0 as [status] from Product
	 WHERE ProdId = @ProdID
	 )
	SELECT @myERROR = @@ERROR,@SaleID = SCOPE_IDENTITY()
    
    IF @myERROR != 0 GOTO HANDLE_ERROR
	
		
	IF @@ERROR <> 0
		BEGIN
			-- Rollback the transaction
		ROLLBACK TRANSACTION RecordSale
			-- Set the Reult to Error
		set @Result = 0
		 -- Raise an error and return
		RAISERROR ('Error in Recording Sale', 16, 1)
		RETURN -- Bail out of everything and do nothing else
	END
	

	
	
	-- Set the result to success
	SET @Result = 1
	
	-- If we got to here then commit tran 
	COMMIT TRANSACTION RecordSale
	RETURN


HANDLE_ERROR:
    ROLLBACK TRANSACTION RecordSale
			-- Set the Reult to Error
		set @Result = 0
		 -- Raise an error and return
		RAISERROR ('Error in Recording Sale', 16, 1)
		RETURN -- Bail out of everything and do nothing else
		
END
GO
