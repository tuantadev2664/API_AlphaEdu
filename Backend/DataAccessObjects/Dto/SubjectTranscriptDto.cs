using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class SubjectTranscriptDto
    {
        //public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public decimal AverageScore { get; set; }
    }
}
