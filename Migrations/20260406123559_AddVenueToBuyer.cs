using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VenueTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddVenueToBuyer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VenueId",
                table: "tBuyer",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tBuyer_VenueId",
                table: "tBuyer",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_tBuyer_tVenue_VenueId",
                table: "tBuyer",
                column: "VenueId",
                principalTable: "tVenue",
                principalColumn: "VenueId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tBuyer_tVenue_VenueId",
                table: "tBuyer");

            migrationBuilder.DropIndex(
                name: "IX_tBuyer_VenueId",
                table: "tBuyer");

            migrationBuilder.DropColumn(
                name: "VenueId",
                table: "tBuyer");
        }
    }
}
