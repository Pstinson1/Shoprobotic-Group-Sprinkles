/****** Object:  Table [dbo].[VmcSettings]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VmcSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Mnemonic] [nvarchar](10) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[ReadValue] [int] NOT NULL,
	[WriteValue] [int] NOT NULL,
	[DefaultValue] [int] NOT NULL,
	[Explaination] [nvarchar](200) NULL
) ON [PRIMARY]
GO
