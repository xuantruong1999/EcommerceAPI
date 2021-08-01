using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommerceAPI.DataAccess.Migrations
{
    public partial class AddContraintProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_AspNetUsers_UserID",
                table: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Profile_UserID",
                table: "Profile");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Profile",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profile_UserID",
                table: "Profile",
                column: "UserID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_AspNetUsers_UserID",
                table: "Profile",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_AspNetUsers_UserID",
                table: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Profile_UserID",
                table: "Profile");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Profile",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_UserID",
                table: "Profile",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_AspNetUsers_UserID",
                table: "Profile",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
