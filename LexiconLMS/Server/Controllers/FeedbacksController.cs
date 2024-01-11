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
using System.Diagnostics;

namespace LexiconLMS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeedbacksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("students/{studentId}/assignments/{assignmentId}/feedback")]
        public async Task<ActionResult<FeedbackDto>> GetFeedback(string studentId, Guid assignmentId)
        {

            if (_context.Feedbacks == null)
            {
                return NotFound();
            }

            var query = _context.Feedbacks.Include(x => x.Teacher)
                .Where(f => f.StudentId == studentId && f.AssignmentId == assignmentId)
                .Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    Message = f.Message,
                    TeacherId = f.TeacherId,
                    TeacherName = f.Teacher!.Fullname,
                });

            var feedback = await query.FirstOrDefaultAsync();

            if (feedback == null)
            {
                return NotFound();
            }

			return feedback;
		}

		// GET: api/Feedback/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Feedback>> GetFeedback(Guid id)
		{
			if (_context.Feedbacks == null)
			{
				return NotFound();
			}

			var feedbacks = await _context.Feedbacks.FirstOrDefaultAsync(a => a.Id == id);


			if (feedbacks == null)
			{
				return NotFound();
			}

			return feedbacks;
		}

		// PUT: api/Feedbacks/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(Guid id, FeedbackDto feedbackDto)
        {
            if (id != feedbackDto.Id)
            {
                return BadRequest();
            }
            Feedback? feedback = await _context.Feedbacks.FirstOrDefaultAsync(f=> f.Id == feedbackDto.Id);

            if (feedback == null)
            {
                return NotFound();
            }

            feedback.Message = feedbackDto.Message;


			_context.Entry(feedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
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

        // POST: api/Feedbacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Feedback>> PostFeedback(Feedback feedback)
        {
          if (_context.Feedbacks == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Feedbacks'  is null.");
          }
			try
            {
				_context.Entry(feedback).State = EntityState.Unchanged;
				_context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
            }

            return CreatedAtAction("GetFeedback", new { id = feedback.Id }, feedback);
        }

        // DELETE: api/Feedbacks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            if (_context.Feedbacks == null)
            {
                return NotFound();
            }
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FeedbackExists(Guid id)
        {
            return (_context.Feedbacks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
