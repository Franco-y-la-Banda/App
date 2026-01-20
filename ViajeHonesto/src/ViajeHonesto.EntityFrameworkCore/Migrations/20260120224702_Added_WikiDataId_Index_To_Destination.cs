using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViajeHonesto.Migrations
{
    /// <inheritdoc />
    public partial class Added_WikiDataId_Index_To_Destination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WikiDataId",
                table: "AppDestinations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppDestinations_WikiDataId",
                table: "AppDestinations",
                column: "WikiDataId",
                unique: true,
                filter: "[WikiDataId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppDestinations_WikiDataId",
                table: "AppDestinations");

            migrationBuilder.DropColumn(
                name: "WikiDataId",
                table: "AppDestinations");
        }
    }
}
