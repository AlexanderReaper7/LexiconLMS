using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LexiconLMS.Server.Data;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LexiconLMS.Shared.Dtos;
using LexiconLMS.Server.Services;

namespace LexiconLMS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMailService _mailService;
        private MailData _mailData;

        public CoursesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMailService mailService)
        {
            _context = context;
            _userManager = userManager;
            _mailService = mailService;
            _mailData = new MailData();
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }

            return await _context.Courses.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(Guid id)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        /// <summary>
        /// Gets the course that the requester is a member of
        /// </summary>
        [HttpGet("mycourse")]
        public async Task<ActionResult<Course>> GetMyCourse()
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User)!;
            // Get the course that the user is a member of
            var course = await _context.Courses.Where(c => c.Users.Any(u => u.Id == userId)).FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Teacher")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(Guid id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                var users = await _context.Users.Where(u => u.CourseId == id).ToListAsync();
                foreach (var user in users)
                {
                    _mailData.EmailTo = user.Email;
                    _mailData.EmailToName = $"{user.FirstName} {user.LastName}";
                    _mailData.EmailSubject = "Course updated";
                    _mailData.EmailBody = $"{course.Name} course has been updated.";

                    _mailService.SendMail(_mailData);
                }
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                    throw;
                }
            }
                       
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
            }
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
            {
                _mailData.EmailTo = user.Email;
                _mailData.EmailToName = $"{user.FirstName} {user.LastName}";
                _mailData.EmailSubject = "New course added";
                _mailData.EmailBody = $"New {course.Name} course has been added.";

                _mailService.SendMail(_mailData);
            }

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        [Authorize(Roles = "Teacher")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return Ok();
        }

        private bool CourseExists(Guid id)
        {
            return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
