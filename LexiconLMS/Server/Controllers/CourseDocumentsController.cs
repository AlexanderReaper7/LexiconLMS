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
    public class CourseDocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseDocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CourseDocuments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDocument>>> GetCourseDocument()
        {
          if (_context.CourseDocument == null)
          {
              return NotFound();
          }
            return await _context.CourseDocument.ToListAsync();
        }

        // GET: api/CourseDocuments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDocument>> GetCourseDocument(Guid id)
        {
          if (_context.CourseDocument == null)
          {
              return NotFound();
          }
            var courseDocument = await _context.CourseDocument.FindAsync(id);

            if (courseDocument == null)
            {
                return NotFound();
            }

            return courseDocument;
        }

        // PUT: api/CourseDocuments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseDocument(Guid id, CourseDocument courseDocument)
        {
            if (id != courseDocument.Id)
            {
                return BadRequest();
            }

            _context.Entry(courseDocument).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseDocumentExists(id))
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

        // POST: api/CourseDocuments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseDocument>> PostCourseDocument(CourseDocument courseDocument)
        {
          if (_context.CourseDocument == null)
          {
              return Problem("Entity set 'ApplicationDbContext.CourseDocument'  is null.");
          }
            _context.CourseDocument.Add(courseDocument);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourseDocument", new { id = courseDocument.Id }, courseDocument);
        }

        // DELETE: api/CourseDocuments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseDocument(Guid id)
        {
            if (_context.CourseDocument == null)
            {
                return NotFound();
            }
            var courseDocument = await _context.CourseDocument.FindAsync(id);
            if (courseDocument == null)
            {
                return NotFound();
            }

            _context.CourseDocument.Remove(courseDocument);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseDocumentExists(Guid id)
        {
            return (_context.CourseDocument?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
