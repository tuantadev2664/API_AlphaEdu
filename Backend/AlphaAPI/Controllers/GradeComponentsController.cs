using BusinessObjects.Models;
using DataAccessObjects.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.interfaces;
using Services.services;

namespace AlphaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GradeComponentsController : ControllerBase
    {
        private readonly IGradeComponentService _service;

        public GradeComponentsController(IGradeComponentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetByFilter([FromQuery] Guid classId, [FromQuery] Guid subjectId, [FromQuery] Guid termId)
        {
            var items = await _service.GetByClassSubjectTermAsync(classId, subjectId, termId);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GradeComponentCreateDto dto)
        {
            if (await _service.ExistsByNameAsync(dto.ClassId, dto.SubjectId, dto.TermId, dto.Name))
                return Conflict(new { message = "Grade component name already exists in this scope" });

            var entity = new GradeComponent
            {
                Id = Guid.NewGuid(),
                ClassId = dto.ClassId,
                SubjectId = dto.SubjectId,
                TermId = dto.TermId,
                Name = dto.Name,
                Kind = dto.Kind,
                Weight = dto.Weight,
                MaxScore = dto.MaxScore,
                Position = dto.Position
            };

            await _service.AddAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] GradeComponentUpdateDto dto)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = dto.Name;
            existing.Kind = dto.Kind;
            existing.Weight = dto.Weight;
            existing.MaxScore = dto.MaxScore;
            existing.Position = dto.Position;

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


