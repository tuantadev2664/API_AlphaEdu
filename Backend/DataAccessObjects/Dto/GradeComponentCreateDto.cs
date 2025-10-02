using System;

namespace DataAccessObjects.Dto
{
    public class GradeComponentCreateDto
    {
        public Guid ClassId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid TermId { get; set; }
        public string Name { get; set; } = null!;
        public string Kind { get; set; } = null!; // ví dụ: Oral, Quiz, Midterm, Final
        public decimal Weight { get; set; }
        public decimal MaxScore { get; set; }
        public int? Position { get; set; }
    }
}



