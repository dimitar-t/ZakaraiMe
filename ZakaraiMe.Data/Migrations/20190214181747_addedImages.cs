using Microsoft.EntityFrameworkCore.Migrations;

namespace ZakaraiMe.Data.Migrations
{
    public partial class addedImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureFileName",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    FileName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.FileName);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfilePictureFileName",
                table: "AspNetUsers",
                column: "ProfilePictureFileName");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_ProfilePictureFileName",
                table: "AspNetUsers",
                column: "ProfilePictureFileName",
                principalTable: "Images",
                principalColumn: "FileName",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_ProfilePictureFileName",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfilePictureFileName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePictureFileName",
                table: "AspNetUsers");
        }
    }
}
