/****** Object:  Table [dbo].[ReceivedItem]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReceivedItem](
	[ItemID] [int] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[SellPrice] [money] NOT NULL,
	[Stocked] [bit] NOT NULL,
	[ImgURL1] [varchar](250) NOT NULL,
	[ImgURL2] [varchar](250) NOT NULL,
	[ImgURL3] [varchar](250) NOT NULL,
	[ImgURL4] [varchar](250) NOT NULL,
	[ItemCategoryID] [int] NULL,
	[PreName] [varchar](250) NOT NULL,
	[FullPrice] [money] NOT NULL,
	[KeyVend] [varchar](50) NOT NULL,
	[Barcode] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
