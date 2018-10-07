using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class FEIndex_
    {
        [Key]
        public int IDFE { get; set; }

        public string Index { get; set; }

        public FEIndex_()
        {

        }
    }
}