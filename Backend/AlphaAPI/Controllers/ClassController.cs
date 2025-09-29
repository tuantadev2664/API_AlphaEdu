using Microsoft.AspNetCore.Mvc;

namespace AlphaAPI.Controllers
{
    using BusinessObjects.Models;
    using DataAccessObjects.Dto;
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
        public class ClassController : ControllerBase
        {
            private readonly IClassService _classService;

            public ClassController(IClassService classService)
            {
                _classService = classService;
            }

            // GET: api/classes
            [HttpGet]
            [Authorize(Roles = "teacher,admin")]
            public async Task<IActionResult> GetAll()
            {
                var classes = await _classService.GetAllAsync();
                return Ok(classes);
            }

            // GET: api/classes/{id}
            [HttpGet("{id:guid}")]
            [Authorize(Roles = "teacher,admin")]
            public async Task<IActionResult> GetById(Guid id)
            {
                var cls = await _classService.GetByIdAsync(id);
                if (cls is null)
                    return NotFound(new { Message = "Class not found." });

                return Ok(cls);
            }

            // GET: api/classes/{classId}/year/{academicYearId}/details
            [HttpGet("{classId:guid}/year/{academicYearId:guid}/details")]
            [Authorize(Roles = "teacher,admin")]
            public async Task<IActionResult> GetClassDetails(Guid classId, Guid academicYearId)
            {
                var details = await _classService.GetClassDetailsAsync(classId, academicYearId);
                if (details is null)
                    return NotFound(new { Message = "No class details found." });

                return Ok(details);
            }

            // POST: api/classes
            [HttpPost]
            [Authorize(Roles = "admin")]
            public async Task<IActionResult> Create([FromBody] Class cls)
            {
                if (cls == null)
                    return BadRequest(new { Message = "Class data is required." });

                await _classService.AddAsync(cls);
                return CreatedAtAction(nameof(GetById), new { id = cls.Id }, cls);
            }

            // PUT: api/classes/{id}
            [HttpPut("{id:guid}")]
            [Authorize(Roles = "admin")]
            public async Task<IActionResult> Update(Guid id, [FromBody] Class cls)
            {
                if (cls == null || id != cls.Id)
                    return BadRequest(new { Message = "Invalid class data." });

                var existing = await _classService.GetByIdAsync(id);
                if (existing is null)
                    return NotFound(new { Message = "Class not found." });

                await _classService.UpdateAsync(cls);
                return NoContent();
            }

            // DELETE: api/classes/{id}
            [HttpDelete("{id:guid}")]
            [Authorize(Roles = "admin")]
            public async Task<IActionResult> Delete(Guid id)
            {
                var existing = await _classService.GetByIdAsync(id);
                if (existing is null)
                    return NotFound(new { Message = "Class not found." });

                await _classService.DeleteAsync(id);
                return NoContent();
            }
        }
    }
}
