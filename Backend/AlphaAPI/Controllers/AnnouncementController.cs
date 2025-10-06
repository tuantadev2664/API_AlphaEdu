using BusinessObjects.Models;
using DataAccessObjects.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.interfaces;
using System.Security.Claims;

namespace AlphaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // có thể tắt khi test: [AllowAnonymous]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementServices _service;

        public AnnouncementController(IAnnouncementServices service)
        {
            _service = service;
        }

        // ✅ [GET] Lấy tất cả thông báo còn hiệu lực
        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveAnnouncements()
        {
            var result = await _service.GetActiveAnnouncementsAsync();
            return Ok(result);
        }

        // ✅ [GET] Lấy thông báo theo lớp, kỳ, năm
        [HttpGet("class/{classId}/term/{termId}/year/{academicYearId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByClassAndTerm(
            Guid classId, Guid termId, Guid academicYearId)
        {
            var result = await _service.GetAnnouncementsByClassAndTermAsync(classId, termId, academicYearId);
            return Ok(result);
        }

        // ✅ [GET] Lấy thông báo theo lớp
        [HttpGet("class/{classId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByClass(Guid classId)
        {
            var result = await _service.GetAnnouncementsByClassAsync(classId);
            return Ok(result);
        }

        // ✅ [GET] Lấy thông báo theo môn học
        [HttpGet("subject/{subjectId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBySubject(Guid subjectId)
        {
            var result = await _service.GetAnnouncementsBySubjectAsync(subjectId);
            return Ok(result);
        }

        // ✅ [GET] Lấy thông báo do 1 giáo viên tạo
        [HttpGet("teacher/{teacherId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByTeacher(Guid teacherId)
        {
            var result = await _service.GetAnnouncementsByTeacherAsync(teacherId);
            return Ok(result);
        }

        // ✅ [GET] Lấy các thông báo khẩn cấp
        [HttpGet("urgent")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUrgentAnnouncements()
        {
            var result = await _service.GetUrgentAnnouncementsAsync();
            return Ok(result);
        }

        // ✅ [POST] Tạo mới thông báo
        [HttpPost]
        public async Task<IActionResult> AddAnnouncement([FromBody] CreateAnnouncementRequest request)
        {
            if (request == null)
                return BadRequest("Announcement request cannot be null.");

            // 🧩 Lấy senderId từ JWT
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var senderIdStr = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(senderIdStr))
                return Unauthorized("Không thể xác định người gửi từ token.");

            var senderId = Guid.Parse(senderIdStr);

            var created = await _service.AddAnnouncementAsync(request, senderId);
            return CreatedAtAction(nameof(GetActiveAnnouncements), new { id = created.Id }, created);
        }


        // ✅ [PUT] Cập nhật thông báo
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnnouncement(Guid id, [FromBody] UpdateAnnouncementRequest request)
        {
            if (request == null || id != request.Id)
                return BadRequest("Invalid announcement data.");

            var updated = await _service.UpdateAnnouncementAsync(request);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // ✅ [DELETE] Xoá thông báo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(Guid id)
        {
            var result = await _service.DeleteAnnouncementAsync(id);

            if (!result.Success)
                return NotFound(new { result.Success,result.Message });

            return Ok(new { result.Success,result.Message });
        }

    }
}
