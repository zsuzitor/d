using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class The: Item
    {
        //[Key]
        //public string Id { get; set; }

        //public string Name { get; set; }

        //public string Parent { get; set; }

        public string Compatible { get; set; }

        public string Path { get; set; }

        public The()
        {

        }
    }
}