/****** Object:  Table [dbo].[Users]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[FobNumber] [nvarchar](50) NOT NULL,
	[AccessType] [int] NOT NULL,
	[Passcode] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 = Stock Control, 1 = Engineer, 2 = Administrator.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'AccessType'
GO
