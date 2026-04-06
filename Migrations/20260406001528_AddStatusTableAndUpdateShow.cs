using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VenueTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusTableAndUpdateShow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Shows");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Shows",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StatusName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.StatusId);
                });

            // Seed statuses
            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Confirmed" },
                    { 3, "Rejected" },
                    { 4, "Cancelled" }
                });

            // Update existing shows to have Pending status
            migrationBuilder.Sql("UPDATE Shows SET StatusId = 1 WHERE StatusId IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_StatusId",
                table: "Shows",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Statuses_StatusId",
                table: "Shows",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Restrict);

            // Make StatusId not nullable after setting defaults
            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Shows",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Statuses_StatusId",
                table: "Shows");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Shows_StatusId",
                table: "Shows");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Shows",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Shows",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            // Convert StatusId back to Status string
            migrationBuilder.Sql("UPDATE Shows SET Status = CASE StatusId WHEN 1 THEN 'Pending' WHEN 2 THEN 'Confirmed' WHEN 3 THEN 'Rejected' WHEN 4 THEN 'Cancelled' ELSE 'Pending' END");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Shows");
        }
    }
}
