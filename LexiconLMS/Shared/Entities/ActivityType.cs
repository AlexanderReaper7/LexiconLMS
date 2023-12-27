using System.ComponentModel.DataAnnotations;

namespace LexiconLMS.Shared.Entities;

public class ActivityType
{
    [Key]
    public string Name { get; set; } = string.Empty;
}
