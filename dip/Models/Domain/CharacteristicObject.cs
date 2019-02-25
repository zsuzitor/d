using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class CharacteristicObject
    {
        public List<PhaseCharacteristicObject> Phase1 { get; set; }
        public string ParamPhase1 { get; set; }
        public List<PhaseCharacteristicObject> Phase2 { get; set; }
        public string ParamPhase2 { get; set; }

        public List<PhaseCharacteristicObject> Phase3 { get; set; }
        public string ParamPhase3 { get; set; }



        public CharacteristicObject()
        {

        }
    }
}