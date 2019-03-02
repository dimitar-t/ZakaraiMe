using Microsoft.EntityFrameworkCore.Migrations;

namespace ZakaraiMe.Data.Migrations
{
    public partial class AddDecimalPrecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "StartPointY",
                table: "Journeys",
                type: "decimal(18, 6)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "StartPointX",
                table: "Journeys",
                type: "decimal(18, 6)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "EndPointY",
                table: "Journeys",
                type: "decimal(18, 6)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "EndPointX",
                table: "Journeys",
                type: "decimal(18, 6)",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "StartPointY",
                table: "Journeys",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "StartPointX",
                table: "Journeys",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "EndPointY",
                table: "Journeys",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "EndPointX",
                table: "Journeys",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 6)");
        }
    }
}
