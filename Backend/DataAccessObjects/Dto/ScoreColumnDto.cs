using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccessObjects.Dto
{
    public class ScoreColumnDto
    {
        public Guid GradeComponentId { get; set; }
        public string GradeComponentName { get; set; }
        public string Kind { get; set; }
        public decimal Weight { get; set; }
        public decimal MaxScore { get; set; }

        public Guid? AssessmentId { get; set; }
        public string? AssessmentName { get; set; }

        public Guid? ScoreId { get; set; }

        public decimal? Score { get; set; }
        public bool IsAbsent { get; set; }
        public string? Comment { get; set; }
    }
}
