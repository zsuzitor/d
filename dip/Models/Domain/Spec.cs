using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Spec
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Parent { get; set; }

        //public ICollection<ActionSpec> ActionSpecs { get; set; }


        public Spec()
        {


        }
    }
}