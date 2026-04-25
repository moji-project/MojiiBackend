using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MojiiBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*ce bloc a été ajouté à la main pour éviter d'avoir une erreur comme quoi un utilisateur avec ce mail là existe déjà,
              donc on supprime les utilisateurs si ils existent et ce qui y est lié
            */
            migrationBuilder.Sql("""
                CREATE TEMP TABLE "_target_users" ("Id" integer) ON COMMIT DROP;
                INSERT INTO "_target_users" ("Id")
                SELECT "Id" FROM "AspNetUsers"
                WHERE "Email" IN (
                    'anis.benjemia@ynov.com',
                    'lukas.bouhlel@ynov.com',
                    'elias.eloudghiri@ynov.com',
                    'matthieu.vernier@ynov.com',
                    'hajar.zahoui@ynov.com',
                    'khadidja.khababa@ynov.com',
                    'lea.regoudis@ynov.com',
                    'hugo.laurent@ynov.com',
                    'ines.thomas@ynov.com',
                    'jules.garnier@esgi.fr'
                );

                CREATE TEMP TABLE "_target_posts" ("Id" integer) ON COMMIT DROP;
                INSERT INTO "_target_posts" ("Id")
                SELECT p."Id" FROM "Posts" p
                WHERE p."UserId" IN (SELECT "Id" FROM "_target_users");

                CREATE TEMP TABLE "_target_events" ("Id" integer) ON COMMIT DROP;
                INSERT INTO "_target_events" ("Id")
                SELECT e."Id" FROM "Events" e
                WHERE e."CreatorUserId" IN (SELECT "Id" FROM "_target_users");

                DELETE FROM "AspNetUserRoles"
                WHERE "UserId" IN (SELECT "Id" FROM "_target_users");

                DELETE FROM "PostLikes"
                WHERE "HavingLikedUsersId" IN (SELECT "Id" FROM "_target_users")
                   OR "LikedPostsId" IN (SELECT "Id" FROM "_target_posts");

                DELETE FROM "ChannelUsers"
                WHERE "UsersId" IN (SELECT "Id" FROM "_target_users");

                DELETE FROM "EventInterestedUsers"
                WHERE "InterestedUsersId" IN (SELECT "Id" FROM "_target_users")
                   OR "InterestingEventsId" IN (SELECT "Id" FROM "_target_events");

                DELETE FROM "Messages"
                WHERE "UserSenderId" IN (SELECT "Id" FROM "_target_users");

                DELETE FROM "UserStates"
                WHERE "InitiatorUserId" IN (SELECT "Id" FROM "_target_users")
                   OR "TargetedUserId" IN (SELECT "Id" FROM "_target_users");

                DELETE FROM "Reports"
                WHERE "ReporterUserId" IN (SELECT "Id" FROM "_target_users")
                   OR "TargetUserId" IN (SELECT "Id" FROM "_target_users");

                DELETE FROM "Comments"
                WHERE "UserId" IN (SELECT "Id" FROM "_target_users")
                   OR "PostId" IN (SELECT "Id" FROM "_target_posts")
                   OR "EventId" IN (SELECT "Id" FROM "_target_events");

                DELETE FROM "Posts"
                WHERE "Id" IN (SELECT "Id" FROM "_target_posts");

                DELETE FROM "Events"
                WHERE "Id" IN (SELECT "Id" FROM "_target_events");

                DELETE FROM "Notifications"
                WHERE "UserId" IN (SELECT "Id" FROM "_target_users");

                DELETE FROM "RefreshTokens"
                WHERE "UserId" IN (SELECT "Id" FROM "_target_users");

                DELETE FROM "AspNetUsers"
                WHERE "Id" IN (SELECT "Id" FROM "_target_users");
                """);
            
            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Address", "City", "CreatedAt", "Name", "PostalCode", "UpdatedAt" },
                values: new object[,]
                {
                    { 101, "24 Rue Pasteur, Lyon", "Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), "Ynov Lyon", "75001", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 102, "10 Rue de la République, Lyon", "Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), "ESGI Lyon", "69001", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Filieres",
                columns: new[] { "Id", "CreatedAt", "Intitule", "Niveau", "OrganizationId", "UpdatedAt" },
                values: new object[,]
                {
                    { 101, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), "Développement Web", "M2", 101, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 102, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), "Intelligence Artificielle", "M1", 101, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 103, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), "Cybersécurité", "M2", 102, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Biography", "ConcurrencyStamp", "Email", "EmailConfirmed", "FiliereId", "FirstName", "IsConnected", "LastConnectionDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OrganizationId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicUrl", "SecurityStamp", "Status", "TwoFactorEnabled", "UserName", "VerificationCode" },
                values: new object[,]
                {
                    { 101, 0, null, "CONCSTAMP1", "anis.benjemia@ynov.com", true, 101, "Anis", false, null, "Ben Jemia", false, null, "ANIS.BENJEMIA@YNOV.COM", "ANIS.BENJEMIA", 101, "AQAAAAIAAYagAAAAEJMYvnVYX5xc6g1z+x87i4aLP/O2sLWIgp31WRqewgUw2vGfKTOApvSzWBnLzc58Ag==", null, false, null, "SECSTAMP1", "Active", false, "anis.benjemia", null },
                    { 102, 0, null, "CONCSTAMP2", "lukas.bouhlel@ynov.com", true, 102, "Lukas", false, null, "Bouhlel", false, null, "LUKAS.BOUHLEL@YNOV.COM", "LUKAS.BOUHLEL", 101, "AQAAAAIAAYagAAAAEJMYvnVYX5xc6g1z+x87i4aLP/O2sLWIgp31WRqewgUw2vGfKTOApvSzWBnLzc58Ag==", null, false, null, "SECSTAMP2", "Active", false, "lukas.bouhlel", null },
                    { 103, 0, null, "CONCSTAMP3", "elias.eloudghiri@ynov.com", true, 101, "Elias", false, null, "El Oudghiri", false, null, "ELIAS.ELOUDGHIRI@YNOV.COM", "ELIAS.ELOUDGHIRI", 101, "AQAAAAIAAYagAAAAEJMYvnVYX5xc6g1z+x87i4aLP/O2sLWIgp31WRqewgUw2vGfKTOApvSzWBnLzc58Ag==", null, false, null, "SECSTAMP3", "Active", false, "elias.eloudghiri", null },
                    { 104, 0, null, "CONCSTAMP4", "matthieu.vernier@ynov.com", true, 102, "Matthieu", false, null, "Vernier", false, null, "MATTHIEU.VERNIER@YNOV.COM", "MATTHIEU.VERNIER", 101, "AQAAAAIAAYagAAAAEJMYvnVYX5xc6g1z+x87i4aLP/O2sLWIgp31WRqewgUw2vGfKTOApvSzWBnLzc58Ag==", null, false, null, "SECSTAMP4", "Active", false, "matthieu.vernier", null },
                    { 105, 0, null, "CONCSTAMP5", "hajar.zahoui@ynov.com", true, 101, "Hajar", false, null, "Zahoui", false, null, "HAJAR.ZAHOUI@YNOV.COM", "HAJAR.ZAHOUI", 101, "AQAAAAIAAYagAAAAEJMYvnVYX5xc6g1z+x87i4aLP/O2sLWIgp31WRqewgUw2vGfKTOApvSzWBnLzc58Ag==", null, false, null, "SECSTAMP5", "Active", false, "hajar.zahoui", null },
                    { 106, 0, null, "CONCSTAMP6", "khadidja.khababa@ynov.com", false, 102, "Khadidja", false, null, "Khababa", false, null, "KHADIDJA.KHABABA@YNOV.COM", "KHADIDJA.KHABABA", 101, null, null, false, null, null, "Active", false, "khadidja.khababa", null },
                    { 107, 0, null, "CONCSTAMP7", "lea.regoudis@ynov.com", true, 101, "Léa", false, null, "Regoudis", false, null, "LEA.REGOUDIS@YNOV.COM", "LEA.REGOUDIS", 101, "AQAAAAIAAYagAAAAEJMYvnVYX5xc6g1z+x87i4aLP/O2sLWIgp31WRqewgUw2vGfKTOApvSzWBnLzc58Ag==", null, false, null, "SECSTAMP7", "Active", false, "lea.regoudis", null },
                    { 108, 0, null, "CONCSTAMP8", "hugo.laurent@ynov.com", true, 102, "Hugo", false, null, "Laurent", false, null, "HUGO.LAURENT@YNOV.COM", "HUGO.LAURENT", 101, "AQAAAAIAAYagAAAAEJMYvnVYX5xc6g1z+x87i4aLP/O2sLWIgp31WRqewgUw2vGfKTOApvSzWBnLzc58Ag==", null, false, null, "SECSTAMP8", "Active", false, "hugo.laurent", null },
                    { 109, 0, null, "CONCSTAMP9", "ines.thomas@ynov.com", true, 101, "Ines", false, null, "Thomas", false, null, "INES.THOMAS@YNOV.COM", "INES.THOMAS", 101, "AQAAAAIAAYagAAAAEJMYvnVYX5xc6g1z+x87i4aLP/O2sLWIgp31WRqewgUw2vGfKTOApvSzWBnLzc58Ag==", null, false, null, "SECSTAMP9", "Active", false, "ines.thomas", null },
                    { 110, 0, null, "CONCSTAMP10", "jules.garnier@esgi.fr", true, 103, "Jules", false, null, "Garnier", false, null, "JULES.GARNIER@ESGI.FR", "JULES.GARNIER", 102, "AQAAAAIAAYagAAAAEJMYvnVYX5xc6g1z+x87i4aLP/O2sLWIgp31WRqewgUw2vGfKTOApvSzWBnLzc58Ag==", null, false, null, "SECSTAMP10", "Active", false, "jules.garnier", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 2, 101 },
                    { 1, 102 },
                    { 1, 103 },
                    { 1, 104 },
                    { 1, 105 },
                    { 1, 106 },
                    { 1, 107 },
                    { 1, 108 },
                    { 1, 109 },
                    { 1, 110 }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Address", "CreatedAt", "CreatorUserId", "DateLabel", "DayLabel", "DefaultInterestedCount", "Description", "ImageUrl", "IsPublished", "Location", "MonthLabel", "Name", "OrganizationId", "StartDate", "UpdatedAt" },
                values: new object[,]
                {
                    { 101, "24 Rue Pasteur, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Lundi 15 Mars 2027 - 09h00", "15", 0, "Hackathon annuel Ynov Lyon", null, true, "Ynov Lyon", "MARS", "Hackathon Ynov 2025", 101, new DateTime(2027, 3, 15, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 102, "24 Rue Pasteur, Bâtiment A, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Samedi 10 Avril 2027 - 14h00", "10", 0, "Conférence sur l'IA et ses enjeux éthiques", null, true, "Amphi A", "AVR", "Conférence IA & Éthique", 101, new DateTime(2027, 4, 10, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 103, "24 Rue Pasteur, Espace Coworking, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Jeudi 20 Mai 2027 - 18h00", "20", 0, "Soirée networking pour les développeurs", null, true, "Espace Collaboratif", "MAI", "Soirée Networking Dev", 101, new DateTime(2027, 5, 20, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 104, "24 Rue Pasteur, Salle 3.12, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Samedi 5 Juin 2027 - 10h00", "5", 0, "Workshop pratique Docker et intégration continue", null, false, "Salle Informatique 3", "JUIN", "Workshop Docker & CI/CD", 101, new DateTime(2027, 6, 5, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 105, "24 Rue Pasteur, Hall principal, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Vendredi 25 Juin 2027 - 09h00", "25", 0, "Forum annuel de recrutement", null, true, "Hall Principal", "JUIN", "Forum des entreprises", 101, new DateTime(2027, 6, 25, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 106, "24 Rue Pasteur, Pôle Carrières, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Dimanche 12 Septembre 2027 - 13h30", "12", 0, "Atelier pratique pour optimiser CV et LinkedIn", null, true, "Salle Carrières", "SEPT", "Atelier CV & LinkedIn", 101, new DateTime(2027, 9, 12, 13, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 107, "24 Rue Pasteur, Campus Ynov Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Dimanche 3 Octobre 2027 - 17h30", "3", 0, "Découverte des filières tech et des projets", null, true, "Campus Ynov", "OCT", "Journée Portes Ouvertes Tech", 101, new DateTime(2027, 10, 3, 17, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 108, "10 Rue de la République, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 110, "Samedi 10 Juillet 2027 - 09h00", "10", 0, "Capture The Flag cybersécurité", null, true, "ESGI Lyon", "JUIL", "CTF Cybersécurité ESGI", 102, new DateTime(2027, 7, 10, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 109, "24 Rue Pasteur, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Jeudi 15 Avril 2027 - 09h00", "15", 0, "Échanges sur les nouveautés Flutter.", null, true, "Labo Mobile", "AVR", "Meetup Flutter & Dart", 101, new DateTime(2027, 4, 15, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 110, "24 Rue Pasteur, Bâtiment B, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Lundi 10 Mai 2027 - 14h00", "10", 0, "Concevoir des interfaces centrées utilisateur.", null, true, "Studio Créa", "MAI", "Masterclass UI/UX Design", 101, new DateTime(2027, 5, 10, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 111, "24 Rue Pasteur, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Dimanche 20 Juin 2027 - 18h00", "20", 0, "Compétition de jeux vidéo inter-spécialités.", null, true, "Foyer Étudiant", "JUIN", "Tournoi E-sport Inter-Filière", 101, new DateTime(2027, 6, 20, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 112, "24 Rue Pasteur, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Lundi 5 Juillet 2027 - 10h00", "5", 0, "Semaine intensive pour booster vos compétences.", null, true, "Espace Coworking", "JUIL", "Summer Coding Camp", 101, new DateTime(2027, 7, 5, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 113, "24 Rue Pasteur, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Dimanche 25 Juillet 2027 - 09h00", "25", 0, "Moment convivial avant les vacances d'été.", null, true, "Toit-Terrasse", "JUIL", "Barbecue de fin d'année", 101, new DateTime(2027, 7, 25, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 114, "24 Rue Pasteur, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Mardi 12 Octobre 2027 - 13h30", "12", 0, "Conférences sur les nouvelles menaces web.", null, true, "Amphi Principal", "OCT", "Séminaire Cybersécurité", 101, new DateTime(2027, 10, 12, 13, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 115, "Cité Internationale, Lyon", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Mercredi 3 Novembre 2027 - 17h30", "3", 0, "Cérémonie officielle et soirée de prestige.", null, true, "Palais des Congrès", "NOV", "Gala de Remise des Diplômes", 101, new DateTime(2027, 11, 3, 17, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "Content", "CreatedAt", "IsRead", "Type", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 101, "Le Hackathon Ynov 2025 a été publié !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), false, "NewEventCreated", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101 },
                    { 102, "Quelqu'un a commenté votre post.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), false, "NewCommentOnPost", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 102 },
                    { 103, "Votre post a reçu un like.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), true, "NewLikeOnPost", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 103 },
                    { 104, "Un nouvel événement est disponible.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), false, "NewEventCreated", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 104 },
                    { 105, "Votre compte a été activé.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), true, "AccountActivated", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 105 },
                    { 106, "Vous avez reçu un nouveau message.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), false, "NewMessage", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 106 },
                    { 107, "L'événement Workshop Docker a été mis à jour.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), false, "EventUpdated", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 107 },
                    { 108, "Un de vos posts a été signalé.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), true, "PostReported", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 108 },
                    { 109, "Votre post a reçu un like.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), false, "NewLikeOnPost", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 109 },
                    { 110, "Le CTF Cybersécurité ESGI est publié !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), false, "NewEventCreated", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 110 }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedAt", "ImageUrls", "NbOfLikes", "NbOfReports", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 101, "Voici mon premier projet React en B3 Dev Web !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new string[0], 0, 0, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101 },
                    { 102, "Quelqu'un a des ressources pour apprendre PyTorch ?", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new string[0], 0, 0, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 102 },
                    { 103, "Le cours d'algorithmique était intense aujourd'hui.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new string[0], 0, 0, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 103 },
                    { 104, "Tips pour le projet de machine learning ?", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new string[0], 0, 0, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 104 },
                    { 105, "Qui veut faire du co-working ce weekend ?", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new string[0], 0, 0, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 105 },
                    { 106, "Je viens de finir mon stage, c'était incroyable !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new string[0], 0, 0, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 106 },
                    { 107, "Des nouvelles du Hackathon Ynov Lyon ?", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new string[0], 0, 0, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 107 },
                    { 108, "Retour d'expérience sur Docker et Kubernetes.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new string[0], 0, 0, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 108 },
                    { 109, "Bonne chance à tous pour les partiels !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), new string[0], 0, 0, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 109 }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "Comment", "CreatedAt", "Reason", "ReporterUserId", "Status", "TargetCommentId", "TargetPostId", "TargetUserId", "UpdatedAt" },
                values: new object[,]
                {
                    { 101, "Comportement inapproprié", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), "Harassment", 101, "Open", null, null, 103, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 102, "Envoi de messages répétitifs", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), "Spam", 102, "Reviewed", null, null, 104, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "UserStates",
                columns: new[] { "Id", "CreatedAt", "InitiatorUserId", "StateType", "TargetedUserId", "UpdatedAt" },
                values: new object[,]
                {
                    { 101, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, "Blocked", 103, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 102, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 102, "Muted", 104, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 103, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 103, "Blocked", 105, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 104, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 104, "Muted", 106, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 105, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 105, "Blocked", 107, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreatedAt", "EventId", "NbOfLikes", "PostId", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 101, "Super projet !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), null, 0, 101, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 102 },
                    { 102, "Bravo, continue comme ça !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), null, 0, 101, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 103 },
                    { 103, "J'ai les mêmes ressources, je t'envoie ça !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), null, 0, 102, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101 },
                    { 104, "Regarde la doc officielle de PyTorch.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), null, 0, 102, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 104 },
                    { 105, "Moi aussi, c'était vraiment intense !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), null, 0, 103, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 105 },
                    { 106, "Le prof est excellent.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), null, 0, 103, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 106 },
                    { 107, "Essaie scikit-learn en premier.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), null, 0, 104, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 107 },
                    { 108, "On peut travailler ensemble si tu veux.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), null, 0, 104, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 108 },
                    { 109, "Je suis dispo samedi !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), null, 0, 105, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 109 },
                    { 110, "Bonne idée, je suis partant.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), null, 0, 105, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101 },
                    { 111, "Hâte d'y participer !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, 0, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 103 },
                    { 112, "On forme une équipe ?", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 101, 0, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 104 },
                    { 113, "Très bon sujet, merci pour l'organisation.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 102, 0, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 105 },
                    { 114, "Est-ce qu'il y a un replay prévu ?", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 102, 0, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 106 },
                    { 115, "Je serai là !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 103, 0, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 107 },
                    { 116, "Super initiative !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 103, 0, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 108 },
                    { 117, "Workshop très utile pour les projets.", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 104, 0, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 109 },
                    { 118, "Quelle version de Docker sera utilisée ?", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 104, 0, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 102 },
                    { 119, "Des entreprises tech seront présentes ?", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 105, 0, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 103 },
                    { 120, "J'ai envoyé mon CV à 3 entreprises !", new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 105, 0, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), 104 }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "Comment", "CreatedAt", "Reason", "ReporterUserId", "Status", "TargetCommentId", "TargetPostId", "TargetUserId", "UpdatedAt" },
                values: new object[,]
                {
                    { 103, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), "InapropriateContent", 103, "Open", null, 102, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 104, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), "FakeInformation", 104, "Closed", null, 105, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 105, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc), "HateSpeech", 105, "Open", 101, null, null, new DateTime(2026, 4, 20, 16, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 101 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 102 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 103 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 104 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 105 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 106 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 107 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 108 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 109 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 110 });

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "UserStates",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "UserStates",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "UserStates",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "UserStates",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "UserStates",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Filieres",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Filieres",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Filieres",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: 101);
        }
    }
}
