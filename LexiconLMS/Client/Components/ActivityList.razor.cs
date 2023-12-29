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
        [Inject]
        public IGenericDataService? ActivityDataService { get; set; }

        public List<Activity> ActivityLst { get; set; } = new List<Activity>();

        public bool Error = false;
        public string responseData = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            string path = "/activities";
            ActivityLst = await GenericDataService.GetAsync(path);

            await base.OnInitializedAsync();
        }
    }
}