using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class StudentDto
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}
