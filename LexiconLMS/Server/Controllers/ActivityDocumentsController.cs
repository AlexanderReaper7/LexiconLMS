using System;
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
    public class ActivityDocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ActivityDocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

	
		// GET: api/ActivityDocuments
		[HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityDocument>>> GetActivityDocument()
        {
          if (_context.ActivityDocument == null)
          {
              return NotFound();
          }
            return await _context.ActivityDocument.ToListAsync();
        }

        // GET: api/ActivityDocuments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDocument>> GetActivityDocument(Guid id)
        {
          if (_context.ActivityDocument == null)
          {
              return NotFound();
          }
            var activityDocument = await _context.ActivityDocument.FindAsync(id);

            if (activityDocument == null)
            {
                return NotFound();
            }

            return activityDocument;
        }

		// GET: api/Modules
		[HttpGet("/activitydocumentsbyactivity/{id}")]
		public async Task<ActionResult<IEnumerable<ActivityDocument>>> GetActivityDocuments(Guid id)
		{
			if (_context.ActivityDocument == null)
			{
				return NotFound();
			}
			return await _context.ActivityDocument.Where(m => m.ActivityId == id).ToListAsync();
		}


		// PUT: api/ActivityDocuments/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
        public async Task<IActionResult> PutActivityDocument(Guid id, ActivityDocument activityDocument)
        {
            if (id != activityDocument.Id)
            {
                return BadRequest();
            }

            _context.Entry(activityDocument).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityDocumentExists(id))
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

        // POST: api/ActivityDocuments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ActivityDocument>> PostActivityDocument(ActivityDocument activityDocument)
        {
          if (_context.ActivityDocument == null)
          {
              return Problem("Entity set 'ApplicationDbContext.ActivityDocument'  is null.");
          }
            _context.ActivityDocument.Add(activityDocument);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActivityDocument", new { id = activityDocument.Id }, activityDocument);
        }

        // DELETE: api/ActivityDocuments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivityDocument(Guid id)
        {
            if (_context.ActivityDocument == null)
            {
                return NotFound();
            }
            var activityDocument = await _context.ActivityDocument.FindAsync(id);
            if (activityDocument == null)
            {
                return NotFound();
            }

            _context.ActivityDocument.Remove(activityDocument);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActivityDocumentExists(Guid id)
        {
            return (_context.ActivityDocument?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
