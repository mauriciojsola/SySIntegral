USE [aspnet-SySIntegral-53bc9b9d-9d6a-45d4-8429-2a2761773502]
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'5a78b70e-d1db-4153-9834-271f38be5876', N'Administrador de Organización', N'ADMINISTRADOR DE ORGANIZACIÓN', NULL)
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'72522397-a868-4e90-a83c-65a59a096708', N'Usuario', N'USUARIO', NULL)
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'9b6ca584-244f-452f-8fc9-f2c53d0a778b', N'Administrador', N'ADMINISTRADOR', NULL)
GO
SET IDENTITY_INSERT [dbo].[Organization] ON 
GO
INSERT [dbo].[Organization] ([Id], [Name]) VALUES (1, N'SySIntegral SRL')
GO
SET IDENTITY_INSERT [dbo].[Organization] OFF
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [OrganizationId]) VALUES (N'd3b5b81b-db61-4371-a637-18d08a95ee1a', N'mauriciojsola11@gmail.com', N'MAURICIOJSOLA11@GMAIL.COM', N'mauriciojsola11@gmail.com', N'MAURICIOJSOLA11@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEIl2RetMTYqbAwt7LpikUDPedy03eD0xUydXy9AsM1H/80npw50ZiDNEjQuhtaSydQ==', N'APABYYAD4MSV5YL4OQWCYS2E5WCKCVSV', N'ad089f0b-b728-4ebd-a4dc-db6a2cb9deda', NULL, 0, 0, NULL, 1, 0, N'Mauricio', N'Sola', 1)

GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'd3b5b81b-db61-4371-a637-18d08a95ee1a', N'9b6ca584-244f-452f-8fc9-f2c53d0a778b')
GO
