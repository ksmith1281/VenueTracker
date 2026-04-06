using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VenueTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeId",
                table: "tShow",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tPaymentType",
                columns: table => new
                {
                    PaymentTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PaymentType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tPaymentType", x => x.PaymentTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tShow_PaymentTypeId",
                table: "tShow",
                column: "PaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_tShow_tPaymentType_PaymentTypeId",
                table: "tShow",
                column: "PaymentTypeId",
                principalTable: "tPaymentType",
                principalColumn: "PaymentTypeId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tShow_tPaymentType_PaymentTypeId",
                table: "tShow");

            migrationBuilder.DropTable(
                name: "tPaymentType");

            migrationBuilder.DropIndex(
                name: "IX_tShow_PaymentTypeId",
                table: "tShow");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "tShow");
        }
    }
}
