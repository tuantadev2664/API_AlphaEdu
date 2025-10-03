using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class GradeComponentDto
    {
        public Guid GradeComponentId { get; set; }
        public string ComponentName { get; set; } = string.Empty;
        public string Kind { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public decimal MaxScore { get; set; }
        public List<AssessmentDto> Assessments { get; set; } = new();
    }
}
