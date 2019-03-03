using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    public class ObjectInputV
    {
        public List<StateObject> StatesStart { get; set; }
        public string StateStartSelected { get; set; }

        public bool changedObject { get; set; }

        public List<StateObject> StatesEnd { get; set; }
        public string StateEndSelected { get; set; }

        public CharacteristicObject CharacteristicsStart { get; set; }
        //public List<PhaseCharacteristicObject> CharacteristicStart1 { get; set; }
        //    public List<PhaseCharacteristicObject> CharacteristicStart2 { get; set; }
        //    public List<PhaseCharacteristicObject> CharacteristicStart3 { get; set; }

        public CharacteristicObject CharacteristicsEnd { get; set; }
        //public List<PhaseCharacteristicObject> CharacteristicEnd1 { get; set; }
        //public List<PhaseCharacteristicObject> CharacteristicEnd2 { get; set; }
        //public List<PhaseCharacteristicObject> CharacteristicEnd3 { get; set; }


        public ObjectInputV()
        {
            StateStartSelected = "";
            StateEndSelected = "";
            changedObject = false;
            CharacteristicsStart = new CharacteristicObject();
            CharacteristicsEnd = new CharacteristicObject();
        }
    }
}