using LexiconLMS.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconLMS.Shared.Dtos.ActivitiesDtos
{
	public class ActivityDto
	{
		public Guid Id { get; set; }
		public string Type { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
