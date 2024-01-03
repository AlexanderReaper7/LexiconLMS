﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LexiconLMS.Server.Data;
using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ActivitiesController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public ActivitiesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Activities
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

		// PUT: api/Activities/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutActivity(Guid id, Activity activity)
		{
			if (id != activity.Id)
			{
				return BadRequest();
			}

			_context.Entry(activity).State = EntityState.Modified;

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

			return NoContent();
		}

        // POST: api/Activities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Activity>> PostActivity(Activity activity)
        {
          if (_context.Activities == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Activities'  is null.");
          }
			_context.Entry(activity).State = EntityState.Unchanged;
			_context.Activities.Add(activity);
            try
            {
                await _context.SaveChangesAsync();
            }

            catch(Exception ex) { var exception = ex; }
            return CreatedAtAction("GetActivity", new { id = activity.Id }, activity);
        }

		// DELETE: api/Activities/5
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
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool ActivityExists(Guid id)
		{
			return (_context.Activities?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
