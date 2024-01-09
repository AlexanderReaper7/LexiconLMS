using LexiconLMS.Client.Helpers;
using LexiconLMS.Client.Services;
using Microsoft.AspNetCore.Components;
using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Client.Pages
{
    
    public partial class ActivitiesList
    {
        [Inject]
        public IGenericDataService GenericDataService { get; set; } = default!;

        public List<Activity> ActivityLst { get; set; } = new List<Activity>();


        protected override async Task OnInitializedAsync()
        {

            ActivityLst = await GenericDataService.GetAsync<List<Activity>>(UriHelper.GetActivitiesUri()) ?? ActivityLst; 

            await base.OnInitializedAsync();
        }

    }
}
