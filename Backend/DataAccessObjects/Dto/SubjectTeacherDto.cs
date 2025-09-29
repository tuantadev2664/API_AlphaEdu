using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class SubjectTeacherDto
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;

        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
    }
}
