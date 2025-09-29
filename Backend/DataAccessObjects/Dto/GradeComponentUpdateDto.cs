using System;

namespace DataAccessObjects.Dto
{
    public class GradeComponentUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Kind { get; set; } = null!;
        public decimal Weight { get; set; }
        public decimal MaxScore { get; set; }
        public int? Position { get; set; }
    }
}


