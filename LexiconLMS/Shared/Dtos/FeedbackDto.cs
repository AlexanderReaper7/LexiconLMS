using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconLMS.Shared.Dtos
{
	public class FeedbackDto
	{
		public Guid? Id { get; set; }
		public string Message { get; set; } = string.Empty;
		public string? TeacherId { get; set; }
		public string TeacherName { get; set; } = string.Empty;
	}
}
