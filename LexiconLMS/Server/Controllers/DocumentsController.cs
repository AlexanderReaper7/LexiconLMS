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
    public class DocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
        {
          if (_context.Documents == null)
          {
              return NotFound();
          }
            return await _context.Documents.ToListAsync();
        }

        // GET: api/Documents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(Guid id)
        {
          if (_context.Documents == null)
          {
              return NotFound();
          }
            var document = await _context.Documents.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            return document;
        }

		// GET: api/Documents/5
		[HttpGet("/documentsetfk/{id}")]
		public async Task<ActionResult<Document>> GetDocumentSetFK(Guid id)
		{
            var document = new Document();
            if (_context.Documents == null)
			{
				return NotFound();
			}

            var activityresponse = await _context.Activities.FirstOrDefaultAsync(p => p.Id == id);

			if (activityresponse != null)
            {
                document.ActivityId = id;
                return Ok(document);
            }

			var moduleresponse = await _context.Modules.FirstOrDefaultAsync(p => p.Id == id);
			
            if (moduleresponse != null)
			{
                document.ModuleId = id;
				return Ok(moduleresponse);
			}

			var courseresponse = await _context.Documents.FirstOrDefaultAsync(p => p.CourseId == id);

			if (moduleresponse != null)
			{
                document.CourseId = id;
				return Ok(document);
			}
		
                return NotFound();
	
		}

        // GET: api/ActivityDocuments from acivity
        [HttpGet("/activitydocumentsbyactivity/{id}")]
        public async Task<ActionResult<IEnumerable<Document>>> GetActivityDocuments(Guid id)
        {
            if (_context.Documents == null)
            {
                return NotFound();
            }
            return await _context.Documents.Where(m => m.ActivityId == id).ToListAsync();
        }


        // PUT: api/Documents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument(Guid id, Document document)
        {
            if (id != document.Id)
            {
                return BadRequest();
            }

            _context.Entry(document).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
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

        // POST: api/Documents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Document>> PostDocument(Document document)
        {
          if (_context.Documents == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Documents'  is null.");
          }
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocument", new { id = document.Id }, document);
        }

        // DELETE: api/Documents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            if (_context.Documents == null)
            {
                return NotFound();
            }
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentExists(Guid id)
        {
            return (_context.Documents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
