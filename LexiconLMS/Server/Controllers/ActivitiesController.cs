using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LexiconLMS.Server.Data;
using LexiconLMS.Shared.Entities;
using LexiconLMS.Client.Pages;
using LexiconLMS.Shared.Dtos;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Identity;
using LexiconLMS.Shared.Dtos.ActivitiesDtos;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LexiconLMS.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ActivitiesController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper mapper;

		public ActivitiesController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			this.mapper = mapper;
		}

		// GET: api/Assignment
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Activity>>> GetActivities(Guid? moduleId = null)
		{
			if (_context.Activities == null)
			{
				return NotFound();
			}

			var query = _context.Activities.Include(a => a.Type).AsQueryable();

			if (moduleId != null)
			{
				query = query.Where(a => a.ModuleId == moduleId);
			}


			return await query.ToListAsync();
		}

		// GET: api/Modules
		[HttpGet("/activitiesbymodule/{id}")]
		public async Task<ActionResult<IEnumerable<Activity>>> GetActivities(Guid id)
		{
			if (_context.Activities == null)
			{
				return NotFound();
			}
			return await _context.Activities.Where(a => a.ModuleId == id).ToListAsync();
		}

		// GET: api/Activities/5
		[HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
          if (_context.Activities == null)
          {
              return NotFound();
          }

			var activity = await _context.Activities.Include(a => a.Type).FirstOrDefaultAsync(a => a.Id == id);


			if (activity == null)
			{
				return NotFound();
			}

			return activity;
		}

        [HttpGet("courses/{courseId}/modules/{moduleId}/assignments")]
        public async Task<ActionResult> GetAssignments(Guid courseId, Guid moduleId)
        {
            if (_context.Activities == null)
            {
                return NotFound();
            }

            DateTime now = DateTime.Now;

            var query = _context.Activities.Include(a => a.Type).Include(a => a.ActivityDocument)
                .Where(a => a.ModuleId == moduleId && a.Type.Name == "Assignment" && a.StartDate <= now)
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.StartDate,
                    a.EndDate,
                    a.ActivityDocument,
                    CourseId = courseId

                })
                .Join(_context.Users.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.Name == StaticUserRoles.Student.ToString())).DefaultIfEmpty(),
                    a => a.CourseId,
                    s => s.CourseId,
                    (a, s) => new AssignmentDtoForTeachers
                    {
                        StudentId = s.Id,
                        StudentName = s.FirstName + " " + s.LastName,
                        AssignmentId = a.Id,
                        AssignmentName = a.Name,

                        SubmissionState = a.ActivityDocument!.Any(d => d.UploaderId == s.Id) ?
						a.EndDate >= a.ActivityDocument!.Min(d=>d.UploadDate) ? SubmissionState.Submitted : SubmissionState.SubmittedLate
						: a.EndDate >= now ? SubmissionState.NotSubmitted : SubmissionState.Late,

                        DueDate = a.EndDate
                    }
                );

            var assignments = await query.ToListAsync();

            return Ok(assignments);
        }

		[HttpGet("students/assignments/{assignmentId}")]
		public async Task<ActionResult> GetAssignments(Guid assignmentId)
		{
			if (_context.Activities == null)
			{
				return NotFound();
			}

			DateTime now = DateTime.Now;

			var query = _context.Activities.Include(a => a.Type).Include(a => a.ActivityDocument).Include(a => a.Module)
				.Where(a => a.Id == assignmentId && a.Type.Name == "Assignment")
				.Select(a => new
				{
					a.Id,
					a.Name,
					a.StartDate,
					a.EndDate,
					a.ActivityDocument,
					a.Module!.CourseId
				})
				.Join(_context.Users.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.Name == StaticUserRoles.Student.ToString())).DefaultIfEmpty(),
					a => a.CourseId,
					s => s.CourseId,

					(a, s) => new AssignmentDtoStudentAndStatusOnly
					{
						StudentId = s.Id,
						StudentName = s.FirstName + " " + s.LastName,

						SubmissionState = a.ActivityDocument!.Any(d => d.UploaderId == s.Id) ?
						a.EndDate >= a.ActivityDocument!.Min(d => d.UploadDate) ? SubmissionState.Submitted : SubmissionState.SubmittedLate
						: a.EndDate >= now ? SubmissionState.NotSubmitted : SubmissionState.Late
					}
				);

			//new AssignmentDtoForTeachers
			//{
			//	StudentId = s.Id,
			//	StudentName = s.FirstName + " " + s.LastName,
			//	AssignmentId = a.Id,
			//	AssignmentName = a.Name,

			//	SubmissionState = a.ActivityDocument!.Any(d => d.UploaderId == s.Id) ?
			//			a.EndDate >= a.ActivityDocument!.Min(d => d.UploadDate) ? SubmissionState.Submitted : SubmissionState.SubmittedLate
			//			: a.EndDate >= now ? SubmissionState.NotSubmitted : SubmissionState.Late,

			//	DueDate = a.EndDate
			//}

			var assignments = await query.ToListAsync();

			return Ok(assignments);
		}

		[HttpGet("students/{studentId}/modules/{moduleId}/assignments")]
        public async Task<ActionResult> GetAssignments(string studentId, Guid moduleId)
        {
            if (_context.Activities == null)
            {
                return NotFound();
            }

			DateTime now = DateTime.Now;

            var query = _context.Activities.Include(a => a.Type).Include(a => a.ActivityDocument)
                .Where(a => a.ModuleId == moduleId && a.Type.Name == "Assignment" && a.StartDate <= now)
                .Select(a => new AssignmentDtoForStudents
                {
                    AssignmentId = a.Id,
                    AssignmentName = a.Name,

					SubmissionState = a.ActivityDocument!.Any(d => d.UploaderId == studentId)? 
					a.EndDate >= a.ActivityDocument!.Min(d => d.UploadDate) ? SubmissionState.Submitted : SubmissionState.SubmittedLate
					: a.EndDate >= now ? SubmissionState.NotSubmitted : SubmissionState.Late,

					DueDate = a.EndDate
					
                });

            var assignments = await query.ToListAsync();

            return Ok(assignments);
        }

        [HttpGet("students/{studentId}/assignments/{assignmentId}")]
        public async Task<ActionResult> GetAssignment(string studentId, Guid assignmentId)
        {
            if (_context.Activities == null)
            {
                return NotFound();
            }


            var query = _context.Activities.Include(a => a.Type).Include(a => a.ActivityDocument)
                .Where(a => a.Id == assignmentId && a.Type.Name == "Assignment")
                .Select(a =>
					a.ActivityDocument!.Where(d => d.UploaderId == studentId).Cast<Document>().ToList()
                );

            var assignment = await query.FirstOrDefaultAsync();

			if (assignment == null)
			{
                return NotFound();
            }

            return Ok(assignment);
        }

		[HttpGet("assignments/{assignmentId}")]
		public async Task<ActionResult> GetAssignment(Guid assignmentId)
		{
			if (_context.Activities == null)
			{
				return NotFound();
			}

			DateTime now = DateTime.Now;

			var query = _context.Activities.Include(a => a.Type).Include(a => a.ActivityDocument)
				.Where(a => a.Id == assignmentId && a.Type.Name == "Assignment");

			var activity = await query.FirstOrDefaultAsync();

			if (activity == null)
			{
				return NotFound();
			}

			var assignment = mapper.Map<AssignmentDto>(activity);

			return Ok(assignment);
		}


		// PUT: api/Assignment/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutActivity(Guid id, Activity activity)
		{
			bool Verification = true;
			
			if (id != activity.Id)
			{
				return BadRequest();
			}


			var moduleInQuery = _context.Modules.Find(activity.ModuleId);
			var activitiesInQuery = _context.Activities.Where(a => a.ModuleId == moduleInQuery.Id).AsNoTracking();

			if (activity.StartDate > activity.EndDate) { Verification = false; }
			if (activity.StartDate < moduleInQuery.StartDate || activity.EndDate > moduleInQuery.EndDate) { Verification = false; }

			foreach (var item in activitiesInQuery)
			{
				if (activity.StartDate > item.StartDate && activity.StartDate < item.EndDate) { Verification = false; }
				if (activity.StartDate < item.StartDate && activity.EndDate > item.StartDate) { Verification = false; }
			}

			try
			{
				_context.Entry(activity).State = EntityState.Modified;
			}
			catch (Exception ex) { Console.Write(ex.ToString()); }
			

			if (Verification)
			{
				try
				{
					await _context.SaveChangesAsync();
				}

				catch (DbUpdateConcurrencyException)
				{
					if (!ActivityExists(id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
			}
			

			return NoContent();
		}

		// POST: api/Assignment
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Activity>> PostActivity(Activity activity)
		{

			bool Verification = true;

			if (_context.Activities == null)
			{
				return Problem("Entity set 'ApplicationDbContext.Activities'  is null.");
			}

			var moduleInQuery = _context.Modules.Find(activity.ModuleId);
			var activitiesInQuery = _context.Activities.Where(a => a.ModuleId == moduleInQuery.Id);

			if (activity.StartDate > activity.EndDate) { Verification = false; }
			if (activity.StartDate < moduleInQuery.StartDate || activity.EndDate > moduleInQuery.EndDate) { Verification = false; }

			foreach (var item in activitiesInQuery)
			{
				if (activity.StartDate > item.StartDate && activity.StartDate < item.EndDate) { Verification = false; }
				if (activity.StartDate < item.StartDate && activity.EndDate > item.StartDate) { Verification = false; }
			}

			if (Verification)
			{
				_context.Entry(activity).State = EntityState.Unchanged;
				_context.Activities.Add(activity);

				try
				{

					await _context.SaveChangesAsync();
				}
				catch (Exception ex) { Console.Write(ex.ToString()); }

				return CreatedAtAction("GetActivity", new { id = activity.Id }, activity);
			}
			return Problem("overlapping activities or out of bounds of module.");
		}

		// DELETE: api/Assignment/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteActivity(Guid id)
		{
			if (_context.Activities == null)
			{
				return NotFound();
			}
			var activity = await _context.Activities.FindAsync(id);
			if (activity == null)
			{
				return NotFound();
			}
			
			_context.Activities.Remove(activity);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch(Exception message) 
			{
				Console.WriteLine($"Exception message: {message.Message}");
			}

			return NoContent();
		}

		private bool ActivityExists(Guid id)
		{
			return (_context.Activities?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
