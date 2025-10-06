using BusinessObjects.Models;
using DataAccessObjects.Dto;
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
        // POST: api/BehaviorNote
        [HttpPost]
        [Authorize(Roles = "teacher,admin")] // chỉ giáo viên hoặc admin mới được thêm ghi chú
        public async Task<IActionResult> AddNote([FromBody] CreateBehaviorNoteRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request data.");

            // ✅ Lấy ID giáo viên từ JWT token
            var teacherIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

            if (teacherIdClaim == null)
                return Unauthorized("Token không hợp lệ hoặc thiếu thông tin giáo viên.");

            var teacherId = Guid.Parse(teacherIdClaim);

            // ✅ Gọi service (truyền thêm teacherId)
            var result = await _service.AddNoteAsync(request, teacherId);

            return Ok(new
            {
                message = "Thêm ghi chú hành vi thành công.",
                data = result
            });
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
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] UpdateBehaviorNoteRequest request)
        {
            if (request == null || request.Id != id)
                return BadRequest("Dữ liệu không hợp lệ hoặc ID không khớp.");

            var result = await _service.UpdateNoteAsync(request);

            if (result == null)
                return NotFound("Không tìm thấy ghi chú hành vi cần cập nhật.");

            return Ok(result);
        }


        // DELETE: api/BehaviorNote/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            var success = await _service.DeleteNoteAsync(id);
            if (!success) return NotFound("Note not found");

            return Ok("Note deleted successfully");
        }

        // POST: api/BehaviorNote/confirm-ai
        //[HttpPost("confirm-ai")]
        //[Authorize(Roles = "teacher,admin")]
        //public async Task<IActionResult> ConfirmNoteFromAI([FromBody] BehaviorNote note)
        //{
        //    if (note == null) return BadRequest("Note cannot be null");

        //    // override CreatedBy từ token user
        //    note.CreatedBy = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

        //    var result = await _service.AddNoteAsync(note);
        //    return Ok(new
        //    {
        //        Message = "AI note confirmed and saved successfully",
        //        Note = result
        //    });
        //}

    }
}
