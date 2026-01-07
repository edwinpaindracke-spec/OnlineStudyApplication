using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStudyApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseRequirements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinimumAverage",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumMathMark",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresMath",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumAverage",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "MinimumMathMark",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "RequiresMath",
                table: "Courses");
        }
    }
}
