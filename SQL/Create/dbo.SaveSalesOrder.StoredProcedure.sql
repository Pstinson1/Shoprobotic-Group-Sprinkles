/****** Object:  StoredProcedure [dbo].[SaveSalesOrder]    Script Date: 10/20/2011 09:30:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveSalesOrder]
(
@OrderDate DATETIME,
@PaymentMethodId INT,
@LineItems TEXT
)

AS

SET NOCOUNT ON

BEGIN TRAN

DECLARE @hdoc INT
EXEC sp_xml_preparedocument @hdoc OUTPUT, @LineItems

INSERT INTO Orders (OrderDate, PaymentMethodId)
VALUES (@OrderDate, @PaymentMethodId)

DECLARE @OrderId INT
SET @OrderId = SCOPE_IDENTITY()

INSERT INTO OrderItems (OrderId, ProdId, Qty, ItemPrice, PosId)
SELECT @OrderId, x.ProdId, x.Qty, x.ItemPrice, x.PosId
FROM OPENXML (@hdoc, '/lineItems/item', 2) 
WITH 
(
ProdId INT 'ProdId', 
Qty INT 'Qty', 
ItemPrice MONEY 'ItemPrice',
PosId INT 'PosId'
) AS x

INSERT INTO StockAdjustments (ProdId, PosId, Adjustment, AdjustmentDate)
SELECT x.ProdId, x.PosId, (x.Qty * -1), @OrderDate
FROM OPENXML (@hdoc, '/lineItems/item', 2) 
WITH 
(
ProdId INT 'ProdId', 
Qty INT 'Qty', 
PosId INT 'PosId'
) AS x

EXEC sp_xml_removedocument @hdoc

IF @@ERROR <> 0 
ROLLBACK TRAN
ELSE
COMMIT TRAN
GO
