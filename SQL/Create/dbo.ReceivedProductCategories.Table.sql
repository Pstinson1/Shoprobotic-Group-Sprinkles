/****** Object:  Table [dbo].[ReceivedProductCategories]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReceivedProductCategories](
	[ItemCategoryId] [int] NULL,
	[Description] [nvarchar](100) NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ImgUrl] [nvarchar](100) NULL,
	[ImageID] [int] NULL
) ON [PRIMARY]
GO
