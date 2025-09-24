using Microsoft.AspNetCore.Mvc;
using Services.interfaces;
using BusinessObjects.Models;
using System;
using System.Threading.Tasks;

namespace AlphaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly IScoreServices _scoreService;

        public ScoreController(IScoreServices scoreService)
        {
            _scoreService = scoreService;
        }

        // POST: api/score
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Score score)
        {
            if (score == null)
                return BadRequest(new { Message = "Score data is required." });

            var exists = await _scoreService.ScoreExistsAsync(score.AssessmentId, score.StudentId);
            if (exists)
                return Conflict(new { Message = "Score already exists for this assessment and student." });

            await _scoreService.AddAsync(score); // dùng CRUD base
            return CreatedAtAction(nameof(GetById), new { id = score.Id }, score);
        }

        // GET: api/score/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var score = await _scoreService.GetByIdAsync(id); // CRUD base
            if (score == null)
                return NotFound(new { Message = "Score not found." });

            return Ok(score);
        }

        // PUT: api/score/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Score score)
        {
            if (score == null || id != score.Id)
                return BadRequest(new { Message = "Invalid score data." });

            var existing = await _scoreService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = "Score not found." });

            await _scoreService.UpdateAsync(score); // CRUD base
            return Ok(score);
        }

        // DELETE: api/score/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _scoreService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = "Score not found." });

            await _scoreService.DeleteAsync(id); // CRUD base
            return NoContent();
        }

        // GET: api/score/student/{studentId}
        [HttpGet("student/{studentId:guid}")]
        public async Task<IActionResult> GetByStudent(Guid studentId)
        {
            var scores = await _scoreService.GetScoresByStudentAsync(studentId);
            return Ok(scores);
        }

        // GET: api/score/student/{studentId}/subject/{subjectId}/term/{termId}
        [HttpGet("student/{studentId:guid}/subject/{subjectId:guid}/term/{termId:guid}")]
        public async Task<IActionResult> GetByStudentAndSubject(Guid studentId, Guid subjectId, Guid termId)
        {
            var scores = await _scoreService.GetScoresByStudentAndSubjectAsync(studentId, subjectId, termId);
            return Ok(scores);
        }

        // GET: api/score/class/{classId}/term/{termId}
        [HttpGet("class/{classId:guid}/term/{termId:guid}")]
        public async Task<IActionResult> GetByClassAndTerm(Guid classId, Guid termId)
        {
            var scores = await _scoreService.GetScoresByClassAndTermAsync(classId, termId);
            return Ok(scores);
        }

        // GET: api/score/student/{studentId}/subject/{subjectId}/term/{termId}/average
        [HttpGet("student/{studentId:guid}/subject/{subjectId:guid}/term/{termId:guid}/average")]
        public async Task<IActionResult> GetAverage(Guid studentId, Guid subjectId, Guid termId)
        {
            var avg = await _scoreService.GetAverageScoreByStudentAndSubjectAsync(studentId, subjectId, termId);
            return Ok(new { StudentId = studentId, SubjectId = subjectId, TermId = termId, Average = avg });
        }

        // GET: api/score/student/{studentId}/term/{termId}/transcript
        [HttpGet("student/{studentId:guid}/term/{termId:guid}/transcript")]
        public async Task<IActionResult> GetTranscript(Guid studentId, Guid termId)
        {
            var transcript = await _scoreService.GetTranscriptByStudentAsync(studentId, termId);
            return Ok(transcript);
        }
    }
}
