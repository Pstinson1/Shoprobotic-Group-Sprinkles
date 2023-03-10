/****** Object:  Table [dbo].[StockAdjustments]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockAdjustments](
	[StockID] [int] IDENTITY(1,1) NOT NULL,
	[ProdID] [int] NOT NULL,
	[PosID] [int] NOT NULL,
	[Adjustment] [smallint] NOT NULL,
	[AdjustmentDate] [datetime] NOT NULL,
	[Transmitted] [int] NULL,
 CONSTRAINT [PK_StockAdjustments] PRIMARY KEY CLUSTERED 
(
	[StockID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A unique ID that indexes every stock adjustment made to a product' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StockAdjustments', @level2type=N'COLUMN',@level2name=N'StockID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Foreign Key reference to the product being adjusted' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StockAdjustments', @level2type=N'COLUMN',@level2name=N'ProdID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The adjustment level (positive or negative) of the product' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StockAdjustments', @level2type=N'COLUMN',@level2name=N'Adjustment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date the adjustment was made' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StockAdjustments', @level2type=N'COLUMN',@level2name=N'AdjustmentDate'
GO
ALTER TABLE [dbo].[StockAdjustments]  WITH CHECK ADD  CONSTRAINT [FK_StockAdjustments_Product] FOREIGN KEY([ProdID])
REFERENCES [dbo].[Product] ([ProdId])
GO
ALTER TABLE [dbo].[StockAdjustments] CHECK CONSTRAINT [FK_StockAdjustments_Product]
GO
ALTER TABLE [dbo].[StockAdjustments]  WITH CHECK ADD  CONSTRAINT [FK_StockAdjustments_ProductPosition] FOREIGN KEY([PosID])
REFERENCES [dbo].[ProductPosition] ([PosId])
GO
ALTER TABLE [dbo].[StockAdjustments] CHECK CONSTRAINT [FK_StockAdjustments_ProductPosition]
GO
