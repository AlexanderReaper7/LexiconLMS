namespace LexiconLMS.Shared.Entities;

public class Activity
{
    public Guid Id { get; set; }
    public ActivityType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid ModuleId { get; set; }
    public Module? Module { get; set; }
}