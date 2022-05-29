SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Asset](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[OrganizationId] [int] NOT NULL,
 CONSTRAINT [PK_Asset] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Device]    Script Date: 4/23/2022 7:33:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Device](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UniqueId] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[AssetId] [int] NOT NULL,
 CONSTRAINT [PK_Device] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrganizationId]    Script Date: 4/23/2022 7:33:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrganizationId] ON [dbo].[Asset]
(
	[OrganizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AssetId]    Script Date: 4/23/2022 7:33:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_AssetId] ON [dbo].[Device]
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Asset_UniqueId]    Script Date: 4/23/2022 7:33:06 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Asset_UniqueId] ON [dbo].[Device]
(
	[UniqueId] ASC,
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Asset]  WITH CHECK ADD  CONSTRAINT [FK_Asset_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_Organization]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [FK_Device_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([Id])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [FK_Device_Asset]
GO



-- 	INSERT INTO Asset(NAME,organizationId) VALUES('Galpon 1 Santa Rosa', (SELECT o.Id FROM Organization AS o WHERE o.Name='Jacob SRL'))
 	
--INSERT INTO Device([Description], UniqueId, AssetId) 	 VALUES('Lector Huevos 1',
--                                                     	        'D6F2045C-0B45-49CE-9E55-B4F0E33EB270', (SELECT a.Id  FROM Asset a WHERE a.Name='Galpon 1 Santa Rosa'))
