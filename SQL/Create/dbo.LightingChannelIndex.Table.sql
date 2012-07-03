/****** Object:  Table [dbo].[LightingChannelIndex]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LightingChannelIndex](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Channel] [int] NULL,
	[Colour] [nchar](1) NULL,
	[Cluster] [nchar](1) NULL
) ON [PRIMARY]
GO
