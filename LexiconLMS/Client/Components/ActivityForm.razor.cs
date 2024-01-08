using LexiconLMS.Client.Helpers;
using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace LexiconLMS.Client.Components;
public partial class ActivityForm
{
	[Inject]
	public NavigationManager NavigationManager { get; set; } = default!;

	[Inject]
	public IGenericDataService GenericDataService { get; set; } = default!;

	[Parameter]
	public Guid? ActivityId { get; set; }

	[Parameter]
	public Guid? ModuleId { get; set; }

	[Parameter]
	public bool OnlyAssignments { get; set; }
	public Activity Activity { get; set; } = new Activity();

	public string ErrorMessage { get; set; } = string.Empty;

	public string Message { get; set; } = string.Empty;

    public string EntityName { get; set; }


    protected override async Task OnInitializedAsync()
	{
		EntityName = OnlyAssignments ? "Assignment" : "Activity";
		if (ActivityId == null)
		{
			Activity.Type.Name = "Assignment";
			return;
		}

		Activity = await GenericDataService.GetAsync<Activity>(UriHelper.GetActivityUri(ActivityId)) ?? Activity;
		if (Activity == null)
		{
			ErrorMessage = EntityName+" not found";
			return;
		}

		ModuleId = Activity.ModuleId;
		await base.OnInitializedAsync();
	}

	private async Task HandleValidSubmit()
	{
		if (ActivityId == null)
		{
			//Save activity
			try
			{
				Activity.ModuleId = ModuleId!.Value;
				Activity.Module = await GenericDataService.GetAsync<Module>(UriHelper.GetModuleUri(ModuleId));
				if (await GenericDataService.AddAsync(UriHelper.GetActivitiesUri(), Activity))

				{
					NavigationManager.NavigateTo(UriHelper.GetModuleDetailsUri(ModuleId));
				}
				else
				{

					ErrorMessage = "Could not add "+ EntityName.ToLower();
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = $"{ex.Message} {ex.HResult}";
			}
		}
		else
		{
			//update activity
			try
			{

				if (await GenericDataService.UpdateAsync(UriHelper.GetActivityUri(ActivityId), Activity))
				{
					Message = EntityName+" saved";
				}
				else
				{
					ErrorMessage = "Could not update "+ EntityName;
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = $"{ex.Message} {ex.HResult}";
			}
		}
	}

	private async Task DeleteActivity()
	{
		try
		{
			if (Activity == null)
			{
				return;
			}
			if (await GenericDataService.DeleteAsync(UriHelper.GetActivityUri(ActivityId)))
			{
				NavigationManager.NavigateTo(UriHelper.GetModuleDetailsUri(Activity.ModuleId));
			}
			else
			{
				ErrorMessage = "Could not delete "+ EntityName;
			}
		}
		catch (Exception ex)
		{
			ErrorMessage = ex.Message;
		}
	}
}