using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    [Table("ActionPros")]
    public class ActionPro
    {
        [Key]
        public int ActionId { get; set; }
        //public Action Action { get; set; }

        public string ProId { get; set; }
        //public Pro Pro { get; set; }


        public ActionPro()
        {

        }
    }
}