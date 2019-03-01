using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.PhysicV
{
    public class EditV
    {
        public FEText Obj { get; set; }
        public List<DescrSearchI> FormsInput { get; set; }
        public List<DescrSearchI> FormsOutput { get; set; }

        public EditV()
        {
            Obj = null;
            FormsInput = null;
            FormsOutput = null;
        }

    }
}