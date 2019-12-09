using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FengWo.Web.Hubs.Entities
{
    public class Message
    {
        public int UserId { get; set; }

        public int TargetUserId { get; set; }

        public string UserName { get; set; }

        public string MessageData { get; set; }
    }
}
