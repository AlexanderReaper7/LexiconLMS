using LexiconLMS.Shared.Dtos.ApplicationUserDtos;
using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Client.Services
{
    public interface IApplicationUserDataService
    {
        Task<List<ApplicationUser>> GetStudentsByCourseId(Guid Id);
        Task<bool> AddApplicationUser(ApplicationUserDtoAdd ApplicationUserDto);
        public Task<ApplicationUserDtoUpdate> GetUser(Guid Id);
        public Task<bool> UpdateUser(ApplicationUserDtoUpdate updatedUser);
        Task<bool> DeleteUser(Guid Id);
    }
}
