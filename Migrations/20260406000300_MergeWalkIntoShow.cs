using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VenueTracker.Migrations
{
    /// <inheritdoc />
    public partial class MergeWalkIntoShow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Walks");

            migrationBuilder.AddColumn<decimal>(
                name: "MerchAmount",
                table: "Shows",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Shows",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WalkAmount",
                table: "Shows",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MerchAmount",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "WalkAmount",
                table: "Shows");

            migrationBuilder.CreateTable(
                name: "Walks",
                columns: table => new
                {
                    WalkId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShowId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MerchAmount = table.Column<decimal>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WalkAmount = table.Column<decimal>(type: "TEXT", nullable: false)
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
    }
}
