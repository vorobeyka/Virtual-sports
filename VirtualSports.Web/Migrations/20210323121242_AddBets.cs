using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtualSports.Web.Migrations
{
    public partial class AddBets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "BetsIds",
                table: "Users",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "MobileBetsIds",
                table: "Users",
                type: "text[]",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BetsIds",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MobileBetsIds",
                table: "Users");
        }
    }
}
