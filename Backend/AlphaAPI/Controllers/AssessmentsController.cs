using BusinessObjects.Models;
using DataAccessObjects;
using DataAccessObjects.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.interfaces;
using System.Security.Claims;

namespace AlphaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssessmentsController : ControllerBase
    {
        private readonly IAssessmentServices _service;
        private readonly IGradeComponentService _gradeComponentService;
        private readonly IScoreServices _scoreServices;
        private readonly SchoolDbContext _db;

        public AssessmentsController(IAssessmentServices service, IGradeComponentService gradeComponentService, IScoreServices scoreServices, SchoolDbContext db)
        {
            _service = service;
            _gradeComponentService = gradeComponentService;
            _scoreServices = scoreServices;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetByFilter([FromQuery] Guid? gradeComponentId, [FromQuery] Guid? classId, [FromQuery] Guid? subjectId, [FromQuery] Guid? termId)
        {
            if (gradeComponentId.HasValue)
            {
                var list = await _service.GetByGradeComponentAsync(gradeComponentId.Value);
                return Ok(list);
            }

            if (classId.HasValue && subjectId.HasValue && termId.HasValue)
            {
                var list = await _service.GetByClassSubjectTermAsync(classId.Value, subjectId.Value, termId.Value);
                return Ok(list);
            }

            return BadRequest(new { message = "Provide either gradeComponentId or classId+subjectId+termId" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> Create([FromBody] CreateAssessmentRequest request)
        {
            try
            {
                // 1) Create GradeComponent
                var gradeComponent = new GradeComponent
                {
                    Id = Guid.NewGuid(),
                    ClassId = request.ClassId,
                    SubjectId = request.SubjectId,
                    TermId = request.TermId,
                    Name = request.GradeComponent.Name,
                    Kind = request.GradeComponent.Kind,
                    Weight = request.GradeComponent.Weight,
                    MaxScore = request.GradeComponent.MaxScore,
                    Position = request.GradeComponent.Position
                };

                // prevent duplicate name
                if (await _gradeComponentService.ExistsByNameAsync(
                    gradeComponent.ClassId, gradeComponent.SubjectId, gradeComponent.TermId, gradeComponent.Name))
                {
                    return Conflict(new { success = false, message = "Grade component name already exists" });
                }

                await _gradeComponentService.AddAsync(gradeComponent);

                // 2) Create Assessment
                var dueDate = string.IsNullOrWhiteSpace(request.Assessment.DueDate)
                    ? (DateOnly?)null
                    : DateOnly.Parse(request.Assessment.DueDate);

                var assessment = new Assessment
                {
                    Id = Guid.NewGuid(),
                    GradeComponentId = gradeComponent.Id,
                    Title = request.Assessment.Title,
                    DueDate = dueDate,
                    Description = request.Assessment.Description
                };

                await _service.AddAsync(assessment);

                if (request.InitializeScores)
                {
                    var studentIds = await _db.ClassEnrollments
                        .Where(ce => ce.ClassId == request.ClassId && ce.AcademicYearId == request.AcademicYearId)
                        .Select(ce => ce.StudentId)
                        .ToListAsync();

                    // 🔑 lấy userId từ JWT claim
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                    {
                        return Unauthorized(new { success = false, message = "Không tìm thấy userId trong token" });
                    }
                    var createdBy = Guid.Parse(userIdClaim);

                    foreach (var sid in studentIds)
                    {
                        var score = new Score
                        {
                            Id = Guid.NewGuid(),
                            AssessmentId = assessment.Id,
                            StudentId = sid,
                            Score1 = null,
                            IsAbsent = false,
                            Comment = null,
                            CreatedBy = createdBy, 
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = null
                        };

                        await _scoreServices.AddAsync(score);
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = "Created assessment and grade component",
                    data = new
                    {
                        gradeComponentId = gradeComponent.Id,
                        assessmentId = assessment.Id
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message,
                    inner = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AssessmentUpdateDto dto)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Title = dto.Title;
            existing.DueDate = dto.DueDate;
            existing.Description = dto.Description;

            await _service.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _service.DeleteAsync(existing);
            return NoContent();
        }
    }
}


