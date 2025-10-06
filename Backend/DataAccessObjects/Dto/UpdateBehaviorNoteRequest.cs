using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class UpdateBehaviorNoteRequest
    {
        public Guid Id { get; set; }
        public string Note { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty; // 'Excellent' | 'Good' | ...
    }
}
