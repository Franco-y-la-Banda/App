using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViajeHonesto.Migrations
{
    /// <inheritdoc />
    public partial class Changed_DestinationPhoto_VO_To_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppDestinationsPhotos");

            migrationBuilder.CreateTable(
                name: "AppDestinationPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDestinationPhotos", x => x.Id);
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
                name: "AppDestinationsPhotos",
                columns: table => new
                {
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDestinationsPhotos", x => new { x.DestinationId, x.Id });
                    table.ForeignKey(
                        name: "FK_AppDestinationsPhotos_AppDestinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "AppDestinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
