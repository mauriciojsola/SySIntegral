SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CheckPoint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CheckPointType] [int] NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[ParentId] [int] NULL,
	[AssetId] [int] NOT NULL,
	[InputDeviceId] [int] NULL,
 CONSTRAINT [PK_CheckPoint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX [IX_CheckPoint_AssetId] ON [dbo].[CheckPoint]
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[CheckPoint]  WITH CHECK ADD  CONSTRAINT [FK_CheckPoint_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([Id])
GO
ALTER TABLE [dbo].[CheckPoint] CHECK CONSTRAINT [FK_CheckPoint_Asset]
GO


/****** Object:  Table [dbo].[CheckPointCount]    Script Date: 7/27/2022 6:40:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CheckPointCount](
	[CheckPointId] [int] NOT NULL,
	[EggRegistryId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Index [IX_CheckPointCountEggRegistry]    Script Date: 7/27/2022 6:40:05 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_CheckPointCountEggRegistry] ON [dbo].[CheckPointCount]
(
	[CheckPointId] ASC,
	[EggRegistryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CheckPoint] ADD  CONSTRAINT [DF_CheckPoint_CheckPointType]  DEFAULT ((2)) FOR [CheckPointType]
GO
ALTER TABLE [dbo].[CheckPoint]  WITH CHECK ADD  CONSTRAINT [FK_CheckPoint_CheckPointParent] FOREIGN KEY([ParentId])
REFERENCES [dbo].[CheckPoint] ([Id])
GO
ALTER TABLE [dbo].[CheckPoint] CHECK CONSTRAINT [FK_CheckPoint_CheckPointParent]
GO
ALTER TABLE [dbo].[CheckPoint]  WITH CHECK ADD  CONSTRAINT [FK_CheckPoint_InputDevice] FOREIGN KEY([InputDeviceId])
REFERENCES [dbo].[InputDevice] ([Id])
GO
ALTER TABLE [dbo].[CheckPoint] CHECK CONSTRAINT [FK_CheckPoint_InputDevice]
GO
ALTER TABLE [dbo].[CheckPointCount]  WITH CHECK ADD  CONSTRAINT [FK_CheckPointCount_CheckPoint] FOREIGN KEY([CheckPointId])
REFERENCES [dbo].[CheckPoint] ([Id])
GO
ALTER TABLE [dbo].[CheckPointCount] CHECK CONSTRAINT [FK_CheckPointCount_CheckPoint]
GO
ALTER TABLE [dbo].[CheckPointCount]  WITH CHECK ADD  CONSTRAINT [FK_CheckPointCount_EggRegistry] FOREIGN KEY([EggRegistryId])
REFERENCES [dbo].[EggRegistry] ([Id])
GO
ALTER TABLE [dbo].[CheckPointCount] CHECK CONSTRAINT [FK_CheckPointCount_EggRegistry]
GO