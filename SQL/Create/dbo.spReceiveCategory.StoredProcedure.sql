/****** Object:  StoredProcedure [dbo].[spReceiveCategory]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Philip Stinson
-- Create date: 27 Sept 2010
-- Description:	SP to Add to the table the downloded Cats
-- @Result = 1 True/Success
-- @Result = 0 False/ Failed
-- =============================================
Create PROCEDURE [dbo].[spReceiveCategory]
	@ItemCategoryId [int],
	@Description nvarchar(100),
	@Name nvarchar(50) ,
	@ImgUrl nvarchar(100),
	@ImageID int,
	@Result int =0 output
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	BEGIN TRANSACTION ReceiveCategory 

	INSERT INTO ReceivedProductCategories
                      (ItemCategoryId,Description,Name,ImgUrl,ImageID)
    Values ( @ItemCategoryId,@Description,@Name,@ImgUrl,@ImageID)

    IF @@ERROR != 0 GOTO HANDLE_ERROR
	

	-- Set the result to success
	SET @Result = 1
	
	-- If we got to here then commit tran 
	COMMIT TRANSACTION ReceiveCategory
	RETURN


HANDLE_ERROR:
    ROLLBACK TRANSACTION ReceiveCategory
			-- Set the Reult to Error
		set @Result = 0
		 -- Raise an error and return
		RAISERROR ('Error in ReceiveCategory', 16, 1)
		RETURN -- Bail out of everything and do nothing else
		
END
GO
