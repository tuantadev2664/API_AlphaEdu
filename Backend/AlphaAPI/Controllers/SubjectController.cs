using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.interfaces;
using System;
using System.Threading.Tasks;

namespace AlphaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // nếu muốn test nhanh có thể comment dòng này
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetSubjectsByStudent(Guid studentId)
        {
            try
            {
                var result = await _subjectService.GetSubjectsByStudentAsync(studentId);

                if (result == null || result.Count == 0)
                {
                    return NotFound(new
                    {
                        Message = $"Không tìm thấy môn học nào cho học sinh {studentId}"
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Có lỗi xảy ra khi lấy dữ liệu môn học",
                    Error = ex.Message
                });
            }
        }
    }
}
