

using Microsoft.AspNetCore.Identity;

namespace LexiconLMS.Shared.Entities;

public class ApplicationUser : IdentityUser
{
    public Course? Course { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
