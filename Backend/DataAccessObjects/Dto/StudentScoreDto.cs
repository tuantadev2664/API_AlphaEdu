using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class StudentScoreDto
    {
        public string Subject { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
        public decimal? Score { get; set; }
        public decimal Weight { get; set; }
    }
}
