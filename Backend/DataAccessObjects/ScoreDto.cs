using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ScoreDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid AssessmentId { get; set; }
        public decimal? Score1 { get; set; }
        public bool? IsAbsent { get; set; }
        public string? Comment { get; set; }

        // Thông tin thêm (flatten từ Assessment & GradeComponent)
        public string? AssessmentName { get; set; }
        public string? GradeComponentName { get; set; }
        public decimal Weight { get; set; }
    }
}
