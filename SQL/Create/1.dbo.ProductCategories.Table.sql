/****** Object:  Table [dbo].[ProductCategories]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategories](
	[ProdCatId] [int] IDENTITY(1,1) NOT NULL,
	[ProdCatDesc] [nvarchar](100) NULL,
	[ProdCatName] [nvarchar](50) NOT NULL,
	[ProdCatImgUrl] [nvarchar](100) NULL,
	[ItemCategoryID] [int] NULL,
 CONSTRAINT [PK_Product Categories] PRIMARY KEY CLUSTERED 
(
	[ProdCatId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
