using LexiconLMS.Client.Helpers;
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Dtos;
using LexiconLMS.Shared.Dtos.ActivitiesDtos;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace LexiconLMS.Client.Pages
{
    public partial class ModuleDetails
    {
		[Inject]
		public NavigationManager NavigationManager { get; set; } = default!;

		[Inject]
		public IGenericDataService GenericDataService { get; set; } = default!;

		[Parameter]
		public Guid? ModuleId { get; set; }

        public Module Module { get; set; } = new Module();

        public IEnumerable<Activity> Activities { get; set; } = new List<Activity>();
		public List<Document> ModuleDocuments { get; set; } = new List<Document>();
		[Inject]
		public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
		public IEnumerable<AssignmentDtoForTeachers> AssignmentsForTeachers { get; set; } = default!;
        public IEnumerable<AssignmentDtoForStudents> AssignmentsForStudents { get; set; } = default!;
		public List<Document> StudentDocuments { get; set; } = new List<Document>();

			
		public string ErrorMessage { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

		protected override async Task OnInitializedAsync()
		{
			if (ModuleId == null)
			{
				ErrorMessage = "Module not found";
				return;
			}

			Module = (await GenericDataService.GetAsync<Module>(UriHelper.GetModuleUri(ModuleId.Value)))!;
			ModuleDocuments = await GenericDataService.GetAsync<List<Document>>($"moduledocumentsbymodule/{ModuleId}") ?? ModuleDocuments;
			StudentDocuments = await GenericDataService.GetAsync<List<Document>>($"studentdocumentsbymodule/{ModuleId}") ?? StudentDocuments;
			if (Module == null)
			{
				ErrorMessage = "Module not found";
				return;
			}

			Activities = (await GenericDataService.GetAsync<IEnumerable<Activity>>(UriHelper.GetActivitiesUri(ModuleId.Value.ToString())))!;

			ClaimsPrincipal user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
			
			if (user.IsInRole(StaticUserRoles.Teacher.ToString()))
			{
				if (ModuleId == null || Module.CourseId == null)
				{
					return;
				}
				AssignmentsForTeachers = (await GenericDataService.GetAsync<IEnumerable<AssignmentDtoForTeachers>>(UriHelper.GetAssignmentsTeachersUri(Module.CourseId, ModuleId)))!;
			}
			else if (user.IsInRole(StaticUserRoles.Student.ToString()))
			{
				if (ModuleId == null)
				{
					return;
				}
				string userId = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.FindFirstValue("sub")!;

				if (string.IsNullOrEmpty(userId))
				{
					return;
				}

				AssignmentsForStudents = (await GenericDataService.GetAsync<IEnumerable<AssignmentDtoForStudents>>(UriHelper.GetAssignmentsStudentsUri(ModuleId, userId)))!;
			}

			await base.OnInitializedAsync();
		}
	}
}