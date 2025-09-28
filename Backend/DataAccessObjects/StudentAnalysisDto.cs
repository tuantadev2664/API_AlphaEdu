using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class StudentAnalysisDto
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public decimal Average { get; set; }
        public int BelowCount { get; set; }
        public string RiskLevel { get; set; } = "Low";
        public string Comment { get; set; } = string.Empty;
        public Dictionary<string, decimal?> Transcript { get; set; } = new();
    }

}
