using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    public class DescriptionFormAllV
    {
        public List<DescrSearchI> DescrInp { get; set; }
        public List<DescrSearchI> DescrOutp { get; set; }
        public int DescrCountInput { get; set; }

        public bool ChangedObject { get; set; }
        public string ObjectStateIdBegin { get; set; }
        public string ObjectStateIdEnd { get; set; }
        public DescrObjectI ObjectFormsBegin { get; set; }
        public DescrObjectI ObjectFormsEnd { get; set; }


        public DescriptionFormAllV()
        {
            DescrCountInput = 1;
            ChangedObject = false;
            ObjectStateIdBegin = null;
            ObjectStateIdEnd = null;
            ObjectFormsBegin = null;
            ObjectFormsEnd = null;
        }

    }
}