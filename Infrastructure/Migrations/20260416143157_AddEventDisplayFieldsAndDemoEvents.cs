using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MojiiBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEventDisplayFieldsAndDemoEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Events",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DateLabel",
                table: "Events",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DayLabel",
                table: "Events",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultInterestedCount",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Events",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MonthLabel",
                table: "Events",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.Sql("""
                WITH base_user AS (
                    SELECT u."Id" AS user_id, u."OrganizationId" AS organization_id
                    FROM "AspNetUsers" u
                    ORDER BY u."Id"
                    LIMIT 1
                )
                INSERT INTO "Events"
                (
                    "Name",
                    "Description",
                    "Location",
                    "Address",
                    "DateLabel",
                    "MonthLabel",
                    "DayLabel",
                    "DefaultInterestedCount",
                    "StartDate",
                    "ImageUrl",
                    "IsPublished",
                    "OrganizationId",
                    "CreatorUserId",
                    "CreatedAt",
                    "UpdatedAt"
                )
                SELECT
                    'Soiree d''integration X Halloween Party',
                    'Viens celebrer Halloween lors d''une soiree d''integration festive et immersive.
                DJ set, ambiance Halloween, bar sur place et bonne vibes toute la nuit.
                Preparez vos meilleurs costumes pour le concours.',
                    'Villeurbanne',
                    '34 Rue Antoine, 69003 Lyon',
                    'Samedi 26 novembre 2025 - 22h30',
                    'NOV',
                    '26',
                    85,
                    '2025-11-26T22:30:00Z'::timestamptz,
                    '/uploads/events/party.png',
                    TRUE,
                    base_user.organization_id,
                    base_user.user_id,
                    NOW(),
                    NOW()
                FROM base_user
                WHERE NOT EXISTS (
                    SELECT 1 FROM "Events" e WHERE e."Name" = 'Soiree d''integration X Halloween Party'
                );
            """);

            migrationBuilder.Sql("""
                WITH base_user AS (
                    SELECT u."Id" AS user_id, u."OrganizationId" AS organization_id
                    FROM "AspNetUsers" u
                    ORDER BY u."Id"
                    LIMIT 1
                )
                INSERT INTO "Events"
                (
                    "Name",
                    "Description",
                    "Location",
                    "Address",
                    "DateLabel",
                    "MonthLabel",
                    "DayLabel",
                    "DefaultInterestedCount",
                    "StartDate",
                    "ImageUrl",
                    "IsPublished",
                    "OrganizationId",
                    "CreatorUserId",
                    "CreatedAt",
                    "UpdatedAt"
                )
                SELECT
                    'Neon Campus Party',
                    'Une nuit electro et light show pour lancer la fin de semestre.
                DJ set, bar soft et zone photo neon toute la soiree.',
                    'Lyon',
                    '12 Quai Victor Augagneur, 69003 Lyon',
                    'Vendredi 12 decembre 2025 - 21h00',
                    'DEC',
                    '12',
                    64,
                    '2025-12-12T21:00:00Z'::timestamptz,
                    'https://picsum.photos/seed/neon-party/1200/800',
                    TRUE,
                    base_user.organization_id,
                    base_user.user_id,
                    NOW(),
                    NOW()
                FROM base_user
                WHERE NOT EXISTS (
                    SELECT 1 FROM "Events" e WHERE e."Name" = 'Neon Campus Party'
                );
            """);

            migrationBuilder.Sql("""
                WITH base_user AS (
                    SELECT u."Id" AS user_id, u."OrganizationId" AS organization_id
                    FROM "AspNetUsers" u
                    ORDER BY u."Id"
                    LIMIT 1
                )
                INSERT INTO "Events"
                (
                    "Name",
                    "Description",
                    "Location",
                    "Address",
                    "DateLabel",
                    "MonthLabel",
                    "DayLabel",
                    "DefaultInterestedCount",
                    "StartDate",
                    "ImageUrl",
                    "IsPublished",
                    "OrganizationId",
                    "CreatorUserId",
                    "CreatedAt",
                    "UpdatedAt"
                )
                SELECT
                    'Winter Welcome Night',
                    'Soiree de rentree d''hiver avec dancefloor, animations et surprises.
                Dress code noir & blanc pour une ambiance elegante.',
                    'Villeurbanne',
                    '78 Avenue Roger Salengro, 69100 Villeurbanne',
                    'Jeudi 08 janvier 2026 - 20h30',
                    'JAN',
                    '08',
                    72,
                    '2026-01-08T20:30:00Z'::timestamptz,
                    'https://picsum.photos/seed/winter-welcome/1200/800',
                    TRUE,
                    base_user.organization_id,
                    base_user.user_id,
                    NOW(),
                    NOW()
                FROM base_user
                WHERE NOT EXISTS (
                    SELECT 1 FROM "Events" e WHERE e."Name" = 'Winter Welcome Night'
                );
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM "Events"
                WHERE "Name" IN
                (
                    'Soiree d''integration X Halloween Party',
                    'Neon Campus Party',
                    'Winter Welcome Night'
                );
            """);

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DateLabel",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DayLabel",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DefaultInterestedCount",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MonthLabel",
                table: "Events");
        }
    }
}
