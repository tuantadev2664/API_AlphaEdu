using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Dto
{
    public class UpdateAnnouncementRequest : CreateAnnouncementRequest
    {
        public Guid Id { get; set; }
    }
}
