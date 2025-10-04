using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class SubjectResultDto
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;

        public Guid? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public List<GradeComponentDto> Components { get; set; } = new();
    }

}
