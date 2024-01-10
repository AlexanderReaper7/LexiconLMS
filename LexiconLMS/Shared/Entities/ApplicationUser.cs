

using Microsoft.AspNetCore.Identity;

namespace LexiconLMS.Shared.Entities;

public class ApplicationUser : IdentityUser
{
    public Guid? CourseId { get; set; }
    public Course? Course { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Fullname => $"{FirstName} {LastName}";
    public ICollection<IdentityRole> Roles { get; set; }
}
