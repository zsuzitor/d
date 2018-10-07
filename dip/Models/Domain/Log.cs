using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Log
    {
        public DateTime DateTime { get; set; }
        public string Action { get; set; }
        public bool Succes { get; set; }
        public string Info { get; set; }

        public Log()
        {

        }

    }
}