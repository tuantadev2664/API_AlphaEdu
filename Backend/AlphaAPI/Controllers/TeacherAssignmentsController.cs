using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.interfaces;
using System.Security.Claims;

namespace AlphaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "teacher")]
    public class TeacherAssignmentsController : ControllerBase
    {
        private readonly ITeacherAssignmentService _service;

        public TeacherAssignmentsController(ITeacherAssignmentService service)
        {
            _service = service;
        }

        private Guid GetCurrentUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(id);
        }


        // GET: api/teacherassignments/my-classes?academicYearId=...
        [HttpGet("my-classes")]
        public async Task<IActionResult> GetMyClasses([FromQuery] Guid academicYearId)
        {
            var teacherId = GetCurrentUserId();
            var classes = await _service.GetClassesByTeacherAsync(teacherId, academicYearId);

            if (classes == null || classes.Count == 0)
                return NotFound(new { Message = "No classes found for this teacher in the selected academic year." });

            return Ok(classes);
        }

        // GET: api/teacherassignments/subjects?classId=...&academicYearId=...
        [HttpGet("subjects")]
        public async Task<IActionResult> GetSubjectsByClass([FromQuery] Guid classId, [FromQuery] Guid academicYearId)
        {
            var teacherId = GetCurrentUserId();
            var subjects = await _service.GetSubjectsByTeacherAndClassAsync(teacherId, classId, academicYearId);

            if (subjects == null || subjects.Count == 0)
                return NotFound(new { Message = "No subjects found for this teacher in the selected class and academic year." });

            return Ok(subjects);
        }
    }
}
