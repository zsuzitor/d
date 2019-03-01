using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    public class ChangeStateObjectV
    {
        public List<StateObject> States { get; set; }
       public string Type { get; set; }



        public CharacteristicObject Characteristics { get; set; }
        //public List<PhaseCharacteristicObject> CharacteristicStart1 { get; set; }
        //    public List<PhaseCharacteristicObject> CharacteristicStart2 { get; set; }
        //    public List<PhaseCharacteristicObject> CharacteristicStart3 { get; set; }

       


        public ChangeStateObjectV()
        {
            States = new List<Domain.StateObject>();
            Type = "";
            Characteristics = new CharacteristicObject();
        }
    }
}