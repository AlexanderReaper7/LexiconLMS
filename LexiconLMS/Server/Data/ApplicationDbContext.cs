using Duende.IdentityServer.EntityFramework.Options;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace LexiconLMS.Server.Data;
public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
    }

    public DbSet<Activity> Activities { get; set; } = null!;
    public DbSet<ActivityType> ActivityTypes { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Module> Modules { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
        .HasMany(e => e.Modules)
        .WithOne(e => e.Course)
        .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Course>()
       .HasMany(e => e.Users)
       .WithOne(e => e.Course)
       .OnDelete(DeleteBehavior.SetNull);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<LexiconLMS.Shared.Entities.Document> Document { get; set; } = default!;

    public DbSet<LexiconLMS.Shared.Entities.ActivityDocument> ActivityDocument { get; set; } = default!;

}
