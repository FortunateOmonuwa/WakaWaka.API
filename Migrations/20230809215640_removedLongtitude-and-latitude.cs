using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WakaWaka.API.Migrations
{
    /// <inheritdoc />
    public partial class removedLongtitudeandlatitude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelReviews_Restaurants_RestaurantId",
                table: "HotelReviews");

            migrationBuilder.DropIndex(
                name: "IX_HotelReviews_RestaurantId",
                table: "HotelReviews");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Longtitude",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Longtitude",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "HotelReviews");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "RestaurantReviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantReviews_RestaurantId",
                table: "RestaurantReviews",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantReviews_Restaurants_RestaurantId",
                table: "RestaurantReviews",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantReviews_Restaurants_RestaurantId",
                table: "RestaurantReviews");

            migrationBuilder.DropIndex(
                name: "IX_RestaurantReviews_RestaurantId",
                table: "RestaurantReviews");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "RestaurantReviews");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Restaurants",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longtitude",
                table: "Restaurants",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Hotels",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longtitude",
                table: "Hotels",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "HotelReviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelReviews_RestaurantId",
                table: "HotelReviews",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelReviews_Restaurants_RestaurantId",
                table: "HotelReviews",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
