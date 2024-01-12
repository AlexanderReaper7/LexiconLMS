using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using LexiconLMS.Client.Helpers;
using LexiconLMS.Shared.Dtos.ActivitiesDtos;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using LexiconLMS.Shared.Dtos;
using System.Reflection;
using System.Net;
using LexiconLMS.Client.Pages;


namespace LexiconLMS.Client.Components
{
	public partial class AssignmentDetailsComponent
	{
		[Inject]
		public NavigationManager NavigationManager { get; set; } = default!;

		[Inject]
		public IGenericDataService GenericDataService { get; set; } = default!;

		[Parameter]
		[EditorRequired]
		public Guid? ActivityId { get; set; }

		[Parameter]
		public string? StudentId { get; set; }

		public string StudentName { get; set; }

		[Inject]
		public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

		public AssignmentDto Assignment { get; set; } = new AssignmentDto();
		public FeedbackDto Feedback { get; set; } = new FeedbackDto();
		public List<Document> ActivityDocuments { get; set; } = new List<Document>();
		public List<Document> StudentDocuments { get; set; } = new List<Document>();
		public IEnumerable<AssignmentDtoStudentAndStatusOnly>? AssignmentForTeachers { get; set; }
		public SubmissionState? SubmissionState { get; set; }
		public DateTime? SubmissionDate { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
		public ClaimsPrincipal User { get; set; } = default!;
		public string UserId { get; set; } = string.Empty;


        protected override async Task OnInitializedAsync()
		{
			
			User = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
			UserId = User.FindFirstValue("sub")!;

			if (ActivityId == null)
			{
				ErrorMessage = "Assignment not found";
				return;
			}

			Assignment = await GenericDataService.GetAsync<AssignmentDto>(UriHelper.GetAssignmentUri(ActivityId)) ?? Assignment;

			if (Assignment == null)
			{
				ErrorMessage = "Assignment not found";
				return;
			}
			ActivityDocuments = (await GenericDataService.GetAsync<List<Document>>($"activitydocumentsbyactivity/{ActivityId}"))!;


			if (StudentId != null || User.IsInRole(StaticUserRoles.Student.ToString()))
			{
				
				if (!await InitWhenStudent())
				{
					return;
				}
				try
				{
					Feedback = (await GenericDataService.GetAsync<FeedbackDto>(UriHelper.GetFeedbackUri(StudentId, ActivityId))) ?? Feedback;
				}
				catch (HttpRequestException ex)
				{
					if (ex.StatusCode != HttpStatusCode.NotFound)
					{
						ErrorMessage = ex.Message;
						return;
					}
				}
			}
			else if (User.IsInRole(StaticUserRoles.Teacher.ToString()))
			{
				//Init when teacher
				AssignmentForTeachers = (await GenericDataService.GetAsync<IEnumerable<AssignmentDtoStudentAndStatusOnly>>(UriHelper.GetAssignmentOnlyStudentsUri(ActivityId)));
			}

			await base.OnInitializedAsync();
		}

		private async Task<bool> InitWhenStudent()
		{
			if (StudentId == null)
			{
				StudentId =  UserId;
			}

			var student = await GenericDataService.GetAsync<ApplicationUser>(UriHelper.GetApplicationUserUri(StudentId));

			if (student == null)
			{
				ErrorMessage = "Could not find student";
				return false;
			}

			StudentName = student.Fullname;

			StudentDocuments = (await GenericDataService.GetAsync<List<Document>>(UriHelper.GetAssignmentStudentUri(StudentId, ActivityId)))!;

			DateTime now = DateTime.Now;
			if (StudentDocuments.Any())
			{
				SubmissionDate = StudentDocuments.Min(d => d.UploadDate);
				SubmissionState = Assignment.DueDate >= SubmissionDate ? LexiconLMS.Shared.Entities.SubmissionState.Submitted : LexiconLMS.Shared.Entities.SubmissionState.SubmittedLate;
			}
			else
			{
				SubmissionState = Assignment.DueDate >= now ? LexiconLMS.Shared.Entities.SubmissionState.NotSubmitted : LexiconLMS.Shared.Entities.SubmissionState.Late;
			}
			return true;
		}

		private async Task DeleteActivity()
		{
			try
			{
				if (Assignment == null)
				{
					return;
				}
				if (await GenericDataService.DeleteAsync(UriHelper.GetActivityUri(ActivityId)))
				{
					NavigationManager.NavigateTo("/");
				}
				else
				{
					ErrorMessage = "Could not delete Module";
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
			}
		}

		private async Task HandleFeedbackValidSubmit()
		{
			if (Feedback.Id == null)
			{
				//Save feedback
				try
				{
					Feedback feedbackToSave = new Feedback
					{
						Message = Feedback.Message,
						StudentId = StudentId!,
						TeacherId = UserId,
						AssignmentId = Assignment.Id
					};
					if (!await GenericDataService.AddAsync(UriHelper.GetFeedbacksUri(), feedbackToSave))
					{
						ErrorMessage = "Could not add feedback";
					}
					else
					{
						Message = "Feedback saved";
					}
				}
				catch (Exception ex)
				{
					ErrorMessage = $"{ex.Message} {ex.HResult}";
				}
			}
			else
			{
                if (string.IsNullOrWhiteSpace(Feedback.Message))
                {
					//Delete feedback
					try
					{
						if (await GenericDataService.DeleteAsync(UriHelper.GetFeedbackUri(Feedback.Id)))
						{
							Message = "Feedback deleted";
							Feedback = new FeedbackDto();
						}
						else
						{
							ErrorMessage = "Could not delete Module";
						}
					}
					catch (Exception ex)
					{
						ErrorMessage = ex.Message;
					}
					return;
				}
				//update feedback
				try
				{

					if (await GenericDataService.UpdateAsync(UriHelper.GetFeedbackUri(Feedback.Id), Feedback))
					{
						Message = "Feedback saved";
					}
					else
					{
						ErrorMessage = "Could not update feedback";
					}
				}
				catch (Exception ex)
				{
					ErrorMessage = $"{ex.Message} {ex.HResult}";
				}
			}
		}
	}
}