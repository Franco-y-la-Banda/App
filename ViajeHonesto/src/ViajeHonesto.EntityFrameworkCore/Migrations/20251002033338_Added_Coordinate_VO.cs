using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViajeHonesto.Migrations
{
    /// <inheritdoc />
    public partial class Added_Coordinate_VO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Latitude",
                table: "AppDestinations",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Longitude",
                table: "AppDestinations",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "AppDestinations");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "AppDestinations");
        }
    }
}
