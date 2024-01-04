using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LexiconLMS.Shared.Entities
{
	public enum SubmissionState
	{
		Submitted,
		NotSubmitted,
		Late,
		SubmittedLate
	}

	public static class SubmissionStateExtensions
	{
		public static string AsText(this SubmissionState s)
		{
			return s switch
			{
				SubmissionState.Submitted => "Submitted",
				SubmissionState.NotSubmitted => "Not submitted",
				SubmissionState.Late => "Late",
				SubmissionState.SubmittedLate => "Submitted late",
				_ => "",
			};
		}
	}
}
