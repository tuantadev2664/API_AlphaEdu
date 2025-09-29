using System;

namespace DataAccessObjects.Dto
{
    public class CreateAssessmentRequest
    {
        public Guid ClassId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid TermId { get; set; }
        public Guid AcademicYearId { get; set; }
        public GradeComponentPayload GradeComponent { get; set; } = null!;
        public AssessmentPayload Assessment { get; set; } = null!;
        public bool InitializeScores { get; set; }
    }

    public class GradeComponentPayload
    {
        public string Name { get; set; } = null!;
        public string Kind { get; set; } = null!;
        public decimal Weight { get; set; }
        public decimal MaxScore { get; set; }
        public int Position { get; set; }
    }

    public class AssessmentPayload
    {
        public string Title { get; set; } = null!;
        public string DueDate { get; set; } = null!; // ISO date string
        public string? Description { get; set; }
    }
}


