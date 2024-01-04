using LexiconLMS.Server.Data;
using LexiconLMS.Shared.Dtos;
using LexiconLMS.Shared.Dtos.ApplicationUserDtos;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LexiconLMS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpPost("/applicationuser")]
        public async Task<ActionResult<ApplicationUserDto>> PostApplicationUserDto(ApplicationUserDto ApplicationUserDto)
        {
            var isExist = await _userManager.FindByNameAsync(ApplicationUserDto.UserName);
            if (isExist is not null)
                return BadRequest("UserName already exists");

            var course = _context.Courses.Where(c => c.Id == ApplicationUserDto.Course.Id).FirstOrDefault();
            ApplicationUser newUser = new ApplicationUser()
            {
                UserName = ApplicationUserDto.UserName,
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

            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.Student);

            return Ok("User created successfully");
        }

        // GET: api/applicationuser/5
        [HttpGet("byname/{name}")]
        public async Task<ActionResult<ApplicationUser>> GetApplicationUser(string name)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == name);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }
}
