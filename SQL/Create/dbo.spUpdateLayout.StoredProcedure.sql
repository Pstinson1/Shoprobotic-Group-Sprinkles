/****** Object:  StoredProcedure [dbo].[spUpdateLayout]    Script Date: 10/20/2011 09:30:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Philip Stinson
-- Create date: 27 Sept 2010
-- Description:	SP to Take the new data and upload layout
-- @Result = 1 True/Success
-- @Result = 0 False/ Failed
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateLayout]
AS
BEGIN
	Declare @myError Int

	SET NOCOUNT ON;
	BEGIN TRANSACTION UpdateLayout 


	-- Do the Categories First
	-- New Cats
--	insert into ProductCategories ( ProdCatDesc,ProdCatName,ProdCatImgUrl,ItemCategoryID)
--	select [description],name,imgurl,itemcategoryid from ReceivedProductCategories
--	WHERE ItemCategoryID not in ( select distinct itemcategoryid from ReceivedProductCategories)
	
	-- select mine
	insert into ProductCategories ( ProdCatDesc,ProdCatName,ProdCatImgUrl,ItemCategoryID)
		select [description],name,imgurl,itemcategoryid from ReceivedProductCategories
	WHERE ItemCategoryID not in ( 
	select distinct itemcategoryid from ProductCategories)
	

	IF @@ERROR != 0 GOTO HANDLE_ERROR
	
	--Update Cats
	update ProductCategories set
	ProdCatDesc = rc.[Description],
	ProdCatName = rc.Name, 
	ProdCatImgUrl = rc.ImgUrl, 
	ItemCategoryID = rc.ItemCategoryId
	
	from  ReceivedProductCategories rc inner join ProductCategories pc on
	rc.ItemCategoryId = pc.ItemCategoryID
	
	IF @@ERROR != 0 GOTO HANDLE_ERROR

	--Insert the Products that don't already exist	
	INSERT INTO Product
                      (ItemID, ProdName, ProdDesc, ProdPrice, IsActive, ProdImgUrl1, 
                      ProdImgUrl2, ProdImgUrl3, ProdImgUrl4, ProdCatId, preName, 
                      fullPrice, keyVend, Barcode)
	SELECT     ReceivedItem.ItemID, ReceivedItem.Name, ReceivedItem.Description, ReceivedItem.SellPrice, ReceivedItem.Stocked, ReceivedItem.ImgURL1,
						ReceivedItem.ImgURL2, ReceivedItem.ImgURL3, ReceivedItem.ImgURL4, ProductCategories.ProdCatId, ReceivedItem.PreName, ReceivedItem.FullPrice, 
						ReceivedItem.KeyVend, ReceivedItem.Barcode
	FROM         ReceivedItem INNER JOIN
						ProductCategories ON ReceivedItem.ItemCategoryID = ProductCategories.ItemCategoryID
	WHERE     (ReceivedItem.ItemID NOT IN
							(SELECT DISTINCT ItemID
	                            FROM          Product AS Product_1))
	                            
    IF @@ERROR != 0 GOTO HANDLE_ERROR
    
	--Update the products that do exist
	update Product set
	ItemID = RI.ItemID, 
	ProdName = RI.Name, 
	ProdDesc = RI.Description, 
	ProdPrice = RI.SellPrice, 
	IsActive = RI.Stocked, 
	ProdImgUrl1 = RI.ImgURL1,
	ProdImgUrl2 = RI.ImgURL2, 
	ProdImgUrl3 = RI.ImgURL3, 
	ProdImgUrl4 = RI.ImgURL4, 
	ProdCatId= PC.ProdCatId, 
	preName = RI.PreName, 
	fullPrice = RI.FullPrice, 
	keyVend = RI.KeyVend, 
	Barcode = RI.Barcode
	
	FROM         ReceivedItem RI INNER JOIN
						ProductCategories  PC ON RI.ItemCategoryID = PC.ItemCategoryID
						inner join Product on Product.ItemID = ri.itemID
	WHERE     (RI.ItemID  IN
							(SELECT DISTINCT ItemID
	                            FROM          Product AS Product_1))
	                            
	                           	                            
	IF @@ERROR != 0 GOTO HANDLE_ERROR

    	-- Bin all the current product Positions
	update ProductPosition set ProdId = null 	
	IF @@ERROR != 0 GOTO HANDLE_ERROR
	
	--Update the positions
	update ProductPosition
	set 
	
	--ProductPosition.xPos = RI.xPos, 
	--ProductPosition.yPos = RI.yPos, 
	ProductPosition.ProdId=P.ProdId, 
	ProductPosition.IsActive =RI.Active
	
    FROM         Product P INNER JOIN
                      ReceivedLayoutIem RI ON P.ItemID = RI.ItemID INNER JOIN
                      ProductPosition PP ON RI.ContainerNumber = PP.ContainerNumber
    
    
    
        IF @@ERROR != 0 GOTO HANDLE_ERROR
	
	UPDATE  tekstatsservice set NewData = 0

	COMMIT TRANSACTION UpdateLayout
	RETURN

HANDLE_ERROR:
    ROLLBACK TRANSACTION UpdateLayout
		
		RAISERROR ('Error in UpdateLayout', 16, 1)
		RETURN -- Bail out of everything and do nothing else
		
END
GO
