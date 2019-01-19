using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class ActionType: Item
    {
        //[Key]
        //public string Id { get; set; }
        //public string Name { get; set; }
        //public string Parent { get; set; }

        public ICollection<Action> Action { get; set; }

        public ActionType()
        {

        }
    }
}