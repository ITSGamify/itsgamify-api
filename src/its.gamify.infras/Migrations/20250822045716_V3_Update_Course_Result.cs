using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace its.gamify.infras.Migrations
{
    /// <inheritdoc />
    public partial class V3_Update_Course_Result : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "CourseResult");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "CourseResult");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("3b72db68-b2c6-40d8-859e-b4996f8535a1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 8, 22, 4, 57, 12, 596, DateTimeKind.Utc).AddTicks(3854), new DateTime(2025, 8, 22, 4, 57, 12, 596, DateTimeKind.Utc).AddTicks(3854) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("620d170e-c32e-4443-b450-32848c1eb5e9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 8, 22, 4, 57, 12, 596, DateTimeKind.Utc).AddTicks(3799), new DateTime(2025, 8, 22, 4, 57, 12, 596, DateTimeKind.Utc).AddTicks(3801) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("71874fd3-1892-4d92-a77f-c85c0d16b8db"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 8, 22, 4, 57, 12, 596, DateTimeKind.Utc).AddTicks(2179), new DateTime(2025, 8, 22, 4, 57, 12, 596, DateTimeKind.Utc).AddTicks(2184) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b002d347-66b9-4722-9547-5b2165abaa9f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 8, 22, 4, 57, 12, 596, DateTimeKind.Utc).AddTicks(3864), new DateTime(2025, 8, 22, 4, 57, 12, 596, DateTimeKind.Utc).AddTicks(3864) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f7fa7c6b-f76a-4b95-8711-517eb8205a1a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 8, 22, 4, 57, 12, 596, DateTimeKind.Utc).AddTicks(3859), new DateTime(2025, 8, 22, 4, 57, 12, 596, DateTimeKind.Utc).AddTicks(3860) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "CourseResult",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "CourseResult",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("3b72db68-b2c6-40d8-859e-b4996f8535a1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 8, 21, 17, 59, 37, 438, DateTimeKind.Utc).AddTicks(33), new DateTime(2025, 8, 21, 17, 59, 37, 438, DateTimeKind.Utc).AddTicks(33) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("620d170e-c32e-4443-b450-32848c1eb5e9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 8, 21, 17, 59, 37, 438, DateTimeKind.Utc).AddTicks(17), new DateTime(2025, 8, 21, 17, 59, 37, 438, DateTimeKind.Utc).AddTicks(18) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("71874fd3-1892-4d92-a77f-c85c0d16b8db"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 8, 21, 17, 59, 37, 437, DateTimeKind.Utc).AddTicks(9176), new DateTime(2025, 8, 21, 17, 59, 37, 437, DateTimeKind.Utc).AddTicks(9179) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b002d347-66b9-4722-9547-5b2165abaa9f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 8, 21, 17, 59, 37, 438, DateTimeKind.Utc).AddTicks(38), new DateTime(2025, 8, 21, 17, 59, 37, 438, DateTimeKind.Utc).AddTicks(38) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f7fa7c6b-f76a-4b95-8711-517eb8205a1a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 8, 21, 17, 59, 37, 438, DateTimeKind.Utc).AddTicks(36), new DateTime(2025, 8, 21, 17, 59, 37, 438, DateTimeKind.Utc).AddTicks(36) });
        }
    }
}
