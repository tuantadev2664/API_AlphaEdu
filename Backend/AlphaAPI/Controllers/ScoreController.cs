using BusinessObjects.Models;
using DataAccessObjects.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.interfaces;
using Services.services;
using System;
using System.Threading.Tasks;

namespace AlphaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly IScoreServices _scoreService;
        private readonly IAnalyticsServices _analyticsService;
        private readonly IBehaviorNoteServices _behaviorNoteService;

        public ScoreController(IScoreServices scoreService, IAnalyticsServices analyticsService, IBehaviorNoteServices behaviorNoteService)
        {
            _scoreService = scoreService;
            _analyticsService = analyticsService;
            _behaviorNoteService = behaviorNoteService;
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
        public async Task<IActionResult> Update(Guid id, [FromBody] ScoreUpdateDto dto)
        {
            if (dto == null || id != dto.Id)
                return BadRequest(new { Message = "Invalid score data." });

            var existing = await _scoreService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = "Score not found." });

            // Cập nhật các field cần thiết
            existing.Score1 = dto.Score1;
            existing.IsAbsent = dto.IsAbsent ?? false;
            existing.Comment = dto.Comment;
            existing.UpdatedAt = DateTime.UtcNow;

            await _scoreService.UpdateAsync(existing);
            return Ok(existing);
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

        // GET: api/score/student/{studentId}/term/{termId}/analysis
        [HttpGet("student/{studentId:guid}/term/{termId:guid}/analysis")]
        public async Task<IActionResult> AnalyzeStudent(Guid studentId, Guid termId)
        {
            // Gọi AnalyticsService để phân tích học sinh
            var analysis = await _analyticsService.AnalyzeStudentAsync(studentId, termId);

            if (analysis == null)
                return NotFound(new { Message = "No analysis found for this student in this term." });

            return Ok(analysis);
        }

        [HttpPost("save-and-analyze")]
        public async Task<IActionResult> SaveAndAnalyze([FromBody] Score score)
        {
            if (score == null)
                return BadRequest(new { Message = "Score data is required." });

            // Kiểm tra điểm đã tồn tại
            var exists = await _scoreService.ScoreExistsAsync(score.AssessmentId, score.StudentId);

            if (exists)
            {
                // Nếu đã có, cập nhật điểm
                await _scoreService.UpdateAsync(score);
            }
            else
            {
                // Nếu chưa có, thêm mới
                await _scoreService.AddAsync(score);
            }

            // Gọi AnalyticsService để phân tích học sinh
            var termId = score.Assessment.GradeComponent.TermId;
            var analysis = await _analyticsService.AnalyzeStudentAsync(score.StudentId, termId);

            return Ok(new
            {
                Message = "Score saved successfully",
                Score = score,
                Analysis = analysis 
            });
        }


        [HttpGet("class/{classId:guid}/subject/{subjectId:guid}/term/{termId:guid}/scores")]
        [Authorize(Roles = "teacher,admin")]
        public async Task<IActionResult> GetScoresByClassAndSubject(Guid classId, Guid subjectId, Guid termId)
        {
            var result = await _scoreService.GetStudentScoresByClassAndSubjectAsync(classId, subjectId, termId);
            return Ok(result);
        }

        [HttpGet("class/{classId:guid}/term/{termId:guid}/ranking")]
        [Authorize(Roles = "teacher,admin")]
        public async Task<IActionResult> GetClassRanking(Guid classId, Guid termId)
        {
            var ranking = await _scoreService.GetClassRankingAsync(classId, termId);
            return Ok(ranking);
        }

        [HttpGet("parent/children/full-info/{termId}")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetChildrenFullInfo(Guid termId)
        {
            var parentId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var childrenInfo = await _scoreService.GetChildrenFullInfoAsync(parentId, termId);
            return Ok(childrenInfo);
        }

        // POST: api/score/analyze-and-note
        [HttpPost("analyze-and-note")]
        [Authorize(Roles = "teacher,admin")]
        public async Task<IActionResult> AnalyzeAndSaveNote([FromBody] Guid studentId, [FromQuery] Guid termId, [FromQuery] Guid classId)
        {
            var teacherId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

            // 1. Phân tích điểm của học sinh
            var analysis = await _analyticsService.AnalyzeStudentAsync(studentId, termId);
            if (analysis == null)
                return NotFound(new { Message = "Không tìm thấy dữ liệu để phân tích." });

            // 2. Tạo BehaviorNote từ phân tích
            var note = await _behaviorNoteService.AddNoteFromAnalysisAsync(
                studentId,
                classId,
                termId,
                teacherId,
                analysis.RiskLevel,
                analysis.Comment
            );

            return Ok(new
            {
                Message = "Phân tích & lưu ghi chú thành công.",
                Analysis = analysis,
                BehaviorNote = note
            });
        }

        // GET: api/score/parent/child/{studentId}/term/{termId}/full-summary
        [HttpGet("parent/child/{studentId:guid}/term/{termId:guid}/full-summary")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetChildFullSummary(Guid studentId, Guid termId)
        {
            var parentId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

            // TODO: check studentId có thuộc parent không

            // 1. Lấy phân tích học sinh
            var analysis = await _analyticsService.AnalyzeStudentAsync(studentId, termId);

            // 2. Lấy BehaviorNotes
            var notes = await _behaviorNoteService.GetNotesByStudentAsync(studentId, termId);

            return Ok(new
            {
                StudentId = studentId,
                TermId = termId,
                Analysis = analysis,
                BehaviorNotes = notes
            });
        }


    }
}
