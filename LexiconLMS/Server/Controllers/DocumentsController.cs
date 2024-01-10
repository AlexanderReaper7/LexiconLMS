using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LexiconLMS.Server.Data;
using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using LexiconLMS.Shared.Dtos;

namespace LexiconLMS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public DocumentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
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

            var activityresponse =  await _context.Activities.FindAsync(id);

            if (activityresponse != null) 
            {
                document.ActivityId = id;
                return Ok(document);
            }
            

            var moduleresponse =  await _context.Modules.FindAsync(id);

            if (moduleresponse != null)
            {
                document.ModuleId = id;
                return Ok(document);
            }

            var courseresponse = await _context.Courses.FindAsync(id);

			if (courseresponse != null)
			{
                document.CourseId = id;
				return Ok(document);
			}
		
                return NotFound();
	
		}

        // GET: api/ActivityDocuments from acivity
        [HttpGet("/activitydocumentsbyactivity/{id}")]
        public async Task<ActionResult<IEnumerable<Document>>> GetActivityDocuments(Guid id, bool includeStudentsDocuments = true)
        {
            if (_context.Documents == null)
            {
                return NotFound();
            }

            var query = _context.Documents.Where(m => m.ActivityId == id);

            if (!includeStudentsDocuments)
            {
                query = query.Include(d => d.Uploader).Where(d => d.Uploader!.Roles.All(r => r.Name != StaticUserRoles.Student.ToString()));
            }

            return await query.ToListAsync();
        }

		[HttpGet("/moduledocumentsbymodule/{id}")]
		public async Task<ActionResult<IEnumerable<Document>>> GetModuleDocuments(Guid id)
		{
			if (_context.Documents == null)
			{
				return NotFound();
			}
			return await _context.Documents.Where(m => m.ModuleId == id).ToListAsync();
		}

		[HttpGet("/coursedocumentsbycourse/{id}")]
		public async Task<ActionResult<IEnumerable<Document>>> GetCourseDocuments(Guid id)
		{
			if (_context.Documents == null)
			{
				return NotFound();
			}
			return await _context.Documents.Where(m => m.CourseId == id).ToListAsync();
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
            document.UploaderId = userManager.GetUserId(User);
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
