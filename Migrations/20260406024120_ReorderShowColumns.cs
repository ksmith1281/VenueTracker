using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VenueTracker.Migrations
{
    /// <inheritdoc />
    public partial class ReorderShowColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys=off;");
            migrationBuilder.Sql(@"CREATE TABLE tShow_new (
                ShowId INTEGER NOT NULL CONSTRAINT PK_tShow PRIMARY KEY AUTOINCREMENT,
                StatusId INTEGER NOT NULL,
                VenueId INTEGER NOT NULL,
                ShowDate TEXT NOT NULL,
                ShowName TEXT NULL,
                WalkAmount TEXT NOT NULL DEFAULT '0.0',
                PaymentTypeId INTEGER NULL,
                MerchAmount TEXT NULL,
                Deal TEXT NULL,
                Notes TEXT NULL,
                CreatedOn TEXT NOT NULL,
                UpdatedOn TEXT NOT NULL,
                CONSTRAINT FK_tShow_tStatus_StatusId FOREIGN KEY (StatusId) REFERENCES tStatus (StatusId) ON DELETE RESTRICT,
                CONSTRAINT FK_tShow_tVenue_VenueId FOREIGN KEY (VenueId) REFERENCES tVenue (VenueId) ON DELETE CASCADE,
                CONSTRAINT FK_tShow_tPaymentType_PaymentTypeId FOREIGN KEY (PaymentTypeId) REFERENCES tPaymentType (PaymentTypeId) ON DELETE SET NULL
            );");
            migrationBuilder.Sql(@"INSERT INTO tShow_new (ShowId, StatusId, VenueId, ShowDate, ShowName, WalkAmount, PaymentTypeId, MerchAmount, Deal, Notes, CreatedOn, UpdatedOn)
                SELECT ShowId, StatusId, VenueId, ShowDate, ShowName, WalkAmount, PaymentTypeId, MerchAmount, Deal, Notes, CreatedOn, UpdatedOn FROM tShow;");
            migrationBuilder.Sql("DROP TABLE tShow;");
            migrationBuilder.Sql("ALTER TABLE tShow_new RENAME TO tShow;");
            migrationBuilder.Sql("PRAGMA foreign_keys=on;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys=off;");
            migrationBuilder.Sql(@"CREATE TABLE tShow_old (
                ShowId INTEGER NOT NULL CONSTRAINT PK_tShow PRIMARY KEY AUTOINCREMENT,
                CreatedOn TEXT NOT NULL,
                Deal TEXT NULL,
                MerchAmount TEXT NULL,
                Notes TEXT NULL,
                PaymentTypeId INTEGER NULL,
                ShowDate TEXT NOT NULL,
                ShowName TEXT NULL,
                StatusId INTEGER NOT NULL,
                UpdatedOn TEXT NOT NULL,
                VenueId INTEGER NOT NULL,
                WalkAmount TEXT NOT NULL DEFAULT '0.0',
                CONSTRAINT FK_tShow_tPaymentType_PaymentTypeId FOREIGN KEY (PaymentTypeId) REFERENCES tPaymentType (PaymentTypeId) ON DELETE SET NULL,
                CONSTRAINT FK_tShow_tStatus_StatusId FOREIGN KEY (StatusId) REFERENCES tStatus (StatusId) ON DELETE RESTRICT,
                CONSTRAINT FK_tShow_tVenue_VenueId FOREIGN KEY (VenueId) REFERENCES tVenue (VenueId) ON DELETE CASCADE
            );");
            migrationBuilder.Sql(@"INSERT INTO tShow_old (ShowId, CreatedOn, Deal, MerchAmount, Notes, PaymentTypeId, ShowDate, ShowName, StatusId, UpdatedOn, VenueId, WalkAmount)
                SELECT ShowId, CreatedOn, Deal, MerchAmount, Notes, PaymentTypeId, ShowDate, ShowName, StatusId, UpdatedOn, VenueId, WalkAmount FROM tShow;");
            migrationBuilder.Sql("DROP TABLE tShow;");
            migrationBuilder.Sql("ALTER TABLE tShow_old RENAME TO tShow;");
            migrationBuilder.Sql("PRAGMA foreign_keys=on;");
        }
    }
}
