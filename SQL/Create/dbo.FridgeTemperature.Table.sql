/****** Object:  Table [dbo].[FridgeTemperature]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FridgeTemperature](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateTime] [datetime] NULL,
	[Temperature] [decimal](18, 1) NULL,
	[Event] [int] NULL
) ON [PRIMARY]
GO
