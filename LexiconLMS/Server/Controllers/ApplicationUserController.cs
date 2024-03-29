﻿using LexiconLMS.Server.Data;
using LexiconLMS.Server.Services;
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
        private readonly IMailService _mailService;

        private MailData _mailData;

        public ApplicationUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMailService mailService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mailService = mailService;
            _mailData = new MailData();
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

            var course = _context.Courses.Where(c => c.Id == ApplicationUserDto.CourseID).FirstOrDefault();
            ApplicationUser newUser = new ApplicationUser()
            {
                UserName = ApplicationUserDto.Email,
                Email = ApplicationUserDto.Email,
                FirstName = ApplicationUserDto.FirstName,
                LastName = ApplicationUserDto.LastName,
                Course = course,
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

            _mailData.EmailTo = newUser.Email;
            _mailData.EmailToName = $"{newUser.FirstName} {newUser.LastName}";
            _mailData.EmailSubject = "User created successfully";
            _mailData.EmailBody = $"You have been added to {course.Name} course as {ApplicationUserDto.Role}.";

            _mailService.SendMail(_mailData);

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

            var user = await _context.Users.Where(u => u.Id == id.ToString())
                .Include(u => u.Course)
                .FirstOrDefaultAsync();

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
                CourseID = user.Course.Id,
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

            var course = _context.Courses.Where(c => c.Id == updatedUser.CourseID).FirstOrDefault();

            var user = await _context.Users.AsNoTracking().Where(u => u.Id == id.ToString()).Include(u => u.Course).FirstOrDefaultAsync();
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.Course = course;
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

            _mailData.EmailTo = user.Email;
            _mailData.EmailToName = $"{user.FirstName} {user.LastName}";
            _mailData.EmailSubject = "Update user data";
            _mailData.EmailBody = $"Your data has been successfully updated.";

            _mailService.SendMail(_mailData);

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

            _mailData.EmailTo = user.Email;
            _mailData.EmailToName = $"{user.FirstName} {user.LastName}";
            _mailData.EmailSubject = "Remove user data";
            _mailData.EmailBody = $"You have been removed from {user.Course.Name} course.";

            _mailService.SendMail(_mailData);

            return Ok();
        }

        private bool UserExists(string id)
        {
            return (_context.Users?.Any(u => u.Id == id)).GetValueOrDefault();
        }

    }
}
