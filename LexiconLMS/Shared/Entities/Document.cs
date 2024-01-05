namespace LexiconLMS.Shared.Entities;

public class Document
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime UploadDate { get; set; }
    public string Path { get; set; } = string.Empty;
    public ApplicationUser? Uploader { get; set; }
    public string UploaderId { get; set; } = string.Empty;
}

public class ModuleDocument : Document
{
    public Guid ModuleId { get; set; }
    
}

public class ActivityDocument : Document
{
    public Guid ActivityId { get; set; }
}

public class CourseDocument : Document
{
    public Guid CourseId { get; set; }
}