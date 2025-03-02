using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScheduleEntrySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Alter SubjectSched to be non-nullable
            migrationBuilder.AlterColumn<string>(
                name: "SubjectSched",
                table: "ScheduleEntry",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            // Alter Room to be non-nullable
            migrationBuilder.AlterColumn<string>(
                name: "Room",
                table: "ScheduleEntry",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            // Alter SubjectName to be non-nullable
            migrationBuilder.AlterColumn<string>(
                name: "SubjectName",
                table: "ScheduleEntry",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert SubjectSched to nullable
            migrationBuilder.AlterColumn<string>(
                name: "SubjectSched",
                table: "ScheduleEntry",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Revert Room to nullable
            migrationBuilder.AlterColumn<string>(
                name: "Room",
                table: "ScheduleEntry",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Revert SubjectName to nullable
            migrationBuilder.AlterColumn<string>(
                name: "SubjectName",
                table: "ScheduleEntry",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
