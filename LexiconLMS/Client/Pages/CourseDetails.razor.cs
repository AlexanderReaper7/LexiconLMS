﻿using LexiconLMS.Client.Components;
using LexiconLMS.Client.Helpers;
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace LexiconLMS.Client.Pages
{
    public partial class CourseDetails
    {
        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        [Inject]
        public required ICourseDataService CourseDataService { get; set; }

        [Inject]
        public required IModuleDataService ModuleDataService { get; set; }
		
        [Inject]
		public required IGenericDataService GenericDataService { get; set; }

		[Inject]
        public required IApplicationUserDataService ApplicationUserDataService { get; set; }

        [Parameter]
        public string? CourseId { get; set; }

        public Course Course { get; set; } = default!;
        public List<Module> Modules { get; set; } = default!;
		public List<Document> CourseDocuments { get; set; } = default!;

		public required List<ApplicationUser> Students { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Guid courseId;
            if (string.IsNullOrEmpty(CourseId))
            {
                // If no course id is provided, use the logged in user's course
                Course = await CourseDataService.GetMyCourse();
                CourseId = Course.Id.ToString();
                courseId = Course.Id;
            }
            else
            {
                // If a course id is provided, try to use it and if it failed, show not found.
                try
                {
                    courseId = Guid.Parse(CourseId);
                }
                catch (Exception)
                {
                    // not found
                    throw new NotImplementedException();
                    return;
                }
                Course = await CourseDataService.GetCourse(courseId);
			}
            Modules = await ModuleDataService.GetModulesByCourseId(courseId);
            Students = await ApplicationUserDataService.GetStudentsByCourseId(courseId);
			CourseDocuments = await GenericDataService.GetAsync<List<Document>>($"coursedocumentsbycourse/{courseId}") ?? new List<Document>();
            
            await base.OnInitializedAsync();
        }
    }
}
