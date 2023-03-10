/****** Object:  Table [dbo].[Product]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[ProdId] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NULL,
	[ProdCatId] [int] NOT NULL,
	[ProdName] [nvarchar](150) NOT NULL,
	[preName] [nvarchar](150) NULL,
	[keyVend] [nvarchar](50) NULL,
	[ProdSkuUpc] [nvarchar](50) NULL,
	[ProdWeight] [decimal](8, 2) NULL,
	[ProdHeight] [decimal](8, 2) NULL,
	[ProdWidth] [decimal](8, 2) NULL,
	[ProdDepth] [decimal](8, 2) NULL,
	[ProdPrice] [money] NOT NULL,
	[ProdCost] [money] NULL,
	[ProdDesc] [nvarchar](max) NOT NULL,
	[ProdImgUrl1] [nvarchar](250) NOT NULL,
	[ProdImgUrl2] [nvarchar](250) NULL,
	[ProdImgUrl3] [nvarchar](250) NULL,
	[ProdImgUrl4] [nvarchar](250) NULL,
	[IsActive] [bit] NOT NULL,
	[ProdAddedDate] [datetime] NULL,
	[ProdModDate] [datetime] NULL,
	[ProdDiscontinue] [bit] NULL,
	[ProdDiscontinueDate] [datetime] NULL,
	[fullPrice] [money] NULL,
	[xPos] [int] NULL,
	[yPos] [int] NULL,
	[Barcode] [varchar](50) NULL,
	[ArtistName] [nvarchar](20) NULL,
	[ArtistLocation] [nvarchar](50) NULL,
	[LongDescription] [nvarchar](50) NULL,
	[ProdimgUrl5] [nvarchar](50) NULL,
	[CollaboratorName1] [nvarchar](50) NULL,
	[Collaborator1] [nvarchar](50) NULL,
	[CollaboratorName2] [nvarchar](50) NULL,
	[Collaborator2] [nvarchar](50) NULL,
	[CollaboratorName3] [nvarchar](50) NULL,
	[Collaborator3] [nvarchar](50) NULL,
	[CollaboratorName4] [nvarchar](50) NULL,
	[Collaborator4] [nvarchar](50) NULL,
	[CollaboratorName5] [nvarchar](50) NULL,
	[Collaborator5] [nvarchar](50) NULL,
	[CollaboratorName6] [nvarchar](50) NULL,
	[Collaborator6] [nvarchar](50) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProdId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Product Categories] FOREIGN KEY([ProdCatId])
REFERENCES [dbo].[ProductCategories] ([ProdCatId])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Product Categories]
GO
