//using System;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Migrations;

//namespace SySIntegral.Core.Data.Migrations
//{
//    [DbContext(typeof(ApplicationDbContext))]
//    [Migration("20220401122603_CreateEggRegistryTable")]
//    public class CreateEggRegistryTable : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "EggRegistry",
//                columns: table => new
//                {
//                    //Id = table.Column<string>(type: "nvarchar(50)", nullable: false),
//                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
//                    DeviceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
//                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    WhiteEggsCount = table.Column<int>(type: "int", nullable: true),
//                    ColorEggsCount = table.Column<int>(type: "int", nullable: true),
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_EggRegistry", x => x.Id);
//                });
            
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable("EggRegistry");
//        }
//    }
//}
