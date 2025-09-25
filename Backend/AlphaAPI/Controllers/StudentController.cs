using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.interfaces;
using System;
using System.Threading.Tasks;

namespace AlphaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentServices _studentService;

        public StudentController(IStudentServices studentService)
        {
            _studentService = studentService;
        }

        // GET: api/students
        [HttpGet]
        [Authorize(Roles = "teacher,admin")]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        // GET: api/students/{id}
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "teacher,admin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student is null)
                return NotFound(new { Message = "Student not found." });
            return Ok(student);
        }

        // GET: api/students/search?keyword=abc
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest(new { Message = "Keyword cannot be empty." });

            var students = await _studentService.SearchStudentsByNameAsync(keyword);
            return Ok(students);
        }

        // GET: api/students/class/{classId}/year/{academicYearId}
        [HttpGet("class/{classId:guid}/year/{academicYearId:guid}")]
        public async Task<IActionResult> GetByClass(Guid classId, Guid academicYearId)
        {
            var students = await _studentService.GetStudentsByClassAsync(classId, academicYearId);
            return Ok(students);
        }

        // GET: api/students/school/{schoolId}
        [HttpGet("school/{schoolId:guid}")]
        public async Task<IActionResult> GetBySchool(Guid schoolId)
        {
            var students = await _studentService.GetStudentsBySchoolAsync(schoolId);
            return Ok(students);
        }

        // POST: api/students
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User student)
        {
            if (student == null)
                return BadRequest(new { Message = "Student data is required." });

            await _studentService.AddAsync(student);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        // PUT: api/students/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] User student)
        {
            if (student == null || id != student.Id)
                return BadRequest(new { Message = "Invalid student data." });

            var existingStudent = await _studentService.GetByIdAsync(id);
            if (existingStudent is null)
                return NotFound(new { Message = "Student not found." });

            await _studentService.UpdateAsync(student);
            return NoContent();
        }

        // DELETE: api/students/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingStudent = await _studentService.GetByIdAsync(id);
            if (existingStudent is null)
                return NotFound(new { Message = "Student not found." });

            await _studentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
