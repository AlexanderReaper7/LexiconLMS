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

        public async Task<bool> AddApplicationUser(ApplicationUserDtoAdd ApplicationUserDto)
        {
            return await _GService.AddAsync($"/applicationuser", ApplicationUserDto);
        }

        public async Task<bool> DeleteUser(Guid Id)
        {
            return await _GService.DeleteAsync($"api/ApplicationUser/{Id}");
        }

        public async Task<List<ApplicationUser>> GetStudentsByCourseId(Guid id) => await _GService.GetAsync<List<ApplicationUser>>($"applicationuserbycourse/{id}");

        public async Task<ApplicationUserDtoUpdate> GetUser(Guid Id)
        {
            return await _GService.GetAsync<ApplicationUserDtoUpdate>($"api/ApplicationUser/{Id}");
        }

        public async Task<bool> UpdateUser(ApplicationUserDtoUpdate updatedUser)
        {
            return await _GService.UpdateAsync($"api/ApplicationUser/{updatedUser.Id}", updatedUser);
        }
    }
}
