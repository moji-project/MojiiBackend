using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MojiiBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EventAnneeScolaire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnneeScolaire",
                table: "Filieres",
                type: "character varying(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Filieres",
                keyColumn: "Id",
                keyValue: 101,
                column: "AnneeScolaire",
                value: "2025/2026");

            migrationBuilder.UpdateData(
                table: "Filieres",
                keyColumn: "Id",
                keyValue: 102,
                column: "AnneeScolaire",
                value: "2025/2026");

            migrationBuilder.UpdateData(
                table: "Filieres",
                keyColumn: "Id",
                keyValue: 103,
                column: "AnneeScolaire",
                value: "2025/2026");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnneeScolaire",
                table: "Filieres");
        }
    }
}
