namespace LexiconLMS.Shared.Entities;

public class Document
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime UploadDate { get; set; }
    public string Path { get; set; } = string.Empty;
    public ApplicationUser Uploader { get; set; }
}

public class ModuleDocument : Document
{
    
}

public class ActivityDocument : Document
{
    
}

public class CourseDocument : Document
{
    
}