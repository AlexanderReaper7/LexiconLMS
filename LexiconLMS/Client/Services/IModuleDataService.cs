using LexiconLMS.Client.Helpers;
using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Client.Services
{
    public interface IModuleDataService
    {
		Task<Module?> GetAsync();
		Task<Module?> GetAsync(Guid id);
		Task<bool> AddAsync(Module module);
		Task<bool> UpdateAsync(Module module);
		Task<bool> DeleteAsync(Guid id);
	}
}