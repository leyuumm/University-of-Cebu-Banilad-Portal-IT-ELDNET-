using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToSectionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
            name: "IX_ScheduleEntry_SectionId",
            table: "ScheduleEntry",
            column: "SectionId",
            unique: true);  // This ensures the SectionId is unique
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
            name: "IX_ScheduleEntry_SectionId",
            table: "ScheduleEntry");
        }
    }
}
