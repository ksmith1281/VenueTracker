using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VenueTracker.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesToSingularWithTPrefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename tables to singular with 't' prefix
            migrationBuilder.RenameTable(
                name: "Venues",
                newName: "tVenue");

            migrationBuilder.RenameTable(
                name: "Shows",
                newName: "tShow");

            migrationBuilder.RenameTable(
                name: "Statuses",
                newName: "tStatus");

            migrationBuilder.RenameTable(
                name: "Buyers",
                newName: "tBuyer");

            migrationBuilder.RenameTable(
                name: "Subcontractors",
                newName: "tSubcontractor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the renames
            migrationBuilder.RenameTable(
                name: "tVenue",
                newName: "Venues");

            migrationBuilder.RenameTable(
                name: "tShow",
                newName: "Shows");

            migrationBuilder.RenameTable(
                name: "tStatus",
                newName: "Statuses");

            migrationBuilder.RenameTable(
                name: "tBuyer",
                newName: "Buyers");

            migrationBuilder.RenameTable(
                name: "tSubcontractor",
                newName: "Subcontractors");
        }
    }
}
