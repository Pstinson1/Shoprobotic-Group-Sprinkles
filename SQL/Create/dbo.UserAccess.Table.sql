/****** Object:  Table [dbo].[UserAccess]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserAccess](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateChecked] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
	[Passcode] [varchar](50) NOT NULL,
	[Result] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
