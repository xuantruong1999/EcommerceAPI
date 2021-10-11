using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommerceAPI.DataAccess.Migrations
{
    public partial class HandleConcurrencyTableProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_AspNetUsers_UserID",
                table: "Profile");

         
            migrationBuilder.RenameIndex(
                name: "IX_Profile_UserID",
                table: "Profile",
                newName: "IX_Profile_UserID");

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "Profile",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_AspNetUsers_UserID",
                table: "Profile",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_AspNetUsers_UserID",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Profile");

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
