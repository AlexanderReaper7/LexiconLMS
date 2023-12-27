namespace LexiconLMS.Shared.Entities;

public class Module
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CourseId { get; set; }
    public Course? Course { get; set; }
    public ICollection<Activity>? Activities { get; set; }
}