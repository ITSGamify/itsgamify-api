using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace its.gamify.infras.Migrations
{
    /// <inheritdoc />
    public partial class V0_10_Modifylesson_Course_1_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "Course",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("3b72db68-b2c6-40d8-859e-b4996f8535a1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(734), new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(735) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("620d170e-c32e-4443-b450-32848c1eb5e9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(721), new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(722) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("71874fd3-1892-4d92-a77f-c85c0d16b8db"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(95), new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(98) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b002d347-66b9-4722-9547-5b2165abaa9f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(756), new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(756) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f7fa7c6b-f76a-4b95-8711-517eb8205a1a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(753), new DateTime(2025, 6, 27, 12, 21, 26, 49, DateTimeKind.Utc).AddTicks(753) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "Course");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("3b72db68-b2c6-40d8-859e-b4996f8535a1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 9, 14, 51, 177, DateTimeKind.Utc).AddTicks(1360), new DateTime(2025, 6, 27, 9, 14, 51, 177, DateTimeKind.Utc).AddTicks(1360) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("620d170e-c32e-4443-b450-32848c1eb5e9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 9, 14, 51, 177, DateTimeKind.Utc).AddTicks(1350), new DateTime(2025, 6, 27, 9, 14, 51, 177, DateTimeKind.Utc).AddTicks(1350) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("71874fd3-1892-4d92-a77f-c85c0d16b8db"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 9, 14, 51, 177, DateTimeKind.Utc).AddTicks(802), new DateTime(2025, 6, 27, 9, 14, 51, 177, DateTimeKind.Utc).AddTicks(804) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b002d347-66b9-4722-9547-5b2165abaa9f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 9, 14, 51, 177, DateTimeKind.Utc).AddTicks(1365), new DateTime(2025, 6, 27, 9, 14, 51, 177, DateTimeKind.Utc).AddTicks(1365) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f7fa7c6b-f76a-4b95-8711-517eb8205a1a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 6, 27, 9, 14, 51, 177, DateTimeKind.Utc).AddTicks(1362), new DateTime(2025, 6, 27, 9, 14, 51, 177, DateTimeKind.Utc).AddTicks(1363) });
        }
    }
}
