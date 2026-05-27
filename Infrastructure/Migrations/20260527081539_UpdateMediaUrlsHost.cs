using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MojiiBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMediaUrlsHost : Migration
    {
        private const string NewHost = "http://10.70.0.165:5282";
        private const string OldHostLan = "http://10.70.1.30:5282";
        private const string OldHostLocalHttp = "http://localhost:5282";
        private const string OldHostLocalHttps = "https://localhost:7171";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"""
                UPDATE "AspNetUsers"
                SET "ProfilePicUrl" = replace(
                    replace(
                        replace("ProfilePicUrl", '{OldHostLan}', '{NewHost}'),
                        '{OldHostLocalHttp}', '{NewHost}'
                    ),
                    '{OldHostLocalHttps}', '{NewHost}'
                )
                WHERE "ProfilePicUrl" IS NOT NULL
                  AND (
                    "ProfilePicUrl" LIKE '{OldHostLan}%'
                    OR "ProfilePicUrl" LIKE '{OldHostLocalHttp}%'
                    OR "ProfilePicUrl" LIKE '{OldHostLocalHttps}%'
                  );

                UPDATE "Channels"
                SET "ImageUrl" = replace(
                    replace(
                        replace("ImageUrl", '{OldHostLan}', '{NewHost}'),
                        '{OldHostLocalHttp}', '{NewHost}'
                    ),
                    '{OldHostLocalHttps}', '{NewHost}'
                )
                WHERE "ImageUrl" IS NOT NULL
                  AND (
                    "ImageUrl" LIKE '{OldHostLan}%'
                    OR "ImageUrl" LIKE '{OldHostLocalHttp}%'
                    OR "ImageUrl" LIKE '{OldHostLocalHttps}%'
                  );

                UPDATE "Events"
                SET "ImageUrl" = replace(
                    replace(
                        replace("ImageUrl", '{OldHostLan}', '{NewHost}'),
                        '{OldHostLocalHttp}', '{NewHost}'
                    ),
                    '{OldHostLocalHttps}', '{NewHost}'
                )
                WHERE "ImageUrl" IS NOT NULL
                  AND (
                    "ImageUrl" LIKE '{OldHostLan}%'
                    OR "ImageUrl" LIKE '{OldHostLocalHttp}%'
                    OR "ImageUrl" LIKE '{OldHostLocalHttps}%'
                  );

                UPDATE "Posts"
                SET "ImageUrls" = (
                    SELECT COALESCE(
                        array_agg(
                            replace(
                                replace(
                                    replace(img, '{OldHostLan}', '{NewHost}'),
                                    '{OldHostLocalHttp}', '{NewHost}'
                                ),
                                '{OldHostLocalHttps}', '{NewHost}'
                            )
                        ),
                        ARRAY[]::text[]
                    )
                    FROM unnest("ImageUrls") AS img
                )
                WHERE "ImageUrls" IS NOT NULL
                  AND EXISTS (
                      SELECT 1
                      FROM unnest("ImageUrls") AS img
                      WHERE img LIKE '{OldHostLan}%'
                         OR img LIKE '{OldHostLocalHttp}%'
                         OR img LIKE '{OldHostLocalHttps}%'
                  );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"""
                UPDATE "AspNetUsers"
                SET "ProfilePicUrl" = replace("ProfilePicUrl", '{NewHost}', '{OldHostLan}')
                WHERE "ProfilePicUrl" IS NOT NULL
                  AND "ProfilePicUrl" LIKE '{NewHost}%';

                UPDATE "Channels"
                SET "ImageUrl" = replace("ImageUrl", '{NewHost}', '{OldHostLan}')
                WHERE "ImageUrl" IS NOT NULL
                  AND "ImageUrl" LIKE '{NewHost}%';

                UPDATE "Events"
                SET "ImageUrl" = replace("ImageUrl", '{NewHost}', '{OldHostLan}')
                WHERE "ImageUrl" IS NOT NULL
                  AND "ImageUrl" LIKE '{NewHost}%';

                UPDATE "Posts"
                SET "ImageUrls" = (
                    SELECT COALESCE(
                        array_agg(replace(img, '{NewHost}', '{OldHostLan}')),
                        ARRAY[]::text[]
                    )
                    FROM unnest("ImageUrls") AS img
                )
                WHERE "ImageUrls" IS NOT NULL
                  AND EXISTS (
                      SELECT 1
                      FROM unnest("ImageUrls") AS img
                      WHERE img LIKE '{NewHost}%'
                  );
                """);
        }
    }
}
