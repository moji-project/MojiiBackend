using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MojiiBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPostImageUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "ImageUrls",
                table: "Posts",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);

            migrationBuilder.Sql("""
                UPDATE "Posts"
                SET "ImageUrls" = CASE
                    WHEN "ImageUrl" IS NULL OR btrim("ImageUrl") = '' THEN ARRAY[]::text[]
                    ELSE ARRAY["ImageUrl"]
                END;
            """);

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Posts");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Posts_ImageUrls_Max5",
                table: "Posts",
                sql: "cardinality(\"ImageUrls\") <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Posts_ImageUrls_Max5",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Posts",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE "Posts"
                SET "ImageUrl" = CASE
                    WHEN COALESCE(cardinality("ImageUrls"), 0) = 0 THEN NULL
                    ELSE LEFT("ImageUrls"[1], 500)
                END;
            """);

            migrationBuilder.DropColumn(
                name: "ImageUrls",
                table: "Posts");
        }
    }
}
