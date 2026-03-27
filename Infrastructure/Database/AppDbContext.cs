using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MojiiBackend.Application.Shared;
using MojiiBackend.Domain.Entities;

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
        modelBuilder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int>
            {
                Id = 1,
                Name = AppRoles.Student,
                NormalizedName = AppRoles.Student.ToUpperInvariant(),
                ConcurrencyStamp = "STUDENT_CONCURRENCY_STAMP"
            },
            new IdentityRole<int>
            {
                Id = 2,
                Name = AppRoles.SchoolAdmin,
                NormalizedName = AppRoles.SchoolAdmin.ToUpperInvariant(),
                ConcurrencyStamp = "SCHOOL_ADMIN_CONCURRENCY_STAMP"
            },
            new IdentityRole<int>
            {
                Id = 3,
                Name = AppRoles.SuperAdmin,
                NormalizedName = AppRoles.SuperAdmin.ToUpperInvariant(),
                ConcurrencyStamp = "SUPER_ADMIN_CONCURRENCY_STAMP"
            }
        );
    }
}