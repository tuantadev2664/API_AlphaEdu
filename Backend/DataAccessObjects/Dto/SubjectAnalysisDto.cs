using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class SubjectAnalysisDto
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;

        public decimal? Average { get; set; }
        public int AssignmentsCount { get; set; }
        public int BelowThresholdCount { get; set; }

        public string RiskLevel { get; set; } = "low"; // low, medium, high
        public string Comment { get; set; } = string.Empty;

        public List<ComponentAnalysisDto> Components { get; set; } = new();
    }


}
