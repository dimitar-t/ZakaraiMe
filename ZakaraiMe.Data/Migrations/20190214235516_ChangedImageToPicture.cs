using Microsoft.EntityFrameworkCore.Migrations;

namespace ZakaraiMe.Data.Migrations
{
    public partial class ChangedImageToPicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_ProfilePictureFileName",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    FileName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.FileName);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Pictures_ProfilePictureFileName",
                table: "AspNetUsers",
                column: "ProfilePictureFileName",
                principalTable: "Pictures",
                principalColumn: "FileName",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Pictures_ProfilePictureFileName",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Pictures");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_ProfilePictureFileName",
                table: "AspNetUsers",
                column: "ProfilePictureFileName",
                principalTable: "Images",
                principalColumn: "FileName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
