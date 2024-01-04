using LexiconLMS.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconLMS.Shared.Dtos.ActivitiesDtos
{
    public class AssigmentDtoForTeachers
    {
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public Guid AssignmentId { get; set; }
        public string AssignmentName { get; set; } = string.Empty;
        public SubmissionState SubmissionState { get; set; }
        public DateTime DueDate { get; set; }

    }
}
