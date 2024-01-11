using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconLMS.Shared.Entities
{
	public class Feedback
	{
        public Guid Id { get; set; }
        public Guid AssignmentId { get; set; }
        public ApplicationUser? Student { get; set; }
        public string StudentId { get; set; } = string.Empty;
		public ApplicationUser? Teacher { get; set; }
		public string TeacherId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

    }
}
