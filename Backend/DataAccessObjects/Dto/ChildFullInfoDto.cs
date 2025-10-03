using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class ChildFullInfoDto
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;

        public List<ScoreDto> Scores { get; set; } = new();
        public TranscriptDto Transcript { get; set; } = new();
        public List<BehaviorNoteDto> BehaviorNotes { get; set; } = new();
        public List<AnnouncementDto> Announcements { get; set; } = new();
        public List<SubjectResultDto> Subjects { get; set; } = new();
    }
}
