using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dip.Models.TechnicalFunctions
{
    //---
    public class Index
    {
        [Key]
        public string Id { get; set; }
        public string OperationId { get; set; }
        public string OperandId { get; set; }
        public string LimitId { get; set; }
        public string EffectIds { get; set; }


        public virtual Limit Limit { get; set; }
        public virtual Operand Operand { get; set; }
        public virtual Operation Operation { get; set; }

        public Index()
        {

        }
    }
}