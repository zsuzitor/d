using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models
{

    public class SaveDescriptionEntry
    {
        public string Id { get; set; }
        //public string NewId { get; set; }
        public string ParentId { get; set; }
        //public int Type { get; set; }
        public string Text { get; set; }
        public bool Parametric { get; set; }
        //public int TypeAction { get; set; }


        public SaveDescriptionEntry()
        {
            //NewId = null;
        }
    }
}