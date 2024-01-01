using LexiconLMS.Shared.Dtos.ApplicationUserDtos;
using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Client.Services
{
    public interface IApplicationUserDataService
    {
        Task<List<ApplicationUser>> GetStudentsByCourseId(Guid Id);
        Task<bool> AddApplicationUser(ApplicationUserDto ApplicationUserDto);
    }
}
