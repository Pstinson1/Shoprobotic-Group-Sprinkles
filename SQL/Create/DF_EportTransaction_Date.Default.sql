/****** Object:  Default [DF_EportTransaction_Date]    Script Date: 10/20/2011 09:30:58 ******/
ALTER TABLE [dbo].[EportTransaction] ADD  CONSTRAINT [DF_EportTransaction_Date]  DEFAULT (getdate()) FOR [Date]
GO
