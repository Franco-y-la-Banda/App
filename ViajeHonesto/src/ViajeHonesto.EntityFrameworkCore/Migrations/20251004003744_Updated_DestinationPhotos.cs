using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViajeHonesto.Migrations
{
    /// <inheritdoc />
    public partial class Updated_DestinationPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppDestinationPhotos_AppDestinations_DestinationId",
                table: "AppDestinationPhotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppDestinationPhotos",
                table: "AppDestinationPhotos");

            migrationBuilder.RenameTable(
                name: "AppDestinationPhotos",
                newName: "DestinationPhotos");

            migrationBuilder.RenameIndex(
                name: "IX_AppDestinationPhotos_DestinationId",
                table: "DestinationPhotos",
                newName: "IX_DestinationPhotos_DestinationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DestinationPhotos",
                table: "DestinationPhotos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DestinationPhotos_AppDestinations_DestinationId",
                table: "DestinationPhotos",
                column: "DestinationId",
                principalTable: "AppDestinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DestinationPhotos_AppDestinations_DestinationId",
                table: "DestinationPhotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DestinationPhotos",
                table: "DestinationPhotos");

            migrationBuilder.RenameTable(
                name: "DestinationPhotos",
                newName: "AppDestinationPhotos");

            migrationBuilder.RenameIndex(
                name: "IX_DestinationPhotos_DestinationId",
                table: "AppDestinationPhotos",
                newName: "IX_AppDestinationPhotos_DestinationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppDestinationPhotos",
                table: "AppDestinationPhotos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppDestinationPhotos_AppDestinations_DestinationId",
                table: "AppDestinationPhotos",
                column: "DestinationId",
                principalTable: "AppDestinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
