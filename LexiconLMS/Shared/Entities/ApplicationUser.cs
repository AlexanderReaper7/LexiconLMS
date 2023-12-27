

using Microsoft.AspNetCore.Identity;

namespace LexiconLMS.Shared.Entities;

public class ApplicationUser : IdentityUser
{
    public Course? Course { get; set; }

}
