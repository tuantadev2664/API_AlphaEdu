using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class DeleteAnnouncementResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
    }
}
