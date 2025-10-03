using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class AnnouncementDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public bool? IsUrgent { get; set; }   // nullable
        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public Guid SenderId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? SubjectId { get; set; }
    }


}
