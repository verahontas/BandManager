using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyRehearsalManager.Web.Migrations
{
    public partial class AddLogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Logo",
                table: "Studios",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Studios");
        }
    }
}
