using LexiconLMS.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconLMS.Shared.Dtos.ActivitiesDtos
{
    public class AssignmentsDtoForStudents
	{
        public Guid AssignmentId { get; set; }
        public string AssignmentName { get; set; } = string.Empty;
        public SubmissionState SubmissionState { get; set; }
        public DateTime DueDate { get; set; }
    }
    public class AssignmentDtoForStudents
	{
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public SubmissionState SubmissionState { get; set; }
        public DateTime DueDate { get; set; }
        public List<Document> Documents { get; set; } = new List<Document>();
    }
}
