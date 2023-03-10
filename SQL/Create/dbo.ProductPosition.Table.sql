/****** Object:  Table [dbo].[ProductPosition]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductPosition](
	[PosId] [int] IDENTITY(1,1) NOT NULL,
	[ProdId] [int] NULL,
	[xPos] [smallint] NOT NULL,
	[yPos] [smallint] NOT NULL,
	[vdo] [tinyint] NOT NULL,
	[RestockLevel] [tinyint] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[LastVended] [bit] NOT NULL,
	[FridgeId] [int] NULL,
	[ItemsInStock] [int] NULL,
	[PickAttempts] [int] NULL,
	[ActiveWander] [bit] NULL,
	[SequenceScore] [int] NULL,
	[xVisualPosition] [int] NULL,
	[yVisualPosition] [int] NULL,
	[ContainerNumber] [nvarchar](50) NULL,
 CONSTRAINT [PK_Product Position Info] PRIMARY KEY CLUSTERED 
(
	[PosId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The index of this product position' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductPosition', @level2type=N'COLUMN',@level2name=N'PosId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The product that is being positioned for vend' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductPosition', @level2type=N'COLUMN',@level2name=N'ProdId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The horizontal position of this product' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductPosition', @level2type=N'COLUMN',@level2name=N'xPos'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The vertical position of this product' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductPosition', @level2type=N'COLUMN',@level2name=N'yPos'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The Vertical Delivery Offset of this product' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductPosition', @level2type=N'COLUMN',@level2name=N'vdo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The quantity of product required before notification sent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductPosition', @level2type=N'COLUMN',@level2name=N'RestockLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date this position was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductPosition', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last date this position was edited by a user' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductPosition', @level2type=N'COLUMN',@level2name=N'ModDate'
GO
ALTER TABLE [dbo].[ProductPosition]  WITH CHECK ADD  CONSTRAINT [FK_Product Position Info_Product] FOREIGN KEY([ProdId])
REFERENCES [dbo].[Product] ([ProdId])
GO
ALTER TABLE [dbo].[ProductPosition] CHECK CONSTRAINT [FK_Product Position Info_Product]
GO
