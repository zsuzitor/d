﻿using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HelpersV
{
    public class PartFormPhaseObjectV
    {

        public List<PhaseCharacteristicObject> Phases { get; set; }
        public string Type { get; set; }
        public string Param { get; set; }


        public PartFormPhaseObjectV()
        {

        }


    }
}