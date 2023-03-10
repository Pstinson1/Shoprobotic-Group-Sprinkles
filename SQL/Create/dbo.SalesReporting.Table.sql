/****** Object:  Table [dbo].[SalesReporting]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesReporting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [nvarchar](50) NOT NULL,
	[Date] [datetime] NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductName] [nvarchar](255) NOT NULL,
	[Barcode] [nvarchar](50) NOT NULL,
	[Price] [int] NOT NULL,
	[StockLevel] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ContainerNumber] [nvarchar](50) NULL
) ON [PRIMARY]
GO
