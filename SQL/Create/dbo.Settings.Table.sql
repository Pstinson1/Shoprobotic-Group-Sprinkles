/****** Object:  Table [dbo].[Settings]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[Id] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[Value] [nvarchar](100) NULL,
	[Section] [nvarchar](50) NULL,
	[Explaination] [nvarchar](200) NULL
) ON [PRIMARY]
GO
