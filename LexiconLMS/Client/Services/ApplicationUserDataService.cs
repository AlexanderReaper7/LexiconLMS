using LexiconLMS.Client.Helpers;
using LexiconLMS.Shared.Dtos.ApplicationUserDtos;
using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Client.Services
{
    public class ApplicationUserDataService : IApplicationUserDataService
    {
        private IGenericDataService _GService;

        public ApplicationUserDataService(HttpClient httpClient, IGenericDataService GService)
        {
            _GService = GService;
        }

        public async Task<bool> AddApplicationUser(ApplicationUserDto ApplicationUserDto)
        {
            return await _GService.AddAsync($"/applicationuser", ApplicationUserDto);
        }

        public async Task<List<ApplicationUser>> GetStudentsByCourseId(Guid id) => await _GService.GetAsync<List<ApplicationUser>>($"applicationuserbycourse/{id}");
    }
}
