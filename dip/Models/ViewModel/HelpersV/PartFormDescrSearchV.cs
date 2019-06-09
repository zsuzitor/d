using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HelpersV
{
    //класс-ViewModel
    public class PartFormDescrSearchV
    {
        public DescriptionForm Form { get; set; }
        public DescrSearchI Param { get; set; }
        public string Type { get; set; }

        public PartFormDescrSearchV()
        {
            Form = null;
            Param = null;
            Type = null;

        }

    }
}