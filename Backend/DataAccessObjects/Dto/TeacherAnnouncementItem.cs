using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class TeacherAnnouncementItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public bool? IsUrgent { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public Guid? ClassId { get; set; }
        public string? ClassName { get; set; }

        public Guid? SubjectId { get; set; }
        public string? SubjectName { get; set; }

        public Guid SenderId { get; set; }
        public string? SenderName { get; set; }
    }
}
