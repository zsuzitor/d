using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class ActionSpec
    {
        [Key]
        public int ActionId { get; set; }
        public Action Action { get; set; }

        public string SpecId { get; set; }
        public Spec Spec { get; set; }


        public ActionSpec()
        {

        }

    }
}