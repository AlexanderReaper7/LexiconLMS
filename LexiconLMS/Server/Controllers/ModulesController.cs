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

namespace LexiconLMS.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
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
			//	query = query.Include(m => m.Activities)!.ThenInclude(a => a.Type);
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
		public async Task<IActionResult> PutModule(Guid id, Module @module)
		{
			if (id != @module.Id)
			{
				return BadRequest();
			}

			_context.Entry(@module).State = EntityState.Modified;

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

			return NoContent();
		}

		// POST: api/Modules
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Module>> PostModule(Module @module)
		{
			if (_context.Modules == null)
			{
				return Problem("Entity set 'ApplicationDbContext.Modules'  is null.");
			}
			_context.Modules.Add(@module);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetModule", new { id = @module.Id }, @module);
		}

		// DELETE: api/Modules/5
		[HttpDelete("{id}")]
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
