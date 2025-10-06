using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class CreateBehaviorNoteRequest
    {
        public Guid StudentId { get; set; }
        public Guid ClassId { get; set; }
        public Guid TermId { get; set; }
        public string Note { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public Guid CreatedBy { get; set; }
    }
}
