using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MojiiBackend.Application.Shared;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Domain.Enums;

namespace MojiiBackend.Infrastructure.Database;

public class AppDbContext: IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Message> Messages { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Channel> Channels { get; set; }
    public DbSet<UserState> UserStates { get; set; }
    public DbSet<Filiere> Filieres { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Post>()
            .HasOne(p => p.User)           
            .WithMany(u => u.CreatedPosts)  
            .HasForeignKey(p => p.UserId)   
            .OnDelete(DeleteBehavior.Restrict); // Important : Empêcher la suppression
        
        modelBuilder.Entity<UserState>()
            .HasOne(us => us.InitiatorUser)
            .WithMany(u => u.UserStates) // On lie la liste définie dans User à l'initiateur
            .HasForeignKey(us => us.InitiatorUserId)
            .OnDelete(DeleteBehavior.Restrict); // Important : Pas de cascade pour éviter les cycles

        modelBuilder.Entity<UserState>()
            .HasOne(us => us.TargetedUser)
            .WithMany() // Pas de liste spécifique "TargetedBy" dans User, donc on laisse vide
            .HasForeignKey(us => us.TargetedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        ConfigureManyToManyRelationships(modelBuilder);

        ConfigureDeleteBehavior(modelBuilder);

        SeedData(modelBuilder);
    }

    private void ConfigureManyToManyRelationships(ModelBuilder modelBuilder)
    {
        // User <-> Post (Likes)
        modelBuilder.Entity<User>()
            .HasMany(u => u.LikedPosts)
            .WithMany(p => p.HavingLikedUsers)
            .UsingEntity(j => j.ToTable("PostLikes")); // Table de liaison nommée proprement

        // User <-> Channel (Members)
        modelBuilder.Entity<Channel>()
            .HasMany(c => c.Users)
            .WithMany(u => u.Channels)
            .UsingEntity(j => j.ToTable("ChannelUsers"));
        
        // User <-> Event (Interested)
        modelBuilder.Entity<Event>()
            .HasMany(e => e.InterestedUsers)
            .WithMany(u => u.InterestingEvents)
            .UsingEntity(j => j.ToTable("EventInterestedUsers"));
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>() 
            .HaveConversion<string>() 
            .HaveMaxLength(50);
    }

    private void ConfigureDeleteBehavior(ModelBuilder modelBuilder)
    {
        // --- ORGANIZATION & FILIERE --- //
        // Si on supprime une Filiere, les Users liés ne doivent PAS être supprimés (ils deviennent orphelins ou erreur)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Filiere)
            .WithMany(f => f.Users)
            .HasForeignKey(u => u.FiliereId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // --- EVENT --- //
        // Si on supprime une organization, les événements liés sont supprimés
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Organization)
            .WithMany(o => o.Events)
            .HasForeignKey(e => e.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Si on supprime un User school admin, les évènements créés ne sont pas supprimés
        modelBuilder.Entity<Event>()
            .HasOne(e => e.CreatorUser)
            .WithMany()
            .HasForeignKey(e => e.CreatorUserId)
            .OnDelete(DeleteBehavior.Restrict); 
        
        // --- NOTIFICATION --- //
        //Si on supprime un User, ses notifs sont supprimées avec
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade); 
        
        // --- REPORTS --- //
        modelBuilder.Entity<Report>()
            .HasOne(r => r.ReporterUser)
            .WithMany() 
            .HasForeignKey(r => r.ReporterUserId)
            .OnDelete(DeleteBehavior.Restrict); // On ne supprime pas le report si l'user part
        
        // Report → TargetUser
        modelBuilder.Entity<Report>()
            .HasOne(r => r.TargetUser)
            .WithMany()  // No navigation back from User (optional)
            .HasForeignKey(r => r.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade); // If user is deleted, delete their reports

        // Report → TargetPost
        modelBuilder.Entity<Report>()
            .HasOne(r => r.TargetPost)
            .WithMany()
            .HasForeignKey(r => r.TargetPostId)
            .OnDelete(DeleteBehavior.Cascade); // If post is deleted, delete its reports

        // Report → TargetComment
        modelBuilder.Entity<Report>()
            .HasOne(r => r.TargetComment)
            .WithMany()
            .HasForeignKey(r => r.TargetCommentId)
            .OnDelete(DeleteBehavior.Cascade); // If comment is deleted, delete its reports
        
        // --- MESSAGERIE --- //
        // Si on supprime un Channel, on supprime les messages (Cascade par défaut, mais explicite c'est mieux)
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Channel)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Si on supprime un User, on garde ses messages pour l'historique du chat (Restrict)
        modelBuilder.Entity<Message>()
            .HasOne(m => m.UserSender)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.UserSenderId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- POSTS & COMMENTAIRES ---
        // Si on supprime un Post, on supprime ses commentaires (Cascade)
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Si on supprime un User, on peut supprimer ses commentaires OU les garder (anonymisés). 
        // Ici, pour éviter les cycles avec Post, on met souvent Restrict ou NoAction.
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Si on supprime un User, les RefreshTokens associés sont supprimés (Cascade)
        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);  
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            // On s'assure que la date est bien en UTC (crucial pour PostgreSQL)
            var now = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Entity.CreatedAt = now;
            }
        
            entityEntry.Entity.UpdatedAt = now;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

