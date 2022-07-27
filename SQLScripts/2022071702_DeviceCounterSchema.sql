SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CounterDevice](
	[Id] [int] NOT NULL,
	[CounterDeviceType] [int] NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[ParentId] [int] NULL,
	[InputDeviceId] [int] NULL,
 CONSTRAINT [PK_CounterDevice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CounterDeviceCount]    Script Date: 7/27/2022 6:40:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CounterDeviceCount](
	[CounterDeviceId] [int] NOT NULL,
	[EggRegistryId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Index [IX_CounterDeviceCountEggRegistry]    Script Date: 7/27/2022 6:40:05 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_CounterDeviceCountEggRegistry] ON [dbo].[CounterDeviceCount]
(
	[CounterDeviceId] ASC,
	[EggRegistryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CounterDevice] ADD  CONSTRAINT [DF_CounterDevice_CounterDeviceType]  DEFAULT ((2)) FOR [CounterDeviceType]
GO
ALTER TABLE [dbo].[CounterDevice]  WITH CHECK ADD  CONSTRAINT [FK_CounterDevice_CounterDeviceParent] FOREIGN KEY([ParentId])
REFERENCES [dbo].[CounterDevice] ([Id])
GO
ALTER TABLE [dbo].[CounterDevice] CHECK CONSTRAINT [FK_CounterDevice_CounterDeviceParent]
GO
ALTER TABLE [dbo].[CounterDevice]  WITH CHECK ADD  CONSTRAINT [FK_CounterDevice_InputDevice] FOREIGN KEY([InputDeviceId])
REFERENCES [dbo].[InputDevice] ([Id])
GO
ALTER TABLE [dbo].[CounterDevice] CHECK CONSTRAINT [FK_CounterDevice_InputDevice]
GO
ALTER TABLE [dbo].[CounterDeviceCount]  WITH CHECK ADD  CONSTRAINT [FK_CounterDeviceCount_CounterDevice] FOREIGN KEY([CounterDeviceId])
REFERENCES [dbo].[CounterDevice] ([Id])
GO
ALTER TABLE [dbo].[CounterDeviceCount] CHECK CONSTRAINT [FK_CounterDeviceCount_CounterDevice]
GO
ALTER TABLE [dbo].[CounterDeviceCount]  WITH CHECK ADD  CONSTRAINT [FK_CounterDeviceCount_EggRegistry] FOREIGN KEY([EggRegistryId])
REFERENCES [dbo].[EggRegistry] ([Id])
GO
ALTER TABLE [dbo].[CounterDeviceCount] CHECK CONSTRAINT [FK_CounterDeviceCount_EggRegistry]
GO