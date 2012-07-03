/****** Object:  Table [dbo].[MdbCardTransaction]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MdbCardTransaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [int] NULL,
	[Status] [int] NULL,
	[Date] [datetime] NULL,
	[Reciept] [nchar](500) NULL,
	[Audit] [nchar](500) NULL,
	[ProductDescription] [nchar](100) NULL
) ON [PRIMARY]
GO
