/****** Object:  Default [DF_Users_FobNumber]    Script Date: 10/20/2011 09:30:58 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_FobNumber]  DEFAULT (N'''') FOR [FobNumber]
GO
