using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class LogParam
    {
        public int Id { get; set; }
        public string Param { get; set; }

        public int LogId { get; set; }
        public Log Log { get; set; }

        public LogParam()
        {

        }
    }
}