using LexiconLMS.Shared.Entities;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using LexiconLMS.Client.Helpers;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using Application = System.Net.Mime.MediaTypeNames.Application;

namespace LexiconLMS.Client.Services
{
    public class ModuleDataService : IModuleDataService
    {
        private readonly HttpClient http;

		public ModuleDataService(HttpClient httpClient)
        {
            http = httpClient;
		}
        public async Task<Module?> GetModuleAsync(Guid moduleId)
        {
            HttpResponseMessage response = await http.GetAsync(UriHelper.GetModuleUri(moduleId.ToString()));
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true
                };

                string responseData = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(responseData))
                {
                    return JsonSerializer.Deserialize<Module>(responseData, options)!;
                }
            }

            return null;
        }

        public async Task<IEnumerable<Module>> GetModulesAsync()
        {
            HttpResponseMessage response = await http.GetAsync(UriHelper.GetModulesUri());
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true
                };

                string responseData = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(responseData))
                {
                    return JsonSerializer.Deserialize<IEnumerable<Module>>(responseData, options)!;
                }
            }

            return new List<Module>();
        }

        public async Task<bool> AddModule(Module module)
        {

            string moduleJson = JsonSerializer.Serialize(module);

            var httpContent = new StringContent(moduleJson, new MediaTypeHeaderValue(Application.Json));
            var response = await http.PostAsync(UriHelper.GetModulesUri(), httpContent);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
                return true;
            }
            return false;

        }

        public async Task<bool> UpdateModule(Module module)
        {

            string moduleJson = JsonSerializer.Serialize(module);

            var httpContent = new StringContent(moduleJson, new MediaTypeHeaderValue(Application.Json));
            var response = await http.PutAsync(UriHelper.GetModulesUri(), httpContent);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
                return true;
            }
            return false;

        }

        public async Task<bool> DeleteModule(Guid moduleId)
        {

			var response = await http.DeleteAsync(UriHelper.GetModuleUri(moduleId.ToString()));
			if (response.IsSuccessStatusCode)
			{
                return true;
			}
            return false;

		}
	}
}
