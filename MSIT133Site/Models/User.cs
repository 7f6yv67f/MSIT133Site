using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSIT133Site.Models
{
    public class User
    {
        public string userName { get; set; }
        public int? age { get; set; }
        public string phone { get; set; }

        public string address { get; set; }

        public string email{ get; set; }
    }
}
