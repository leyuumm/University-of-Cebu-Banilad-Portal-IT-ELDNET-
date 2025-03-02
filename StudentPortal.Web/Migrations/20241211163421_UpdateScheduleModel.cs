using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScheduleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove the primary key constraint temporarily
            migrationBuilder.DropPrimaryKey(
                name: "PK_ScheduleEntry",
                table: "ScheduleEntry");

            // Rename 'Course' to 'SubjectName'
            migrationBuilder.RenameColumn(
                name: "Course",
                table: "ScheduleEntry",
                newName: "SubjectName");

            // Drop the old 'Id' column to avoid conflict with the new 'Id' column with IDENTITY
            migrationBuilder.DropColumn(
                name: "Id",
                table: "ScheduleEntry");

            // Add new 'Id' column with the 'IDENTITY' property
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ScheduleEntry",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Add 'SubjectEDP' and 'SubjectID' columns
            migrationBuilder.AddColumn<string>(
                name: "SubjectEDP",
                table: "ScheduleEntry",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SubjectID",
                table: "ScheduleEntry",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Create index for 'SubjectID' column
            migrationBuilder.CreateIndex(
                name: "IX_ScheduleEntry_SubjectID",
                table: "ScheduleEntry",
                column: "SubjectID");

            // Add foreign key for 'SubjectID' linking to 'SubjectEntry'
            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleEntry_SubjectEntry_SubjectID",
                table: "ScheduleEntry",
                column: "SubjectID",
                principalTable: "SubjectEntry",
                principalColumn: "SubjectID",
                onDelete: ReferentialAction.Cascade);

            // Recreate the primary key constraint on 'Id'
            migrationBuilder.AddPrimaryKey(
                name: "PK_ScheduleEntry",
                table: "ScheduleEntry",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint for 'SubjectID'
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleEntry_SubjectEntry_SubjectID",
                table: "ScheduleEntry");

            // Drop index for 'SubjectID'
            migrationBuilder.DropIndex(
                name: "IX_ScheduleEntry_SubjectID",
                table: "ScheduleEntry");

            // Drop the 'SubjectEDP' and 'SubjectID' columns
            migrationBuilder.DropColumn(
                name: "SubjectEDP",
                table: "ScheduleEntry");

            migrationBuilder.DropColumn(
                name: "SubjectID",
                table: "ScheduleEntry");

            // Drop the new 'Id' column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "ScheduleEntry");

            // Re-add the old 'Id' column (without IDENTITY property)
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "ScheduleEntry",
                type: "bigint",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Recreate the primary key constraint on 'Id'
            migrationBuilder.AddPrimaryKey(
                name: "PK_ScheduleEntry",
                table: "ScheduleEntry",
                column: "Id");
        }
    }
}
