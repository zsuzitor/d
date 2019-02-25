using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    public class ObjectInputV
    {
        public StateObject StateStart { get; set; }
        public string StateStartSelected { get; set; }

        public StateObject StateEnd { get; set; }
        public string StateEndSelected { get; set; }

        public CharacteristicObject CharacteristicStart { get; set; }
        //public List<PhaseCharacteristicObject> CharacteristicStart1 { get; set; }
        //    public List<PhaseCharacteristicObject> CharacteristicStart2 { get; set; }
        //    public List<PhaseCharacteristicObject> CharacteristicStart3 { get; set; }

        public CharacteristicObject CharacteristicEnd { get; set; }
        //public List<PhaseCharacteristicObject> CharacteristicEnd1 { get; set; }
        //public List<PhaseCharacteristicObject> CharacteristicEnd2 { get; set; }
        //public List<PhaseCharacteristicObject> CharacteristicEnd3 { get; set; }


        public ObjectInputV()
        {
            StateStartSelected = "";
            StateEndSelected = "";
        }
    }
}