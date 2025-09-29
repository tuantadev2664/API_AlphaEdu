using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ScoreUpdateDto
    {
        public Guid Id { get; set; }
        public decimal? Score1 { get; set; }
        public bool? IsAbsent { get; set; }
        public string? Comment { get; set; }
    }

}
