using LexiconLMS.Server.Data;
using LexiconLMS.Shared.Dtos;
using LexiconLMS.Shared.Dtos.ApplicationUserDtos;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LexiconLMS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/ApplicationUser
        [HttpGet("/applicationuserbycourse/{id}")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetApplicationUseres(Guid id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
             return await _context.Users.Where(m => m.Course.Id == id).ToListAsync();
        }

        // POST: api/ApplicationUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Teacher")]
        [HttpPost("/applicationuser")]
        public async Task<ActionResult<ApplicationUserDtoAdd>> PostApplicationUserDto(ApplicationUserDtoAdd ApplicationUserDto)
        {
            var isExist = await _userManager.FindByNameAsync(ApplicationUserDto.Email);
            if (isExist is not null)
                return BadRequest("UserName already exists");

            var course = _context.Courses.Where(c => c.Id == ApplicationUserDto.Course.Id).FirstOrDefault();
            ApplicationUser newUser = new ApplicationUser()
            {
                UserName = ApplicationUserDto.Email,
                Email = ApplicationUserDto.Email,
                FirstName = ApplicationUserDto.FirstName,
                LastName = ApplicationUserDto.LastName,
                Course = course != null? course: ApplicationUserDto.Course,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createUserResult = await _userManager.CreateAsync(newUser, ApplicationUserDto.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User creation failed because: ";

                foreach (var error in createUserResult.Errors)
                {
                    errorString += $" # {error.Description}";
                }
                return BadRequest(errorString);
            }

            //await _userManager.AddToRoleAsync(newUser, Enum.GetName(typeof(StaticUserRoles), 1));
            await _userManager.AddToRoleAsync(newUser, ApplicationUserDto.Role);

            return Ok("User created successfully");
        }

        // GET: api/applicationuser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUserDto>> GetApplicationUser(Guid id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            
            var user = await _context.Users.Where(u => u.Id == id.ToString()).Include(u => u.Course).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            ApplicationUserDto applicationUserDto = new ApplicationUserDto()
            {
                Id = Guid.Parse(user.Id),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Course = user.Course,
                Role = roles.Count !=0 ? roles.ElementAt(0) : "Student"
            };

            return applicationUserDto;
        }

        [Authorize(Roles = "Teacher")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateApplicationUser(Guid id, ApplicationUserDtoUpdate updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users.AsNoTracking().Where(u => u.Id == id.ToString()).Include(u => u.Course).FirstOrDefaultAsync();
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.Course = updatedUser.Course;
            user.UserName = updatedUser.Email;

            _context.Entry(user.Course).State = EntityState.Unchanged;
            _context.Entry(user).State = EntityState.Detached;
            _context.Set<ApplicationUser>().Update(user);
            _context.SaveChanges();

            if(updatedUser.OldRole != updatedUser.Role)
            {
                await _userManager.RemoveFromRoleAsync(user, updatedUser.OldRole);
                await _userManager.AddToRoleAsync(user, updatedUser.Role);
            }
            
            return Ok();
        }

        [Authorize(Roles = "Teacher")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUser(Guid id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return Ok();
        }

        private bool UserExists(string id)
        {
            return (_context.Users?.Any(u => u.Id == id)).GetValueOrDefault();
        }

    }
}
