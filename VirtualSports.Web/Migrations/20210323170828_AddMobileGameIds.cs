using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtualSports.Web.Migrations
{
    public partial class AddMobileGameIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Queue<string>>(
                name: "RecentMobileGameIds",
                table: "Users",
                type: "jsonb",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecentMobileGameIds",
                table: "Users");
        }
    }
}
