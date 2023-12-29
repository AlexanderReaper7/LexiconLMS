using LexiconLMS.Client.Services;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.WebSockets;

namespace LexiconLMS.Client.Pages
{
    public partial class ModuleAdd
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public IModuleDataService ModuleDataService { get; set; } = default!;

        [Parameter]
        public Guid? CourseId { get; set; }

        public Module Module { get; set; } = new Module();

        public string ErrorMessage { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();
		}

		private async Task HandleValidSubmit()
        {
			try
			{
				Module.CourseId = CourseId!.Value;
				if (await ModuleDataService.AddAsync(Module))
				{
					NavigationManager.NavigateTo("/");
				}
				else
				{
					ErrorMessage = "Could not add module";
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = $"{ex.Message} {ex.HResult}";
			}
		}

        private void DeleteModule()
        {

        }
    }
}