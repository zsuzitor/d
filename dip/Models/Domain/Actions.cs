using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    [Table("Actions")]
    public class Actions_
    {
        [Key]
        public int id { get; set; }
        public string actionId { get; set; }
        public string actionType { get; set; }
        public string fizVelId { get; set; }
       
        public Actions_()
        {

        }
    }
}
