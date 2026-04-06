using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VenueTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddShowSubcontractor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tShowSubcontractor",
                columns: table => new
                {
                    ShowSubcontractorId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubcontractorId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShowId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tShowSubcontractor", x => x.ShowSubcontractorId);
                    table.ForeignKey(
                        name: "FK_tShowSubcontractor_tShow_ShowId",
                        column: x => x.ShowId,
                        principalTable: "tShow",
                        principalColumn: "ShowId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tShowSubcontractor_tSubcontractor_SubcontractorId",
                        column: x => x.SubcontractorId,
                        principalTable: "tSubcontractor",
                        principalColumn: "SubcontractorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tShowSubcontractor_ShowId",
                table: "tShowSubcontractor",
                column: "ShowId");

            migrationBuilder.CreateIndex(
                name: "IX_tShowSubcontractor_SubcontractorId",
                table: "tShowSubcontractor",
                column: "SubcontractorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tShowSubcontractor");
        }
    }
}
