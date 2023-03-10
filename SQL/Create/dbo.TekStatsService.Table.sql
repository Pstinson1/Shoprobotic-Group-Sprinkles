/****** Object:  Table [dbo].[TekStatsService]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TekStatsService](
	[TekStatsServiceID] [int] IDENTITY(1,1) NOT NULL,
	[NewData] [bit] NULL,
	[LastDownload] [datetime] NULL,
	[DownloadInterval] [int] NULL,
 CONSTRAINT [PK_TekStatsService] PRIMARY KEY CLUSTERED 
(
	[TekStatsServiceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
