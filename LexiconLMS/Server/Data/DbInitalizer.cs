using Bogus;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Identity;

namespace LexiconLMS.Server.Data;

public static class DbInitializer
{
    private static readonly Faker Faker = new Faker();
    private static RoleManager<IdentityRole> roleManager = default!;
    private static UserManager<ApplicationUser> userManager = default!;

    private static readonly List<IdentityRole> Roles = new()
    {
        new IdentityRole { Name = "Teacher" },
        new IdentityRole { Name = "Student" }
    };

    public static async Task InitAsync(ApplicationDbContext db, IServiceProvider services)
    {
        // --- Add Roles --
        roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        // add roles to database if they don't exist
        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name!))
            {
                await roleManager.CreateAsync(role);
            }
        }

        userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        const int courseCount = 3;
        var courses = new List<Course>();
        for (int i = 0; i < courseCount; i++)
        {
            courses.Add(await CreateCourse());
        }
        await db.AddRangeAsync(courses);
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// List of example course names
    /// </summary>
    private static string[] CourseNames = { "C#", "Javascript", "HTML/CSS" };

    /// <summary>
    /// Creates a course with modules, activities and users
    /// </summary>
    /// <returns></returns>
    private static async Task<Course> CreateCourse()
    {
        // --- Add Users ---
        // create the default users
        var users = new List<ApplicationUser>();
        // Teachers
        int teacherCount = Faker.Random.Int(1, 3);
        for (int i = 0; i < teacherCount; i++)
        {
            users.Add(GenerateUser());
        }
        // Students
        int studentCount = Faker.Random.Int(3, 11);
        for (int i = 0; i < studentCount; i++)
        {
            users.Add(GenerateUser());
        }
        // add the users to userManager
        int j = 0;
        foreach (var user in users)
        {
            j++;
            await userManager.CreateAsync(user, "!123qwe");
            await userManager.AddToRoleAsync(user, (j < teacherCount ? Roles[0].Name : Roles[1].Name)!);
        }

        var startDate = Faker.Date.Between(DateTime.Now, DateTime.Now.AddDays(100));
        var endDate = startDate.AddDays(Faker.Random.Int(30, 120));

        var modules = GenerateModules(startDate, endDate);

        return new Course
        {
            Name = CourseNames[Faker.Random.Int(0, CourseNames.Length - 1)],
            Description = Faker.Lorem.Sentence(),
            StartDate = startDate,
            EndDate = endDate,
            Modules = modules,
            Users = users,
        };
    }

    /// <summary>
    /// Generates a single student with name, email
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static ApplicationUser GenerateUser()
    {
        var firstname = Faker.Name.FirstName();
        var lastname = Faker.Name.LastName();
        var email = Faker.Internet.Email(firstname, lastname);
        var user = new ApplicationUser
        {
            UserName = $"{firstname} {lastname}",
            Email = email,
            EmailConfirmed = true,
        };
        return user;
    }

    /// <summary>
    /// Generates a list of modules with start and end dates
    /// </summary>
    /// <returns></returns>
    private static List<Module> GenerateModules(DateTime start, DateTime end)
    {
        var output = new List<Module>();
        var moduleCount = Faker.Random.Int(1, 4);
        for (int i = 0; i < moduleCount; i++)
        {
            // create a module with a start date after the previous module's end date
            output.Add(GenerateModule(i == 0 ? start : output[i - 1].EndDate));
        }
        return output;
    }

    /// <summary>
    /// List of example module names
    /// </summary>
    private static readonly string[] ModuleNames = { "Database design", "Javascript", "C#", "Azure" };

    /// <summary>
    /// Generates a single module with start and end dates
    /// </summary>
    /// <param name="after">The date after which the module should start</param>
    /// <returns></returns>
    private static Module GenerateModule(DateTime startDate)
    {
        var endDate = Faker.Date.Soon(4);
        return new Module
        {
            Name = ModuleNames[Faker.Random.Int(0, ModuleNames.Length - 1)],
            Description = Faker.Lorem.Sentence(),
            StartDate = startDate,
            EndDate = endDate,
            Activities = GenerateActivities(ActivityTypes, startDate, endDate)
        };
    }

    /// <summary>
    /// List of example activity types
    /// </summary>
    private static readonly List<ActivityType> ActivityTypes = new List<ActivityType>
        {
            new ActivityType { Name = "E-Learning" },
            new ActivityType { Name = "Lecture" },
            new ActivityType { Name = "Practice session" },
            new ActivityType { Name = "Assignment" },
        };

    /// <summary>
    /// List of assignment name/description pairs examples
    /// </summary>
    private static readonly List<(string, string)> AssignmentNameDescriptionPairs = new List<(string, string)> { 
        ("Garage 1.0", "Initiera Garage"),
        ("Mankind", "HTML and CSS"),
        ("Coffeshop", "Recursion"),
        ("Database Design", "ER Diagrams"),
        ("Garage 3.0", "Expansion of Garage 1.0") };

    /// <summary>
    /// Generate activities for a module
    /// </summary>
    /// <param name="types">List of types to generate for</param>
    /// <param name="start">start of the module the activities are generated for</param>
    /// <param name="end">end of the module the activities are generated for</param>
    /// <returns></returns>
    private static List<Activity> GenerateActivities(List<ActivityType> types, DateTime start, DateTime end)
    {
        var activities = new List<Activity>();

        var activityCount = Faker.Random.Int(types.Count, 3 * types.Count);

        // create random activities wit random order of the types
        for (int i = 0; i < activityCount; i++)
        {
            // pick a random type and name/description pair
            var type = types[Faker.Random.Int(0, types.Count - 1)];
            var nameDescriptionPair = AssignmentNameDescriptionPairs[Faker.Random.Int(0, AssignmentNameDescriptionPairs.Count - 1)];
            // start the activity after the previous activity's end date, or the module's start date if this is the first activity
            var startDate = activities.Count == 0 ? start : activities[i-1].EndDate;

            var endDate = activityCount == i - 1
                ? end
                : Faker.Date.Between(startDate, end.Subtract(TimeSpan.FromSeconds(1)));
            // add the activity to the list
            activities.Add(new Activity
            {
                Id = Guid.NewGuid(),
                Name = nameDescriptionPair.Item1,
                Description = nameDescriptionPair.Item2,
                Type = type,
                StartDate = startDate,
                EndDate = endDate,
            });
        }
        return activities;
    }

}