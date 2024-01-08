using LexiconLMS.Server.Data;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using LexiconLMS.Server.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using System.Security.Claims;
using LexiconLMS.Shared.Dtos;

namespace LexiconLMS;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        //Works without builder.Services.AddAuthentication().AddIdentityServerJwt()
        builder.Services.AddIdentityServer()
        .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(
        
        options => {

            options.IdentityResources["openid"].UserClaims.Add("role");

            if (options.ApiResources.Any())
            {
                options.ApiResources.Single().UserClaims.Add("role");
            }
        });

        //builder.Services.AddIdentityServer()
        //    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options => {
        //        options.IdentityResources["openid"].UserClaims.Add("role");
        //        options.ApiResources.Single().UserClaims.Add("role");
        //    });


        //builder.Services.AddAuthentication()
        //    .AddIdentityServerJwt();

        //builder.Services.Configure<JwtBearerOptions>(IdentityServerJwtConstants.IdentityServerJwtBearerScheme,
        //options =>
        //{
        //    options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
        //});

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(typeof(ActivitiesMappings));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseWebAssemblyDebugging();
            app.UseSwagger();
            app.UseSwaggerUI();
            // ----------------------------------------
            // NOTE: Uncomment the following line to delete the database each time on startup
            // Leave this commented out when committing to git
            // ----------------------------------------
            await app.SeedDataAsync();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
