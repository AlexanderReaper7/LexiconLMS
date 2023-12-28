using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Client.Services
{
    public interface IModuleDataService
    {
        Task<bool> AddModule(Module module);
        Task<Module?> GetModuleAsync(Guid moduleId);
        Task<IEnumerable<Module>> GetModulesAsync();
        Task<bool> UpdateModule(Module module);
    }
}