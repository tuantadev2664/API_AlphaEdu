using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class StudentRankingDto
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public decimal? Average { get; set; }
        public int? Rank { get; set; }
    }

}
