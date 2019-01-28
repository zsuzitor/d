using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    public class DescriptionInputV
    {
        public DescriptionForm InputForm { get; set; }
        public DescriptionForm OutpForm { get; set; }

        public DescrSearchI InputFormData { get; set; }
        public DescrSearchI OutputFormData { get; set; }
        
        public DescriptionInputV()
        {
            InputForm = null;
            OutpForm = null;
            InputFormData = null;
            OutputFormData = null;
        }

    }
}