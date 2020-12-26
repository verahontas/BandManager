using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyRehearsalManager.Web.Migrations
{
    public partial class CreateReservationEquipmentPairsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EquipmentName",
                table: "ReservationEquipmentPairs",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EquipmentName",
                table: "ReservationEquipmentPairs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
