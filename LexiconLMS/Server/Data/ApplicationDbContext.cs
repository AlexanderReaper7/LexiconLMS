﻿using Azure;
using Duende.IdentityServer.EntityFramework.Options;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
	public DbSet<Document> Documents { get; set; } = null!;
	public DbSet<Feedback> Feedbacks { get; set; } = null!;


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

		modelBuilder.Entity<Activity>()
		.HasMany(e => e.ActivityDocument)
		.WithOne()
		.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Module>().
			HasMany(m => m.ModuleDocuments)
			.WithOne()
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Course>().
			HasMany(m => m.CourseDocuments)
			.WithOne()
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Feedback>(b =>
		{
			b.HasOne<Activity>()
			.WithMany()
			.HasForeignKey(a => a.AssignmentId);
			b.HasOne(f => f.Student).WithMany().OnDelete(DeleteBehavior.NoAction);
			b.HasOne(f => f.Teacher).WithMany().OnDelete(DeleteBehavior.NoAction);
		});

		modelBuilder.Entity<IdentityRole>(b =>
		{
			b.HasMany<ApplicationUser>()
			.WithMany(u => u.Roles)
			.UsingEntity<IdentityUserRole<string>>(
				l => l.HasOne<ApplicationUser>().WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(ApplicationUser.Id)),
				r => r.HasOne<IdentityRole>().WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(IdentityRole.Id)),
				j => j.HasKey("RoleId", "UserId")
				);
		});

		base.OnModelCreating(modelBuilder);
	}


	//public DbSet<LexiconLMS.Shared.Entities.Document> Documents { get; set; } = default!;


	//public DbSet<LexiconLMS.Shared.Entities.CourseDocument> CourseDocument { get; set; } = default!;


	//public DbSet<LexiconLMS.Shared.Entities.ModuleDocument> ModuleDocument { get; set; } = default!;

}
