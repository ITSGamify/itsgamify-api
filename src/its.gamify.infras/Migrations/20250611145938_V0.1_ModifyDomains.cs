using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace its.gamify.infras.Migrations
{
    /// <inheritdoc />
    public partial class V01_ModifyDomains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_CourseSection_CourseSectionId",
                table: "Quiz");

            migrationBuilder.DropTable(
                name: "LearningMaterial");

            migrationBuilder.RenameColumn(
                name: "CourseSectionId",
                table: "Quiz",
                newName: "LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_Quiz_CourseSectionId",
                table: "Quiz",
                newName: "IX_Quiz_LessonId");

            migrationBuilder.AddColumn<Guid>(
                name: "LessonId",
                table: "Practice",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CourseId",
                table: "CourseSection",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Lesson",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationInMinutes = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningProgressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lesson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lesson_CourseSection_CourseSectionId",
                        column: x => x.CourseSectionId,
                        principalTable: "CourseSection",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lesson_LearningProgress_LearningProgressId",
                        column: x => x.LearningProgressId,
                        principalTable: "LearningProgress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Practice_LessonId",
                table: "Practice",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_CourseSectionId",
                table: "Lesson",
                column: "CourseSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_LearningProgressId",
                table: "Lesson",
                column: "LearningProgressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Practice_Lesson_LessonId",
                table: "Practice",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Lesson_LessonId",
                table: "Quiz",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Practice_Lesson_LessonId",
                table: "Practice");

            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Lesson_LessonId",
                table: "Quiz");

            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropIndex(
                name: "IX_Practice_LessonId",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Practice");

            migrationBuilder.RenameColumn(
                name: "LessonId",
                table: "Quiz",
                newName: "CourseSectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Quiz_LessonId",
                table: "Quiz",
                newName: "IX_Quiz_CourseSectionId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CourseId",
                table: "CourseSection",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "LearningMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationInMinutes = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningMaterial_CourseSection_CourseSectionId",
                        column: x => x.CourseSectionId,
                        principalTable: "CourseSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningMaterial_CourseSectionId",
                table: "LearningMaterial",
                column: "CourseSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_CourseSection_CourseSectionId",
                table: "Quiz",
                column: "CourseSectionId",
                principalTable: "CourseSection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
