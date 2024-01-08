using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Client.Extensions;
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

    public static string GetCSSClass(this SubmissionState status)
    {
        return status switch
        {
            SubmissionState.Submitted => "alert-success",
            SubmissionState.NotSubmitted => "alert-warning",
            SubmissionState.Late => "alert-danger",
            SubmissionState.SubmittedLate => "alert-info",
            _ => ""
        };
    }
}
