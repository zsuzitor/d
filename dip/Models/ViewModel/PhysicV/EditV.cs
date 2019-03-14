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

        public DescrObjectI FormObjectBegin { get; set; }
        public int CountPhaseBegin { get; set; }

        public DescrObjectI FormObjectEnd { get; set; }
        public int CountPhaseEnd { get; set; }

        //public bool ChangedObject { get; set; }
        //public string StateIdStart { get; set; }
        //public string StateIdEnd { get; set; }


        public EditV()
        {
            Obj = null;
            FormsInput = null;
            FormsOutput = null;
            FormObjectBegin = new DescrObjectI();
            FormObjectEnd = new DescrObjectI();

            CountPhaseBegin = 0;
            CountPhaseEnd = 0;
        }

    }
}