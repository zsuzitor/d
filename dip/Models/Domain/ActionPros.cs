using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    [Table("ActionPros")]
    public class ActionPros_
    {
        [Key]
        public int actionId { get; set; }
        public string prosId { get; set; }

        public ActionPros_()
        {

        }
    }
}