using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HelpersV
{
    //класс-ViewModel
    public class PartFormCharacteristicObjectV
    {
        public CharacteristicObject Characteristic { get; set; }
        public string Type { get; set; }


        public PartFormCharacteristicObjectV()
        {

        }
    }
}