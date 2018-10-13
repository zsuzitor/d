using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class ActionVrem
    {
        [Key]
        public int ActionId { get; set; }
        //public Action Action { get; set; }

        public string VremId { get; set; }
        //public Vrem Vrem { get; set; }




        public ActionVrem()
        {


        }
    }
}