using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class TranscriptDto
    {
        public Guid TermId { get; set; }
        public string TermName { get; set; } = string.Empty;
        public List<SubjectTranscriptDto> Subjects { get; set; } = new();
    }
}
