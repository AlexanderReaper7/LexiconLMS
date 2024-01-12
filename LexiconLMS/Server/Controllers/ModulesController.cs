using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LexiconLMS.Server.Data;
using LexiconLMS.Shared.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;
using LexiconLMS.Shared.Dtos.ModulesDtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using LexiconLMS.Shared.Dtos;
using System.Diagnostics;

namespace LexiconLMS.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ModulesController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper map;

		public ModulesController(ApplicationDbContext context, IMapper map)
		{
			_context = context;
			this.map = map;
		}

		// GET: api/Modules
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Module>>> GetModules()
		{
			if (_context.Modules == null)
			{
				return NotFound();
			}
			return await _context.Modules.ToListAsync();
		}

        // GET: api/Modules
        [HttpGet("/modulesbycourse/{id}")]
        public async Task<ActionResult<IEnumerable<Module>>> GetModules(Guid id)
        {
            if (_context.Modules == null)
            {
                return NotFound();
            }
            return await _context.Modules.Where(m => m.CourseId == id).ToListAsync();
        }

        // GET: api/Modules/5
        [HttpGet("{id}")]
		public async Task<ActionResult<Module>> GetModule(Guid id, bool includeActivities = false)
		{
			if (_context.Modules == null)
			{
				return NotFound();
			}
			var query = _context.Modules.AsQueryable();

			if (includeActivities)
			{
			//	query = query.Include(m => m.Assignment)!.ThenInclude(a => a.Type);
			}
			var @module = await query.FirstOrDefaultAsync(m=> m.Id == id);

			if (@module == null)
			{
				return NotFound();
			}
			//var m = map.Map<ModuleDto>(@module);
			return @module;
		}

		// PUT: api/Modules/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> PutModule(Guid id, Module @module)
		{
			bool Verification = true;
			if (id != @module.Id)
			{
				return BadRequest();
			}

			var courseInQuery = _context.Courses.Find(module.CourseId);
			var modulesInQuery = _context.Modules.Where(m => m.CourseId == courseInQuery.Id).AsNoTracking();

			if (module.StartDate > module.EndDate) { Verification = false; }
			if (module.StartDate < courseInQuery.StartDate || module.EndDate > courseInQuery.EndDate) { Verification = false; }

			foreach (var item in modulesInQuery)
				if (item.Id != id)
				{
					{
						if (module.StartDate > item.StartDate && module.StartDate < item.EndDate) { Verification = false; }
						if (module.StartDate < item.StartDate && module.EndDate > item.StartDate) { Verification = false; }
					}
				}

			_context.Entry(@module).State = EntityState.Modified;

			if (Verification)
			{
				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ModuleExists(id))
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

		// POST: api/Modules
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<Module>> PostModule(Module @module)
		{
			bool Verification = true;
			if (_context.Modules == null)
			{
				return Problem("Entity set 'ApplicationDbContext.Modules'  is null.");
			}
			var courseInQuery = _context.Courses.Find(module.CourseId);
			var modulesInQuery = _context.Modules.Where(m => m.CourseId == courseInQuery.Id);

			if (module.StartDate > module.EndDate) { Verification = false; }
			if (module.StartDate < courseInQuery.StartDate || module.EndDate > courseInQuery.EndDate) { Verification = false; }

			foreach (var item in modulesInQuery)
			
				{
				if (module.StartDate > item.StartDate && module.StartDate < item.EndDate) { Verification = false; }
				if (module.StartDate < item.StartDate && module.EndDate > item.StartDate) { Verification = false; }
				}

			if (Verification)
			{
				_context.Modules.Add(@module);
				await _context.SaveChangesAsync();
				return CreatedAtAction("GetModule", new { id = @module.Id }, @module);
			}

			return Problem("overlapping modules or out of bounds of Course.");
		}

		// DELETE: api/Modules/5
		[HttpDelete("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteModule(Guid id)
		{
			if (_context.Modules == null)
			{
				return NotFound();
			}
			var @module = await _context.Modules.FindAsync(id);
			if (@module == null)
			{
				return NotFound();
			}

			var documents = _context.Documents.Where(d => d.ModuleId == id);

			_context.Documents.RemoveRange(documents);
		

			_context.Modules.Remove(@module);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool ModuleExists(Guid id)
		{
			return (_context.Modules?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
