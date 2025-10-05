using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class StudentClassDto
    {

        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string GradeName { get; set; } = string.Empty;
        public string AcademicYearName { get; set; } = string.Empty;
    }
}
