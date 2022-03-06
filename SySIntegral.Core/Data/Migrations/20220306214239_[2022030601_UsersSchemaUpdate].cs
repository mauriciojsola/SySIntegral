using Microsoft.EntityFrameworkCore.Migrations;

namespace SySIntegral.Core.Data.Migrations
{
    public partial class _2022030601_UsersSchemaUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "AspNetUsers",
                nullable: true);
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "AspNetUsers");

            
        }
    }
}
