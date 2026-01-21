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

        // User <-> Post (Saves)
        modelBuilder.Entity<User>()
            .HasMany(u => u.SavedPosts)
            .WithMany(p => p.HavingSavedUsers)
            .UsingEntity(j => j.ToTable("PostSaves"));

        // User <-> Channel (Members)
        modelBuilder.Entity<Channel>()
            .HasMany(c => c.Users)
            .WithMany(u => u.Channels)
            .UsingEntity(j => j.ToTable("ChannelUsers"));
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

        // --- MESSAGERIE ---
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
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int>
            {
                Id = 1,
                Name = AppRoles.Admin,
                NormalizedName = AppRoles.Admin.ToUpperInvariant(),
                ConcurrencyStamp = "ADMIN_CONCURRENCY_STAMP"
            },
            new IdentityRole<int>
            {
                Id = 2,
                Name = AppRoles.Student,
                NormalizedName = AppRoles.Student.ToUpperInvariant(),
                ConcurrencyStamp = "STUDENT_CONCURRENCY_STAMP"
            }
        );
    }
}