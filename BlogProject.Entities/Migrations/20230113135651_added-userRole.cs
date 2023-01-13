using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogProject.Entities.Migrations
{
    public partial class addeduserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRole",
                table: "AppUsers",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "AppUsers");
        }
    }
}
