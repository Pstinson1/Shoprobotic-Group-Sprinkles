/****** Object:  Table [dbo].[Metrics]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Metrics](
	[MetID] [int] IDENTITY(1,1) NOT NULL,
	[MetXMin] [smallint] NOT NULL,
	[MetXMax] [smallint] NOT NULL,
	[MetYMin] [smallint] NOT NULL,
	[MetYMax] [smallint] NOT NULL,
	[ConfigDate] [datetime] NOT NULL,
	[LastModDate] [datetime] NULL,
 CONSTRAINT [PK_Metrics] PRIMARY KEY CLUSTERED 
(
	[MetID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
