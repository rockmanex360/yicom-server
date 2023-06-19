using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Response
    {
        public string Message { get; set; } = String.Empty;
        public string TimeStamp { get; set; } = String.Empty;
        public int Priority { get; set; }
    }
}