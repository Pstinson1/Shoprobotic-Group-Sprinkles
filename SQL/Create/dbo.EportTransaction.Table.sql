/****** Object:  Table [dbo].[EportTransaction]    Script Date: 10/20/2011 09:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EportTransaction](
	[EportTransactionID] [int] IDENTITY(1,1) NOT NULL,
	[TransactionID] [int] NULL,
	[TransactionAmount] [int] NULL,
	[TransactionDetails] [varchar](100) NULL,
	[TransactionResult] [int] NULL,
	[ErrorCode] [int] NULL,
	[Success] [int] NULL,
	[Date] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
