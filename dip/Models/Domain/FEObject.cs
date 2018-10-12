using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class FEObject
    {

        public int Id { get; set; }
        public int Idfe { get; set; }
        public int Begin { get; set; }
        public string PhaseState { get; set; }
        public string Composition { get; set; }
        public string MagneticStructure { get; set; }
        public string Conductivity { get; set; }
        public string MechanicalState { get; set; }
        public string OpticalState { get; set; }
        public string Special { get; set; }

        public FEObject()
        {


        }
    }
}