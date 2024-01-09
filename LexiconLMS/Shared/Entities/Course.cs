namespace LexiconLMS.Shared.Entities;

public class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ICollection<Module>? Modules { get; set; }

    public ICollection<ApplicationUser>? Users { get; set; }
    public ICollection<CourseDocument>? CourseDocuments { get; set; }
}