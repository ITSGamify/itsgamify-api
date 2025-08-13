using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace its.gamify.infras.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCourse_Users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CourseType",
                table: "Course",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("3b72db68-b2c6-40d8-859e-b4996f8535a1"),
                columns: new[] { "CreatedDate", "Name", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 28, 7, 21, 29, 896, DateTimeKind.Utc).AddTicks(6143), "TRAININGSTAFF", new DateTime(2025, 6, 28, 7, 21, 29, 896, DateTimeKind.Utc).AddTicks(6143) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("620d170e-c32e-4443-b450-32848c1eb5e9"),
                columns: new[] { "CreatedDate", "Name", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 28, 7, 21, 29, 896, DateTimeKind.Utc).AddTicks(6132), "LEADER", new DateTime(2025, 6, 28, 7, 21, 29, 896, DateTimeKind.Utc).AddTicks(6132) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("71874fd3-1892-4d92-a77f-c85c0d16b8db"),
                columns: new[] { "CreatedDate", "Name", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 28, 7, 21, 29, 896, DateTimeKind.Utc).AddTicks(5583), "EMPLOYEE", new DateTime(2025, 6, 28, 7, 21, 29, 896, DateTimeKind.Utc).AddTicks(5586) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b002d347-66b9-4722-9547-5b2165abaa9f"),
                columns: new[] { "CreatedDate", "Name", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 28, 7, 21, 29, 896, DateTimeKind.Utc).AddTicks(6147), "ADMIN", new DateTime(2025, 6, 28, 7, 21, 29, 896, DateTimeKind.Utc).AddTicks(6147) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f7fa7c6b-f76a-4b95-8711-517eb8205a1a"),
                columns: new[] { "CreatedDate", "Name", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 28, 7, 21, 29, 896, DateTimeKind.Utc).AddTicks(6145), "MANAGER", new DateTime(2025, 6, 28, 7, 21, 29, 896, DateTimeKind.Utc).AddTicks(6145) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "CourseType",
                table: "Course",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("3b72db68-b2c6-40d8-859e-b4996f8535a1"),
                columns: new[] { "CreatedDate", "Name", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(734), "TrainingStaff", new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(735) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("620d170e-c32e-4443-b450-32848c1eb5e9"),
                columns: new[] { "CreatedDate", "Name", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(721), "Leader", new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(722) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("71874fd3-1892-4d92-a77f-c85c0d16b8db"),
                columns: new[] { "CreatedDate", "Name", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(95), "Employee", new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(98) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b002d347-66b9-4722-9547-5b2165abaa9f"),
                columns: new[] { "CreatedDate", "Name", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(756), "Admin", new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(756) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f7fa7c6b-f76a-4b95-8711-517eb8205a1a"),
                columns: new[] { "CreatedDate", "Name", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(753), "Manager", new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(753) });
        }
    }
}
