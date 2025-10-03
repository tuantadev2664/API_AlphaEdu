using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class ConversationDto
    {
        public Guid OtherUserId { get; set; }
        public string OtherUserName { get; set; } = "";
        public MessageDto? LastMessage { get; set; }
        public int UnreadCount { get; set; }
    }
}
