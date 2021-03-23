using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using VirtualSports.Web.Models.DatabaseModels;

namespace VirtualSports.Web.Migrations
{
    public partial class AddMobileBets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<Bet>>(
                name: "MobileBets",
                table: "Users",
                type: "jsonb",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileBets",
                table: "Users");
        }
    }
}
