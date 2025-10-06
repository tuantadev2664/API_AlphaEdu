using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class CreateAnnouncementRequest
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public Guid ClassId { get; set; }
        public Guid? SubjectId { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsUrgent { get; set; }
    }
}
