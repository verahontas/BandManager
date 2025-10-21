using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyRehearsalManager.Web.Migrations
{
    public partial class RemoveUserImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_Reservations_ReservationId",
                table: "Equipments");

            migrationBuilder.DropTable(
                name: "UserImages");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_ReservationId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Equipments");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Equipments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageLarge = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ImageSmall = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserImages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_ReservationId",
                table: "Equipments",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserImages_UserId",
                table: "UserImages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_Reservations_ReservationId",
                table: "Equipments",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
