using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Image
    {
        public int Id { get; set; }

        public int FeTextId { get; set; }
        public FEText FeText { get; set; }



    }
}