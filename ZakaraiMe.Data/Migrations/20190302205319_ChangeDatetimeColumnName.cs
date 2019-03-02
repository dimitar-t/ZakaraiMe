using Microsoft.EntityFrameworkCore.Migrations;

namespace ZakaraiMe.Data.Migrations
{
    public partial class ChangeDatetimeColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Journeys",
                newName: "SetOffTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SetOffTime",
                table: "Journeys",
                newName: "DateTime");
        }
    }
}
