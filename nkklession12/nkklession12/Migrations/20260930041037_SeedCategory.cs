using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace nkklession12.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "CreatedDate", "Name", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 9, 30, 11, 10, 36, 672, DateTimeKind.Local).AddTicks(5809), "Điện thoại", (byte)1 },
                    { 2, new DateTime(2026, 9, 30, 11, 10, 36, 672, DateTimeKind.Local).AddTicks(6285), "Laptop", (byte)1 },
                    { 3, new DateTime(2026, 9, 30, 11, 10, 36, 672, DateTimeKind.Local).AddTicks(6287), "Phụ kiện", (byte)1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
