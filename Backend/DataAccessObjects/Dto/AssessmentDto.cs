using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class AssessmentDto
    {
        public Guid AssessmentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }   // OK vì mình convert từ DateOnly?
        public decimal? Score { get; set; }      // Score1 là decimal? => mapping thẳng
        public bool IsAbsent { get; set; }       // mapping ?? false
        public string? Comment { get; set; }
    }
}
