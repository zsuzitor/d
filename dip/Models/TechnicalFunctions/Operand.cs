using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dip.Models.TechnicalFunctions
{
    //---
    public class Operand
    {
        [Key]
        public string Id { get; set; }
        public string Value { get; set; }
        public string OperandGroupId { get; set; }


        public ICollection<Index> Index { get; set; }
        public OperandGroup OperandGroup { get; set; }
        public Operand()
        {

        }
    }
}