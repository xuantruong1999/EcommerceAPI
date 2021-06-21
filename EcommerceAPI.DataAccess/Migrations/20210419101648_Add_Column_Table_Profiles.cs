using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommerceAPI.DataAccess.Migrations
{
    public partial class Add_Column_Table_Profiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Profiles");
        }
    }
}
