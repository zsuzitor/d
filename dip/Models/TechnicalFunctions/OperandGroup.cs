using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dip.Models.TechnicalFunctions
{
    //---
    public class OperandGroup
    {
        [Key]
        public string Id { get; set; }
        public string Value { get; set; }
        //public string Parent { get; set; }

        public ICollection<Operand> Operands { get; set; }
        public OperandGroup()
        {

        }
    }
}