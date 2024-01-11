namespace LexiconLMS.Shared.Dtos.ActivitiesDtos
{
	public class AssignmentDto
	{
        public Guid Id { get; set; }
        public Guid ModuleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
