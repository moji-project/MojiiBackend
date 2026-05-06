using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MojiiBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserDatesCorrected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 109,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 110,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 17, 16, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 109,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 110,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) });
        }
    }
}
