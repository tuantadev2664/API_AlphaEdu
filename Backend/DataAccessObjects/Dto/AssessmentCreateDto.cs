using System;

namespace DataAccessObjects.Dto
{
    public class AssessmentCreateDto
    {
        public Guid GradeComponentId { get; set; }
        public string Title { get; set; } = null!;
        public DateOnly? DueDate { get; set; }
        public string? Description { get; set; }
    }
}



