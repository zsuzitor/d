using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    //---
    public class ThesChild
    {

        public int Id { get; set; }
        public string NodeID { get; set; }
        public string ChildID { get; set; }
        public int? Order { get; set; }


        public ThesChild()
        {

        }
    }
}