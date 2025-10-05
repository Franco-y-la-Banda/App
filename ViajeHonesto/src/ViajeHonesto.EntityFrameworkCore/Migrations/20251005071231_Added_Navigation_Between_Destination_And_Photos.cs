using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViajeHonesto.Migrations
{
    /// <inheritdoc />
    public partial class Added_Navigation_Between_Destination_And_Photos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DestinationPhotos");

            migrationBuilder.CreateTable(
                name: "AppDestinationPhotos",
                columns: table => new
                {
                    PhotoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDestinationPhotos", x => new { x.PhotoId, x.DestinationId });
                    table.ForeignKey(
                        name: "FK_AppDestinationPhotos_AppDestinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "AppDestinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppDestinationPhotos_DestinationId",
                table: "AppDestinationPhotos",
                column: "DestinationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppDestinationPhotos");

            migrationBuilder.CreateTable(
                name: "DestinationPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestinationPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DestinationPhotos_AppDestinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "AppDestinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DestinationPhotos_DestinationId",
                table: "DestinationPhotos",
                column: "DestinationId");
        }
    }
}
