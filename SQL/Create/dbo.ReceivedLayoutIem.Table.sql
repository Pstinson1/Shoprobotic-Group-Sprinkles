/****** Object:  Table [dbo].[ReceivedLayoutIem]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReceivedLayoutIem](
	[LayoutItemID] [int] NOT NULL,
	[ContainerNumber] [varchar](50) NOT NULL,
	[ItemID] [int] NULL,
	[LayoutPrice] [money] NOT NULL,
	[xPos] [int] NOT NULL,
	[yPos] [int] NOT NULL,
	[Active] [bit] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
