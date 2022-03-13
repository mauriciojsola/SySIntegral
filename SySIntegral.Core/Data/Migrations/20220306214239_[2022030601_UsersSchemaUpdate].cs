using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SySIntegral.Core.Data.Migrations
{
    public partial class _2022030601_UsersSchemaUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //            //migrationBuilder.CreateTable(
            //            //    name: "Organization",
            //            //    columns: table => new
            //            //    {
            //            //        Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
            //            //        Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
            //            //        ReleaseYear = table.Column<int>(type: "int", nullable: false),
            //            //        Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
            //            //        RuntimeMinutes = table.Column<int>(type: "int", nullable: false)
            //            //    },
            //            //    constraints: table =>
            //            //    {
            //            //        table.PrimaryKey("PK_Movies", x => x.Id);
            //            //    });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                });

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                type: "nvarchar(50)",
                name: "OrganizationId",
                table: "AspNetUsers",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Organization",
                table: "AspNetUsers",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrganizationId",
                table: "AspNetUsers",
                column: "OrganizationId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Organization",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Organization");
        }
    }
}
