using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WakaWaka.API.Migrations
{
    /// <inheritdoc />
    public partial class removedphoneproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Hotels");

            migrationBuilder.RenameColumn(
                name: "Phone_Number",
                table: "Restaurants",
                newName: "Telephone");

            migrationBuilder.RenameColumn(
                name: "Phone_Number",
                table: "Hotels",
                newName: "Telephone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Telephone",
                table: "Restaurants",
                newName: "Phone_Number");

            migrationBuilder.RenameColumn(
                name: "Telephone",
                table: "Hotels",
                newName: "Phone_Number");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
