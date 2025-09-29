using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ClassWithStudentCountDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid GradeId { get; set; }
        public Guid? HomeroomTeacherId { get; set; }
        public int StudentCount { get; set; }
    }
}
