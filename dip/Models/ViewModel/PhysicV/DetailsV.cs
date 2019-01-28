using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.PhysicV
{
    public class DetailsV
    {
        public string EffectName { get; set; }
        public string TechnicalFunctionId { get; set; }
        public bool? Admin { get; set; }
        public FEText Effect { get; set; }

        public DetailsV()
        {
            EffectName = null;
            TechnicalFunctionId = null;
            Admin = null;
            Effect = null;
        }
    }
}