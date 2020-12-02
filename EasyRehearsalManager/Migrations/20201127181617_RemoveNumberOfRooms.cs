using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyRehearsalManager.Web.Migrations
{
    public partial class RemoveNumberOfRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfRooms",
                table: "Studios");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfRooms",
                table: "Studios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
