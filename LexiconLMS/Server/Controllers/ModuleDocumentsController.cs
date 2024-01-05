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
    public class ModuleDocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ModuleDocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ModuleDocuments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDocument>>> GetModuleDocument()
        {
          if (_context.ModuleDocument == null)
          {
              return NotFound();
          }
            return await _context.ModuleDocument.ToListAsync();
        }

		// GET: api/ModuleDocuments from Module
		[HttpGet("/moduledocumentsbymodule/{id}")]
		public async Task<ActionResult<IEnumerable<ModuleDocument>>> GetModuleDocuments(Guid id)
		{
			if (_context.ModuleDocument == null)
			{
				return NotFound();
			}
			return await _context.ModuleDocument.Where(m => m.ModuleId == id).ToListAsync();
		}

		// GET: api/ModuleDocuments/5
		[HttpGet("{id}")]
        public async Task<ActionResult<ModuleDocument>> GetModuleDocument(Guid id)
        {
          if (_context.ModuleDocument == null)
          {
              return NotFound();
          }
            var moduleDocument = await _context.ModuleDocument.FindAsync(id);

            if (moduleDocument == null)
            {
                return NotFound();
            }

            return moduleDocument;
        }

        // PUT: api/ModuleDocuments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModuleDocument(Guid id, ModuleDocument moduleDocument)
        {
            if (id != moduleDocument.Id)
            {
                return BadRequest();
            }

            _context.Entry(moduleDocument).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleDocumentExists(id))
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

        // POST: api/ModuleDocuments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ModuleDocument>> PostModuleDocument(ModuleDocument moduleDocument)
        {
          if (_context.ModuleDocument == null)
          {
              return Problem("Entity set 'ApplicationDbContext.ModuleDocument'  is null.");
          }
            _context.ModuleDocument.Add(moduleDocument);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModuleDocument", new { id = moduleDocument.Id }, moduleDocument);
        }

        // DELETE: api/ModuleDocuments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModuleDocument(Guid id)
        {
            if (_context.ModuleDocument == null)
            {
                return NotFound();
            }
            var moduleDocument = await _context.ModuleDocument.FindAsync(id);
            if (moduleDocument == null)
            {
                return NotFound();
            }

            _context.ModuleDocument.Remove(moduleDocument);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModuleDocumentExists(Guid id)
        {
            return (_context.ModuleDocument?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
