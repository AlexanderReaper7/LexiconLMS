using LexiconLMS.Client.Components;
using LexiconLMS.Client.Helpers;
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace LexiconLMS.Client.Pages
{
    public partial class CourseDetails
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public ICourseDataService? CourseDataService { get; set; }

        [Inject]
        public IModuleDataService? ModuleDataService { get; set; }
		
        [Inject]
		public IGenericDataService GenericDataService { get; set; } = default!;

		[Inject]
        public IApplicationUserDataService? ApplicationUserDataService { get; set; }

        [Parameter]
        public string? CourseId { get; set; }

        public Course Course { get; set; }
        public List<Module> Modules { get; set; }
		public List<Document> CourseDocuments { get; set; } = new List<Document>();

		public List<ApplicationUser> Students { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(CourseId))
            {
                Course = await CourseDataService.GetCourse(Guid.Parse(CourseId));
                Modules = await ModuleDataService.GetModulesByCourseId(Guid.Parse(CourseId));
                Students = await ApplicationUserDataService.GetStudentsByCourseId(Guid.Parse(CourseId));
				//CourseDocuments = await GenericDataService.GetAsync<List<Document>>($"coursedocumentsbycourse/{CourseId}") ?? CourseDocuments;
			}
            
            await base.OnInitializedAsync();
        }
    }
}
