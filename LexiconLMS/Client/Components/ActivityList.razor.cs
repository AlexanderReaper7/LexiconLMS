using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using LexiconLMS.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using LexiconLMS.Client.Helpers;

namespace LexiconLMS.Client.Components
{
    public partial class ActivityList
    {
       
        //public IGenericDataService? ActivityDataService { get; set; }
        [Inject]
        public IGenericDataService? GenericDataService { get; set; }

        public List<Activity> ActivityLst { get; set; } = new List<Activity>();
  

        protected override async Task OnInitializedAsync()
        {

            ActivityLst = await GenericDataService.GetAsync<List<Activity>>(UriHelper.GetActivitiesUri());

            await base.OnInitializedAsync();
        }
    }
}