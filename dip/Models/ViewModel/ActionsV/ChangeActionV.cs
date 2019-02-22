using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    public class ChangeActionV
    {
        public string CheckboxParamsId { get; set; }
        public string ParametricFizVelsId { get; set; }
        public string FizVelId { get; set; }
        public string Type { get; set; }
        public bool? Parametric { get; set; }

        public ChangeActionV()
        {
            CheckboxParamsId = null;
            ParametricFizVelsId = null;
            FizVelId = null;
            Type = null;
            Parametric = null;
        }
    }
}