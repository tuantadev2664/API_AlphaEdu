using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class ComponentAnalysisDto
    {
        public Guid GradeComponentId { get; set; }
        public string GradeComponentName { get; set; } = string.Empty;
        public string Kind { get; set; } = string.Empty;

        public decimal Weight { get; set; }
        public decimal MaxScore { get; set; }

        public decimal? Average { get; set; }
        public int Count { get; set; }
        public int BelowThresholdCount { get; set; }

        public string RiskLevel { get; set; } = "Low";
        public string Comment { get; set; } = string.Empty;
    }

}
