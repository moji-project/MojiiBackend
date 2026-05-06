using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MojiiBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserDatesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "BirthDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2003, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "BirthDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2003, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "BirthDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2003, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "BirthDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2003, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "BirthDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2003, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "BirthDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2003, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "BirthDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2003, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "BirthDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2003, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 109,
                columns: new[] { "BirthDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2003, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 110,
                columns: new[] { "BirthDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2003, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetUsers");
        }
    }
}
