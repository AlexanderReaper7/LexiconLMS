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
		private readonly IGenericDataService dataService;

		public ModuleDataService(IGenericDataService genericDataService)
		{
			dataService = genericDataService;
		}

		public async Task<Module?> GetAsync() => await dataService.GetAsync<Module>(UriHelper.GetModulesUri());
		public async Task<Module?> GetAsync(Guid id) => await dataService.GetAsync<Module>(UriHelper.GetModuleUri(id));

		public async Task<bool> AddAsync(Module module) => await dataService.AddAsync(UriHelper.GetModulesUri(), module);

		public async Task<bool> UpdateAsync(Module module) => await dataService.UpdateAsync(UriHelper.GetModuleUri(module.Id), module);

		public async Task<bool> DeleteAsync(Guid id) => await dataService.DeleteAsync(UriHelper.GetModuleUri(id));
	}
}