private void SeedData(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2026, 4, 20, 16, 0, 0, DateTimeKind.Utc);
        var frCulture = new CultureInfo("fr-FR");
        // All seeded users share the same hash for plain text password: --> Password1 <--
        const string defaultPasswordHash = "AQAAAAIAAYagAAAAEJMYvnVYX5xc6g1z+x87i4aLP/O2sLWIgp31WRqewgUw2vGfKTOApvSzWBnLzc58Ag==";
        // All users have EmailConfirmed bool set to true, except Khadidja KHABABA (who also doesnt have a password yet)

        string BuildDayLabel(DateTime date) => date.Day.ToString();
        string BuildMonthLabel(DateTime date) => date.ToString("MMM", frCulture).ToUpper().Replace(".", "");
        string BuildDateLabel(DateTime date)
        {
            var dateString = date.ToString("dddd d MMMM yyyy", frCulture);
            return $"{frCulture.TextInfo.ToTitleCase(dateString)} - {date:HH:mm}".Replace(":", "h");
        }

        // --- ROLES ---
        modelBuilder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int> { Id = 1, Name = AppRoles.Student,     NormalizedName = AppRoles.Student.ToUpperInvariant(),     ConcurrencyStamp = "STUDENT_CONCURRENCY_STAMP" },
            new IdentityRole<int> { Id = 2, Name = AppRoles.SchoolAdmin, NormalizedName = AppRoles.SchoolAdmin.ToUpperInvariant(), ConcurrencyStamp = "SCHOOL_ADMIN_CONCURRENCY_STAMP" },
            new IdentityRole<int> { Id = 3, Name = AppRoles.SuperAdmin,  NormalizedName = AppRoles.SuperAdmin.ToUpperInvariant(),  ConcurrencyStamp = "SUPER_ADMIN_CONCURRENCY_STAMP" }
        );

        // --- ORGANIZATIONS (2) ---
        modelBuilder.Entity<Organization>().HasData(
            new Organization { Id = 101, Name = "Ynov Lyon", City = "Lyon", PostalCode = "75001", Address = "24 Rue Pasteur, Lyon",                 CreatedAt = seedDate, UpdatedAt = seedDate },
            new Organization { Id = 102, Name = "ESGI Lyon",     City = "Lyon",  PostalCode = "69001", Address = "10 Rue de la République, Lyon", CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        // --- FILIERES (3: 2 for org 1, 1 for org 2) ---
        modelBuilder.Entity<Filiere>().HasData(
            new Filiere { Id = 101, Intitule = "Développement Web",        Niveau = "M2", OrganizationId = 101, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Filiere { Id = 102, Intitule = "Intelligence Artificielle", Niveau = "M1", OrganizationId = 101, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Filiere { Id = 103, Intitule = "Cybersécurité",             Niveau = "M2", OrganizationId = 102, CreatedAt = seedDate, UpdatedAt = seedDate }
        );
        

        // --- USERS (10: users 1-9 → org 1 with filiere 1 or 2, user 10 → org 2 filiere 3) ---
        modelBuilder.Entity<User>().HasData(
            new User { Id = 101, FirstName = "Anis",    LastName = "Ben Jemia",    UserName = "anis.benjemia",    NormalizedUserName = "ANIS.BENJEMIA",    Email = "anis.benjemia@ynov.com",    NormalizedEmail = "ANIS.BENJEMIA@YNOV.COM",    EmailConfirmed = true, PasswordHash = defaultPasswordHash, SecurityStamp = "SECSTAMP1",  ConcurrencyStamp = "CONCSTAMP1",  OrganizationId = 101, FiliereId = 101, Status = UserStatus.Active },
            new User { Id = 102, FirstName = "Lukas",   LastName = "Bouhlel",      UserName = "lukas.bouhlel",    NormalizedUserName = "LUKAS.BOUHLEL",    Email = "lukas.bouhlel@ynov.com",    NormalizedEmail = "LUKAS.BOUHLEL@YNOV.COM",    EmailConfirmed = true, PasswordHash = defaultPasswordHash, SecurityStamp = "SECSTAMP2",  ConcurrencyStamp = "CONCSTAMP2",  OrganizationId = 101, FiliereId = 102, Status = UserStatus.Active },
            new User { Id = 103, FirstName = "Elias",   LastName = "El Oudghiri",  UserName = "elias.eloudghiri", NormalizedUserName = "ELIAS.ELOUDGHIRI", Email = "elias.eloudghiri@ynov.com", NormalizedEmail = "ELIAS.ELOUDGHIRI@YNOV.COM", EmailConfirmed = true, PasswordHash = defaultPasswordHash, SecurityStamp = "SECSTAMP3",  ConcurrencyStamp = "CONCSTAMP3",  OrganizationId = 101, FiliereId = 101, Status = UserStatus.Active },
            new User { Id = 104, FirstName = "Matthieu",LastName = "Vernier",      UserName = "matthieu.vernier",  NormalizedUserName = "MATTHIEU.VERNIER",  Email = "matthieu.vernier@ynov.com",  NormalizedEmail = "MATTHIEU.VERNIER@YNOV.COM",  EmailConfirmed = true, PasswordHash = defaultPasswordHash, SecurityStamp = "SECSTAMP4",  ConcurrencyStamp = "CONCSTAMP4",  OrganizationId = 101, FiliereId = 102, Status = UserStatus.Active },
            new User { Id = 105, FirstName = "Hajar",   LastName = "Zahoui",       UserName = "hajar.zahoui",     NormalizedUserName = "HAJAR.ZAHOUI",     Email = "hajar.zahoui@ynov.com",     NormalizedEmail = "HAJAR.ZAHOUI@YNOV.COM",     EmailConfirmed = true, PasswordHash = defaultPasswordHash, SecurityStamp = "SECSTAMP5",  ConcurrencyStamp = "CONCSTAMP5",  OrganizationId = 101, FiliereId = 101, Status = UserStatus.Active },
            new User { Id = 106, FirstName = "Khadidja",LastName = "Khababa",      UserName = "khadidja.khababa",  NormalizedUserName = "KHADIDJA.KHABABA",  Email = "khadidja.khababa@ynov.com",  NormalizedEmail = "KHADIDJA.KHABABA@YNOV.COM",  EmailConfirmed = false, ConcurrencyStamp = "CONCSTAMP6",  OrganizationId = 101, FiliereId = 102, Status = UserStatus.Active },
            new User { Id = 107, FirstName = "Léa",    LastName = "Regoudis",     UserName = "lea.regoudis",     NormalizedUserName = "LEA.REGOUDIS",     Email = "lea.regoudis@ynov.com",     NormalizedEmail = "LEA.REGOUDIS@YNOV.COM",     EmailConfirmed = true, PasswordHash = defaultPasswordHash, SecurityStamp = "SECSTAMP7",  ConcurrencyStamp = "CONCSTAMP7",  OrganizationId = 101, FiliereId = 101, Status = UserStatus.Active },
            new User { Id = 108, FirstName = "Hugo",   LastName = "Laurent",  UserName = "hugo.laurent",   NormalizedUserName = "HUGO.LAURENT",   Email = "hugo.laurent@ynov.com",   NormalizedEmail = "HUGO.LAURENT@YNOV.COM",   EmailConfirmed = true, PasswordHash = defaultPasswordHash, SecurityStamp = "SECSTAMP8",  ConcurrencyStamp = "CONCSTAMP8",  OrganizationId = 101, FiliereId = 102, Status = UserStatus.Active },
            new User { Id = 109, FirstName = "Ines",   LastName = "Thomas",   UserName = "ines.thomas",    NormalizedUserName = "INES.THOMAS",    Email = "ines.thomas@ynov.com",    NormalizedEmail = "INES.THOMAS@YNOV.COM",    EmailConfirmed = true, PasswordHash = defaultPasswordHash, SecurityStamp = "SECSTAMP9",  ConcurrencyStamp = "CONCSTAMP9",  OrganizationId = 101, FiliereId = 101, Status = UserStatus.Active },
            new User { Id = 110, FirstName = "Jules",  LastName = "Garnier",  UserName = "jules.garnier",  NormalizedUserName = "JULES.GARNIER",  Email = "jules.garnier@esgi.fr",     NormalizedEmail = "JULES.GARNIER@ESGI.FR",     EmailConfirmed = true, PasswordHash = defaultPasswordHash, SecurityStamp = "SECSTAMP10", ConcurrencyStamp = "CONCSTAMP10", OrganizationId = 102, FiliereId = 103, Status = UserStatus.Active }
        );

        // --- USER ROLES (user 1 = SchoolAdmin, rest = Student) ---
        modelBuilder.Entity<IdentityUserRole<int>>().HasData(
            new IdentityUserRole<int> { UserId = 101, RoleId = 2 },
            new IdentityUserRole<int> { UserId = 102, RoleId = 1 },
            new IdentityUserRole<int> { UserId = 103, RoleId = 1 },
            new IdentityUserRole<int> { UserId = 104, RoleId = 1 },
            new IdentityUserRole<int> { UserId = 105, RoleId = 1 },
            new IdentityUserRole<int> { UserId = 106, RoleId = 1 },
            new IdentityUserRole<int> { UserId = 107, RoleId = 1 },
            new IdentityUserRole<int> { UserId = 108, RoleId = 1 },
            new IdentityUserRole<int> { UserId = 109, RoleId = 1 },
            new IdentityUserRole<int> { UserId = 110, RoleId = 1 }
        );

        // --- POSTS (9: one per user 1–9, all in org 1) ---
        modelBuilder.Entity<Post>().HasData(
            new Post { Id = 101, Content = "Voici mon premier projet React en B3 Dev Web !",        UserId = 101, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Post { Id = 102, Content = "Quelqu'un a des ressources pour apprendre PyTorch ?",   UserId = 102, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Post { Id = 103, Content = "Le cours d'algorithmique était intense aujourd'hui.",    UserId = 103, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Post { Id = 104, Content = "Tips pour le projet de machine learning ?",              UserId = 104, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Post { Id = 105, Content = "Qui veut faire du co-working ce weekend ?",             UserId = 105, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Post { Id = 106, Content = "Je viens de finir mon stage, c'était incroyable !",     UserId = 106, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Post { Id = 107, Content = "Des nouvelles du Hackathon Ynov Lyon ?",             UserId = 107, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Post { Id = 108, Content = "Retour d'expérience sur Docker et Kubernetes.",          UserId = 108, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Post { Id = 109, Content = "Bonne chance à tous pour les partiels !",               UserId = 109, CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        var eventStart101 = new DateTime(2027, 3, 15, 9, 0, 0, DateTimeKind.Utc);
        var eventStart102 = new DateTime(2027, 4, 10, 14, 0, 0, DateTimeKind.Utc);
        var eventStart103 = new DateTime(2027, 5, 20, 18, 0, 0, DateTimeKind.Utc);
        var eventStart104 = new DateTime(2027, 6, 5, 10, 0, 0, DateTimeKind.Utc);
        var eventStart105 = new DateTime(2027, 6, 25, 9, 0, 0, DateTimeKind.Utc);
        var eventStart106 = new DateTime(2027, 9, 12, 13, 30, 0, DateTimeKind.Utc);
        var eventStart107 = new DateTime(2027, 10, 3, 17, 30, 0, DateTimeKind.Utc);
        var eventStart108 = new DateTime(2027, 7, 10, 9, 0, 0, DateTimeKind.Utc);

        // --- EVENTS (8: 7 for org 1, 1 for org 2) ---
        modelBuilder.Entity<Event>().HasData(
            new Event { Id = 101, Name = "Hackathon Ynov 2025",         StartDate = eventStart101, Location = "Ynov Lyon",            Address = "24 Rue Pasteur, Lyon",                        Description = "Hackathon annuel Ynov Lyon",                      IsPublished = true,  OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart101), MonthLabel = BuildMonthLabel(eventStart101), DateLabel = BuildDateLabel(eventStart101), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Event { Id = 102, Name = "Conférence IA & Éthique",      StartDate = eventStart102, Location = "Amphi A",              Address = "24 Rue Pasteur, Bâtiment A, Lyon",            Description = "Conférence sur l'IA et ses enjeux éthiques",      IsPublished = true,  OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart102), MonthLabel = BuildMonthLabel(eventStart102), DateLabel = BuildDateLabel(eventStart102), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Event { Id = 103, Name = "Soirée Networking Dev",        StartDate = eventStart103, Location = "Espace Collaboratif", Address = "24 Rue Pasteur, Espace Coworking, Lyon",      Description = "Soirée networking pour les développeurs",         IsPublished = true,  OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart103), MonthLabel = BuildMonthLabel(eventStart103), DateLabel = BuildDateLabel(eventStart103), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Event { Id = 104, Name = "Workshop Docker & CI/CD",      StartDate = eventStart104, Location = "Salle Informatique 3",Address = "24 Rue Pasteur, Salle 3.12, Lyon",            Description = "Workshop pratique Docker et intégration continue", IsPublished = false, OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart104), MonthLabel = BuildMonthLabel(eventStart104), DateLabel = BuildDateLabel(eventStart104), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Event { Id = 105, Name = "Forum des entreprises",        StartDate = eventStart105, Location = "Hall Principal",       Address = "24 Rue Pasteur, Hall principal, Lyon",        Description = "Forum annuel de recrutement",                     IsPublished = true,  OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart105), MonthLabel = BuildMonthLabel(eventStart105), DateLabel = BuildDateLabel(eventStart105), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Event { Id = 106, Name = "Atelier CV & LinkedIn",        StartDate = eventStart106, Location = "Salle Carrières",      Address = "24 Rue Pasteur, Pôle Carrières, Lyon",        Description = "Atelier pratique pour optimiser CV et LinkedIn",  IsPublished = true,  OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart106), MonthLabel = BuildMonthLabel(eventStart106), DateLabel = BuildDateLabel(eventStart106), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Event { Id = 107, Name = "Journée Portes Ouvertes Tech", StartDate = eventStart107, Location = "Campus Ynov",          Address = "24 Rue Pasteur, Campus Ynov Lyon",            Description = "Découverte des filières tech et des projets",     IsPublished = true,  OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart107), MonthLabel = BuildMonthLabel(eventStart107), DateLabel = BuildDateLabel(eventStart107), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Event { Id = 108, Name = "CTF Cybersécurité ESGI",       StartDate = eventStart108, Location = "ESGI Lyon",            Address = "10 Rue de la République, Lyon",               Description = "Capture The Flag cybersécurité",                  IsPublished = true,  OrganizationId = 102, CreatorUserId = 110, DayLabel = BuildDayLabel(eventStart108), MonthLabel = BuildMonthLabel(eventStart108), DateLabel = BuildDateLabel(eventStart108), CreatedAt = seedDate, UpdatedAt = seedDate }
        );
        // 7 events de plus pour l'org n°1
        modelBuilder.Entity<Event>().HasData(
            new Event { Id = 109, Name = "Meetup Flutter & Dart",         StartDate = eventStart101.AddMonths(1), Location = "Labo Mobile",        Address = "24 Rue Pasteur, Lyon",             Description = "Échanges sur les nouveautés Flutter.",           IsPublished = true,  OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart101.AddMonths(1)), MonthLabel = BuildMonthLabel(eventStart101.AddMonths(1)), DateLabel = BuildDateLabel(eventStart101.AddMonths(1)), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Event { Id = 110, Name = "Masterclass UI/UX Design",     StartDate = eventStart102.AddMonths(1), Location = "Studio Créa",        Address = "24 Rue Pasteur, Bâtiment B, Lyon",  Description = "Concevoir des interfaces centrées utilisateur.", IsPublished = true,  OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart102.AddMonths(1)), MonthLabel = BuildMonthLabel(eventStart102.AddMonths(1)), DateLabel = BuildDateLabel(eventStart102.AddMonths(1)), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Event { Id = 111, Name = "Tournoi E-sport Inter-Filière", StartDate = eventStart103.AddMonths(1), Location = "Foyer Étudiant",     Address = "24 Rue Pasteur, Lyon",             Description = "Compétition de jeux vidéo inter-spécialités.",   IsPublished = true,  OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart103.AddMonths(1)), MonthLabel = BuildMonthLabel(eventStart103.AddMonths(1)), DateLabel = BuildDateLabel(eventStart103.AddMonths(1)), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Event { Id = 112, Name = "Summer Coding Camp",           StartDate = eventStart104.AddMonths(1), Location = "Espace Coworking",   Address = "24 Rue Pasteur, Lyon",             Description = "Semaine intensive pour booster vos compétences.",IsPublished = true,  OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart104.AddMonths(1)), MonthLabel = BuildMonthLabel(eventStart104.AddMonths(1)), DateLabel = BuildDateLabel(eventStart104.AddMonths(1)), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Event { Id = 113, Name = "Barbecue de fin d'année",       StartDate = eventStart105.AddMonths(1), Location = "Toit-Terrasse",      Address = "24 Rue Pasteur, Lyon",             Description = "Moment convivial avant les vacances d'été.",     IsPublished = true,  OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart105.AddMonths(1)), MonthLabel = BuildMonthLabel(eventStart105.AddMonths(1)), DateLabel = BuildDateLabel(eventStart105.AddMonths(1)), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Event { Id = 114, Name = "Séminaire Cybersécurité",       StartDate = eventStart106.AddMonths(1), Location = "Amphi Principal",    Address = "24 Rue Pasteur, Lyon",             Description = "Conférences sur les nouvelles menaces web.",     IsPublished = true,  OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart106.AddMonths(1)), MonthLabel = BuildMonthLabel(eventStart106.AddMonths(1)), DateLabel = BuildDateLabel(eventStart106.AddMonths(1)), CreatedAt = seedDate, UpdatedAt = seedDate },
            new Event { Id = 115, Name = "Gala de Remise des Diplômes",  StartDate = eventStart107.AddMonths(1), Location = "Palais des Congrès", Address = "Cité Internationale, Lyon",         Description = "Cérémonie officielle et soirée de prestige.",    IsPublished = true,  OrganizationId = 101, CreatorUserId = 101, DayLabel = BuildDayLabel(eventStart107.AddMonths(1)), MonthLabel = BuildMonthLabel(eventStart107.AddMonths(1)), DateLabel = BuildDateLabel(eventStart107.AddMonths(1)), CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        // --- COMMENTS (20: 2 per posts 1–5, 2 per events 1–5, by random users) ---
        modelBuilder.Entity<Comment>().HasData(
            new Comment { Id = 101, Content = "Super projet !",                              UserId = 102, PostId = 101, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 102, Content = "Bravo, continue comme ça !",                 UserId = 103, PostId = 101, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 103, Content = "J'ai les mêmes ressources, je t'envoie ça !", UserId = 101, PostId = 102, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 104, Content = "Regarde la doc officielle de PyTorch.",       UserId = 104, PostId = 102, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 105, Content = "Moi aussi, c'était vraiment intense !",       UserId = 105, PostId = 103, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 106, Content = "Le prof est excellent.",                       UserId = 106, PostId = 103, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 107, Content = "Essaie scikit-learn en premier.",              UserId = 107, PostId = 104, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 108, Content = "On peut travailler ensemble si tu veux.",      UserId = 108, PostId = 104, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 109, Content = "Je suis dispo samedi !",                       UserId = 109, PostId = 105, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 110, Content = "Bonne idée, je suis partant.",                 UserId = 101, PostId = 105, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 111, Content = "Hâte d'y participer !",                        UserId = 103, EventId = 101, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 112, Content = "On forme une équipe ?",                        UserId = 104, EventId = 101, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 113, Content = "Très bon sujet, merci pour l'organisation.",   UserId = 105, EventId = 102, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 114, Content = "Est-ce qu'il y a un replay prévu ?",           UserId = 106, EventId = 102, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 115, Content = "Je serai là !",                                UserId = 107, EventId = 103, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 116, Content = "Super initiative !",                            UserId = 108, EventId = 103, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 117, Content = "Workshop très utile pour les projets.",         UserId = 109, EventId = 104, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 118, Content = "Quelle version de Docker sera utilisée ?",      UserId = 102, EventId = 104, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 119, Content = "Des entreprises tech seront présentes ?",       UserId = 103, EventId = 105, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Comment { Id = 120, Content = "J'ai envoyé mon CV à 3 entreprises !",          UserId = 104, EventId = 105, CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        // --- REPORTS (5: 2 targeting users, 2 targeting posts, 1 targeting a comment) ---
        modelBuilder.Entity<Report>().HasData(
            new Report { Id = 101, ReporterUserId = 101, TargetUserId    = 103, Reason = ReportReason.Harassment,        Status = ReportStatus.Open,     Comment = "Comportement inapproprié",       CreatedAt = seedDate, UpdatedAt = seedDate },
            new Report { Id = 102, ReporterUserId = 102, TargetUserId    = 104, Reason = ReportReason.Spam,              Status = ReportStatus.Reviewed, Comment = "Envoi de messages répétitifs",   CreatedAt = seedDate, UpdatedAt = seedDate },
            new Report { Id = 103, ReporterUserId = 103, TargetPostId    = 102, Reason = ReportReason.InapropriateContent, Status = ReportStatus.Open,   CreatedAt = seedDate, UpdatedAt = seedDate },
            new Report { Id = 104, ReporterUserId = 104, TargetPostId    = 105, Reason = ReportReason.FakeInformation,   Status = ReportStatus.Closed,   CreatedAt = seedDate, UpdatedAt = seedDate },
            new Report { Id = 105, ReporterUserId = 105, TargetCommentId = 101, Reason = ReportReason.HateSpeech,        Status = ReportStatus.Open,     CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        // --- USER STATES (5 between users of org 1) ---
        modelBuilder.Entity<UserState>().HasData(
            new UserState { Id = 101, InitiatorUserId = 101, TargetedUserId = 103, StateType = UserStateType.Blocked, CreatedAt = seedDate, UpdatedAt = seedDate },
            new UserState { Id = 102, InitiatorUserId = 102, TargetedUserId = 104, StateType = UserStateType.Muted,   CreatedAt = seedDate, UpdatedAt = seedDate },
            new UserState { Id = 103, InitiatorUserId = 103, TargetedUserId = 105, StateType = UserStateType.Blocked, CreatedAt = seedDate, UpdatedAt = seedDate },
            new UserState { Id = 104, InitiatorUserId = 104, TargetedUserId = 106, StateType = UserStateType.Muted,   CreatedAt = seedDate, UpdatedAt = seedDate },
            new UserState { Id = 105, InitiatorUserId = 105, TargetedUserId = 107, StateType = UserStateType.Blocked, CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        // --- NOTIFICATIONS (10: one per user) ---
        modelBuilder.Entity<Notification>().HasData(
            new Notification { Id = 101, UserId = 101, Type = NotificationType.NewEventCreated,   Content = "Le Hackathon Ynov 2025 a été publié !",      IsRead = false, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Notification { Id = 102, UserId = 102, Type = NotificationType.NewCommentOnPost,  Content = "Quelqu'un a commenté votre post.",               IsRead = false, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Notification { Id = 103, UserId = 103, Type = NotificationType.NewLikeOnPost,     Content = "Votre post a reçu un like.",                     IsRead = true,  CreatedAt = seedDate, UpdatedAt = seedDate },
            new Notification { Id = 104, UserId = 104, Type = NotificationType.NewEventCreated,   Content = "Un nouvel événement est disponible.",            IsRead = false, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Notification { Id = 105, UserId = 105, Type = NotificationType.AccountActivated,  Content = "Votre compte a été activé.",                     IsRead = true,  CreatedAt = seedDate, UpdatedAt = seedDate },
            new Notification { Id = 106, UserId = 106, Type = NotificationType.NewMessage,        Content = "Vous avez reçu un nouveau message.",             IsRead = false, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Notification { Id = 107, UserId = 107, Type = NotificationType.EventUpdated,      Content = "L'événement Workshop Docker a été mis à jour.",  IsRead = false, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Notification { Id = 108, UserId = 108, Type = NotificationType.PostReported,      Content = "Un de vos posts a été signalé.",                 IsRead = true,  CreatedAt = seedDate, UpdatedAt = seedDate },
            new Notification { Id = 109, UserId = 109, Type = NotificationType.NewLikeOnPost,     Content = "Votre post a reçu un like.",                     IsRead = false, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Notification { Id = 110, UserId = 110, Type = NotificationType.NewEventCreated,   Content = "Le CTF Cybersécurité ESGI est publié !",          IsRead = false, CreatedAt = seedDate, UpdatedAt = seedDate }
        );
    }
}