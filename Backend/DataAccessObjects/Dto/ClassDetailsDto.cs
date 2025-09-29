using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class ClassDetailsDto
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string GradeName { get; set; } = string.Empty;

        public Guid? HomeroomTeacherId { get; set; }
        public string? HomeroomTeacherName { get; set; }

        public int StudentCount { get; set; }
        public List<StudentDto> Students { get; set; } = new();

        public List<SubjectTeacherDto> Subjects { get; set; } = new();
    }
}
