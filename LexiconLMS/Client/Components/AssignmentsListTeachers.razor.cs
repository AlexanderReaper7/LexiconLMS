using LexiconLMS.Client.Helpers;
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Dtos.ActivitiesDtos;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LexiconLMS.Client.Components
{
    public partial class AssignmentsListTeachers
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Parameter]
		[EditorRequired]
		public IEnumerable<AssignmentDtoForTeachers> Assignments { get; set; } = default!;

        public string Message { get; set; } = string.Empty;
        private void ShowMessage(string message)
        {
            Message = message;
        }
    }
}