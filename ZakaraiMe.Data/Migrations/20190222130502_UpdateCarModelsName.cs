using Microsoft.EntityFrameworkCore.Migrations;

namespace ZakaraiMe.Data.Migrations
{
    public partial class UpdateCarModelsName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarMakes_MakeId",
                table: "CarModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModels_ModelId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarModels",
                table: "CarModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarMakes",
                table: "CarMakes");

            migrationBuilder.RenameTable(
                name: "CarModels",
                newName: "Models");

            migrationBuilder.RenameTable(
                name: "CarMakes",
                newName: "Makes");

            migrationBuilder.RenameIndex(
                name: "IX_CarModels_MakeId",
                table: "Models",
                newName: "IX_Models_MakeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Models",
                table: "Models",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Makes",
                table: "Makes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Models_ModelId",
                table: "Cars",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Models_Makes_MakeId",
                table: "Models",
                column: "MakeId",
                principalTable: "Makes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Models_ModelId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Models_Makes_MakeId",
                table: "Models");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Models",
                table: "Models");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Makes",
                table: "Makes");

            migrationBuilder.RenameTable(
                name: "Models",
                newName: "CarModels");

            migrationBuilder.RenameTable(
                name: "Makes",
                newName: "CarMakes");

            migrationBuilder.RenameIndex(
                name: "IX_Models_MakeId",
                table: "CarModels",
                newName: "IX_CarModels_MakeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarModels",
                table: "CarModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarMakes",
                table: "CarMakes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarMakes_MakeId",
                table: "CarModels",
                column: "MakeId",
                principalTable: "CarMakes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarModels_ModelId",
                table: "Cars",
                column: "ModelId",
                principalTable: "CarModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
