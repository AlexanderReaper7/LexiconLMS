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
    public class DocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocument()
        {
            if (_context.Document == null)
            {
                return NotFound();
            }
            return await _context.Document.ToListAsync();
        }

		//// GET: api/ActivityDocuments from acivity
		//[HttpGet("/coursedocumentsbycourseyactivity/{id}")]
		//public async Task<ActionResult<IEnumerable<CourseDocument>>> GetCourseDocuments(Guid id)
		//{
		//	if (_context.CourseDocument == null)
		//	{
		//		return NotFound();
		//	}
		//	return await _context.CourseDocument.Where(m => m.CourseId == id).ToListAsync();
		//}

		// GET: api/Documents/5
		[HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(Guid id)
        {
            if (_context.Document == null)
            {
                return NotFound();
            }
            var document = await _context.Document.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            return document;
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
            if (_context.Document == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Document'  is null.");
            }
            _context.Document.Add(document);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocument", new { id = document.Id }, document);
        }

        // DELETE: api/Documents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            if (_context.Document == null)
            {
                return NotFound();
            }
            var document = await _context.Document.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            _context.Document.Remove(document);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentExists(Guid id)
        {
            return (_context.Document?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}