using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.PhysicV
{
    public class CreateDescriptionObjectV
    {

        public List<StateObject> States { get; set; }
        public List<PhaseCharacteristicObject> Characteristic { get; set; }


        public CreateDescriptionObjectV()
        {
            States = new List<StateObject>();
            Characteristic = new List<PhaseCharacteristicObject>();


        }

    }
}