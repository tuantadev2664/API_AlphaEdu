using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class StudentDetailDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Trường
        public string? SchoolName { get; set; }

        // Danh sách lớp + năm học
        public List<StudentClassDto> Classes { get; set; } = new();

        // Danh sách phụ huynh
        public List<ParentDto> Parents { get; set; } = new();

        // Ghi chú hạnh kiểm
        public List<string> BehaviorNotes { get; set; } = new();

        // Điểm số
        public List<StudentScoreDto> Scores { get; set; } = new();
    }
}
