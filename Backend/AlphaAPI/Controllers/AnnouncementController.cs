using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.interfaces;

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
        public async Task<IActionResult> AddAnnouncement([FromBody] Announcement ann)
        {
            if (ann == null)
                return BadRequest("Announcement cannot be null.");

            var created = await _service.AddAnnouncementAsync(ann);
            return CreatedAtAction(nameof(GetActiveAnnouncements), new { id = created.Id }, created);
        }

        // ✅ [PUT] Cập nhật thông báo
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnnouncement(Guid id, [FromBody] Announcement updated)
        {
            if (updated == null || id != updated.Id)
                return BadRequest("Invalid announcement data.");

            var result = await _service.UpdateAnnouncementAsync(updated);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // ✅ [DELETE] Xoá thông báo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(Guid id)
        {
            var success = await _service.DeleteAnnouncementAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
