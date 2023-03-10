/****** Object:  StoredProcedure [dbo].[spReceiveLayoutItem]    Script Date: 10/20/2011 09:30:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Philip Stinson
-- Create date: 27 Sept 2010
-- Description:	SP to Add to the table the downloded LayoutItems
-- @Result = 1 True/Success
-- @Result = 0 False/ Failed
-- =============================================
Create PROCEDURE [dbo].[spReceiveLayoutItem]
	@LayoutItemID int,
	@ContainerNumber varchar(50),
	@ItemID int,
	@LayoutPrice money,
	@xPos int,
	@yPos int,
	@Active bit,
	@Result int =0 output
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	BEGIN TRANSACTION ReceiveLayoutItem 

	INSERT INTO ReceivedLayoutIem
                      (LayoutItemID, ContainerNumber, ItemID, 
                      LayoutPrice, xPos, yPos, Active)
    Values ( @LayoutItemID, @ContainerNumber, @ItemID, 
			@LayoutPrice, @xPos, @yPos, @Active)

    IF @@ERROR != 0 GOTO HANDLE_ERROR
	

	-- Set the result to success
	SET @Result = 1
	
	-- If we got to here then commit tran 
	COMMIT TRANSACTION ReceiveLayoutItem
	RETURN


HANDLE_ERROR:
    ROLLBACK TRANSACTION ReceiveLayoutItem
			-- Set the Reult to Error
		set @Result = 0
		 -- Raise an error and return
		RAISERROR ('Error in ReceiveLayoutItem', 16, 1)
		RETURN -- Bail out of everything and do nothing else
		
END
GO
