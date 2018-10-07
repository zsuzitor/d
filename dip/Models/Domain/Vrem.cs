using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Vrem_
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Parent { get; set; }

        public ICollection<ActionVrem> ActionVrems { get; set; }

        public Vrem_()
        {


        }
    }
}