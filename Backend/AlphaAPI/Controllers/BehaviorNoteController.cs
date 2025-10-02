using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.interfaces;

namespace AlphaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BehaviorNoteController : ControllerBase
    {
        private readonly IBehaviorNoteServices _service;

        public BehaviorNoteController(IBehaviorNoteServices service)
        {
            _service = service;
        }

        // POST: api/BehaviorNote
        [HttpPost]
        public async Task<IActionResult> AddNote([FromBody] BehaviorNote note)
        {
            if (note == null) return BadRequest("Note cannot be null");
            var result = await _service.AddNoteAsync(note);
            return Ok(result);
        }

        // GET: api/BehaviorNote/student/{studentId}/{termId}
        [HttpGet("student/{studentId}/{termId}")]
        public async Task<IActionResult> GetNotesByStudent(Guid studentId, Guid termId)
        {
            var notes = await _service.GetNotesByStudentAsync(studentId, termId);
            return Ok(notes);
        }

        // GET: api/BehaviorNote/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetAllNotesByStudent(Guid studentId)
        {
            var notes = await _service.GetAllNotesByStudentAsync(studentId);
            return Ok(notes);
        }

        // GET: api/BehaviorNote/class/{classId}/{termId}
        [HttpGet("class/{classId}/{termId}")]
        public async Task<IActionResult> GetNotesByClass(Guid classId, Guid termId)
        {
            var notes = await _service.GetNotesByClassAsync(classId, termId);
            return Ok(notes);
        }

        // GET: api/BehaviorNote/teacher/{teacherId}/{termId}
        [HttpGet("teacher/{teacherId}/{termId}")]
        public async Task<IActionResult> GetNotesByTeacher(Guid teacherId, Guid termId)
        {
            var notes = await _service.GetNotesByTeacherAsync(teacherId, termId);
            return Ok(notes);
        }

        // PUT: api/BehaviorNote/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] BehaviorNote updatedNote)
        {
            if (updatedNote == null || updatedNote.Id != id)
                return BadRequest("Invalid request");

            var success = await _service.UpdateNoteAsync(updatedNote);
            if (!success) return NotFound("Note not found");

            return Ok("Note updated successfully");
        }

        // DELETE: api/BehaviorNote/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            var success = await _service.DeleteNoteAsync(id);
            if (!success) return NotFound("Note not found");

            return Ok("Note deleted successfully");
        }
    }
}
