using LexiconLMS.Client.Helpers;
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace LexiconLMS.Client.Components
{
    public partial class ButtonDownloadSubmission
    {
        [Inject]
        public IGenericDataService GenericDataService { get; set; }

        [Parameter]
        [EditorRequired]
        public string StudentId { get; set; }

        [Parameter]
        [EditorRequired]
        public Guid AssignmnetId { get; set; }

        [Parameter]
        [EditorRequired]
        public SubmissionState SubmissionState { get; set; }

        [Parameter]
        public EventCallback<string> DownloadedEvent { get; set; }

        public async Task Download()
        {
            var StudentDocuments = (await GenericDataService.GetAsync<List<Document>>(UriHelper.GetAssignmentStudentUri(StudentId, AssignmnetId)))!;
            await DownloadedEvent.InvokeAsync($"{StudentDocuments.Count} document{(StudentDocuments.Count != 1 ? "s" : "")} downloaded");

        }
    }
}