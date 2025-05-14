using Microsoft.EntityFrameworkCore;
using myroompal_api.Entities.Entities;
using myroompal_api.Entities.Types;
using Newtonsoft.Json.Linq;

namespace myroompal_api.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Preference> Preferences { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<SupportTicket> SupportTickets { get; set; }
    public DbSet<UserRating> UserRatings { get; set; }
    public DbSet<PreferenceUser> PreferenceUsers { get; set; }
    public DbSet<ProfilePreferences> ProfilePreferences {get;set;}

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Room
        modelBuilder.Entity<Room>(entity =>
        {
            entity
                .HasOne(e => e.Owner)
                .WithMany(e => e.Rooms)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(e => e.Address)
                .WithOne()
                .HasForeignKey<Room>(e => e.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //User Preferences
        modelBuilder.Entity<PreferenceUser>(entity =>
        {
            entity
                .HasOne(e => e.User)
                .WithMany(e => e.PreferencesUsers)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity
                .HasOne(e => e.Preference)
                .WithMany(e => e.PreferencesUsers)
                .HasForeignKey(e => e.PreferenceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

    
        //User matches
        modelBuilder.Entity<User>()
            .HasMany(e => e.Matches)
            .WithOne(e => e.MatcherUser)
            .HasForeignKey(e => e.MatcherUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);


        //User Rolenames
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.RoleName)
                .HasConversion<string>()
                .IsRequired();

            entity.ToTable(e => e.HasCheckConstraint(
                "CK_RoleNames", "[RoleName] IN ('Tenant', 'Owner', 'Moderator')"
            ));

            entity.Property(u => u.Status)
                .HasConversion<string>()
                .IsRequired();

            entity.ToTable(u => u.HasCheckConstraint(
                "CK_UserAccountStatusTypes", "[Status] IN ('Active', 'Banned')"
            ));
        });


        //Support Ticket Issue Type
        modelBuilder.Entity<SupportTicket>(entity =>
        {
            entity.Property(s => s.Status)
                .HasConversion<string>();

            entity.ToTable(s => s.HasCheckConstraint(
                "CK_SupportTicketStatus", "[Status] IN ('New', 'Committed', 'Done')"
            ));

            entity.Property(s => s.IssueType)
                .HasConversion<string>();

            entity.ToTable(s => s.HasCheckConstraint(
                "CK_SupportTicketIssueType", "[IssueType] IN ('Login', 'Payment', 'Account', 'Renting', 'Other')"
            ));
        });

        modelBuilder.Entity<SupportTicket>()
            .HasOne(e => e.CreatorOfTicket)
            .WithMany(e => e.SupportTickets)
            .HasForeignKey(e => e.CreatorOfTicketId)
            .OnDelete(DeleteBehavior.Restrict);


        //User ratings
        modelBuilder.Entity<UserRating>()
            .HasOne(e => e.User)
            .WithMany(e => e.Ratings)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserRating>()
            .HasOne(e => e.RatedUser)
            .WithMany()
            .HasForeignKey(e => e.RatedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserRating>()
            .HasIndex(e => new { e.UserId, e.RatedUserId })
            .IsUnique();
    }
}