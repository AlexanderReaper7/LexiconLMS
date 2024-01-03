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

		[Parameter]
		public IEnumerable<Activity>? ActivityLst { get; set; } = null;
  

        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }
    }
}