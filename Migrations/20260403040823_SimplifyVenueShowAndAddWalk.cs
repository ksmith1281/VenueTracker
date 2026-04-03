using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VenueTracker.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyVenueShowAndAddWalk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "TourName",
                table: "Shows");

            migrationBuilder.CreateTable(
                name: "Walks",
                columns: table => new
                {
                    WalkId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShowId = table.Column<int>(type: "INTEGER", nullable: false),
                    WalkAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    MerchAmount = table.Column<decimal>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Walks", x => x.WalkId);
                    table.ForeignKey(
                        name: "FK_Walks_Shows_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Shows",
                        principalColumn: "ShowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Walks_ShowId",
                table: "Walks",
                column: "ShowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Walks");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Venues",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TourName",
                table: "Shows",
                type: "TEXT",
                maxLength: 200,
                nullable: true);
        }
    }
}
