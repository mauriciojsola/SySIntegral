using System;
using Microsoft.EntityFrameworkCore.Migrations;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Entities.Roles;

namespace SySIntegral.Core.Data.Migrations
{
    public partial class InitializeAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"

                                INSERT INTO AspNetRoles (Id,Name,NormalizedName) VALUES ('{Guid.NewGuid().ToString()}','{SySRoles.Administrator}','ADMINISTRATOR');
                                INSERT INTO AspNetRoles (Id,Name,NormalizedName) VALUES ('{Guid.NewGuid().ToString()}','{SySRoles.OrganizationAdministrator}','ORGANIZATION_ADMINISTRATOR');
                                INSERT INTO AspNetRoles (Id,Name,NormalizedName) VALUES ('{Guid.NewGuid().ToString()}','{SySRoles.User}','USER');

                                INSERT INTO Organization(Name) VALUES ('SySIntegral SRL');

                                INSERT INTO [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], 
                                [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], 
                                [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], 
                                [AccessFailedCount], [FirstName], [LastName], [OrganizationId]) 
                                VALUES (N'd3b5b81b-db61-4371-a637-18d08a95ee1a', N'mauriciojsola11@gmail.com', 
                                N'MAURICIOJSOLA11@GMAIL.COM', N'mauriciojsola11@gmail.com', 
                                N'MAURICIOJSOLA11@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEIJRmRTbpjTMus1VEgVodwAy59HVpHRhAfdoaDsZ4F+n70UEPkrp9OgJv50b+PaMhg==', 
                                N'JGT5M647H3UOTVWMTM434U26VMMQEKQD', N'b5914415-18f8-4912-ab64-2844e7300445', NULL, 0, 0, NULL, 1, 0, NULL, NULL, (SELECT Id FROM Organization WHERE 
                                [Name]='SySIntegral SRL'))



                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"

                    DELETE FROM AspNetUsers WHERE UserName = 'mauriciojsola11@gmail.com';
                    DELETE FROM Organization WHERE [Name]='SySIntegral SRL';
                    DELETE FROM AspNetRoles WHERE [Name]='{SySRoles.Administrator}';
                    DELETE FROM AspNetRoles WHERE [Name]='{SySRoles.OrganizationAdministrator}';
                    DELETE FROM AspNetRoles WHERE [Name]='{SySRoles.User}';

");
        }
    }
}

