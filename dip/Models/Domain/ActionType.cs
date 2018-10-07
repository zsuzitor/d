using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class ActionType_
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }

        public Action Actions { get; set; }

        public ActionType_()
        {

        }
    }
}