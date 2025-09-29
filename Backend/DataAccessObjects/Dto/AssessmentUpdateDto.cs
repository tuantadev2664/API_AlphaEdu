using System;

namespace DataAccessObjects.Dto
{
    public class AssessmentUpdateDto
    {
        public string Title { get; set; } = null!;
        public DateOnly? DueDate { get; set; }
        public string? Description { get; set; }
    }
}


