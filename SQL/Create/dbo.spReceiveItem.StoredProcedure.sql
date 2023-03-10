/****** Object:  StoredProcedure [dbo].[spReceiveItem]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Philip Stinson
-- Create date: 27 Sept 2010
-- Description:	SP to Add to the table the downloded Items
-- @Result = 1 True/Success
-- @Result = 0 False/ Failed
-- =============================================
Create PROCEDURE [dbo].[spReceiveItem]
	@ItemID int,
	@Name varchar(250),
	@Description nvarchar(max),
	@SellPrice money,
	@Stocked bit,
	@ImgURL1 varchar(250),
	@ImgURL2 varchar(250),
	@ImgURL3 varchar(250),
	@ImgURL4 varchar(250),
	@ItemCategoryID int,
	@PreName varchar(250),
	@FullPrice money,
	@KeyVend varchar(50),
	@Barcode varchar(50),
	@Result int =0 output
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	BEGIN TRANSACTION ReceiveItem 

	INSERT INTO ReceivedItem
                      (ItemID, Name, Description, SellPrice, Stocked, ImgURL1, 
                      ImgURL2, ImgURL3, ImgURL4, ItemCategoryID, PreName,
                       FullPrice, KeyVend, Barcode)
    Values ( @ItemID, @Name, @Description, @SellPrice, @Stocked, @ImgURL1, 
			@ImgURL2, @ImgURL3, @ImgURL4, @ItemCategoryID, @PreName, 
			@FullPrice, @KeyVend,@Barcode)

    IF @@ERROR != 0 GOTO HANDLE_ERROR
	

	-- Set the result to success
	SET @Result = 1
	
	-- If we got to here then commit tran 
	COMMIT TRANSACTION ReceiveItem
	RETURN


HANDLE_ERROR:
    ROLLBACK TRANSACTION ReceiveItem
			-- Set the Reult to Error
		set @Result = 0
		 -- Raise an error and return
		RAISERROR ('Error in ReceiveCategory', 16, 1)
		RETURN -- Bail out of everything and do nothing else
		
END
GO
