using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.PhysicV
{
    public class EditV
    {
        public FEText FeText { get; set; }
        public DescrSearchIInput FormInput { get; set; }
        public DescrSearchIOut FormOutput { get; set; }

        public EditV()
        {
            FeText = null;
            FormInput = null;
            FormOutput = null;
        }

    }
}