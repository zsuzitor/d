﻿using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    public class GetPhaseObjectV
    {
        public List<PhaseCharacteristicObject> List { get; set; }

        public string Type { get; set; }


        public GetPhaseObjectV()
        {
            List = new List<PhaseCharacteristicObject>();
        }

    }
}