﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public string SenderName { get; set; } = "";
        public Guid ReceiverId { get; set; }
        public string ReceiverName { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime? CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
