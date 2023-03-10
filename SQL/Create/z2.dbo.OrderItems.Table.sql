/****** Object:  Table [dbo].[OrderItems]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[OrderItemID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[ProdID] [int] NOT NULL,
	[PosID] [int] NOT NULL,
	[ItemPrice] [money] NOT NULL,
	[Qty] [int] NOT NULL,
	[Transmitted] [int] NULL,
 CONSTRAINT [PK_Order Items] PRIMARY KEY CLUSTERED 
(
	[OrderItemID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_Order Items_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_Order Items_Orders]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_Order Items_Product] FOREIGN KEY([ProdID])
REFERENCES [dbo].[Product] ([ProdId])
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_Order Items_Product]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_Order Items_Product Position Info] FOREIGN KEY([PosID])
REFERENCES [dbo].[ProductPosition] ([PosId])
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_Order Items_Product Position Info]
GO
