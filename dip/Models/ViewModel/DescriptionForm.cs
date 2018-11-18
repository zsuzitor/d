using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Models.ViewModel
{
    public class DescriptionForm
    {
       // public string Postfix { get; set; }
        public List<Domain.AllAction> actionId { get; set; }
        public List<Domain.ActionType> actionType { get; set; }
        public List<Domain.FizVel> fizVelId { get; set; }
        public List<Domain.FizVel> parametricFizVelId { get; set; }
        public List<Pro> pros { get; set; }
        public List<Spec> spec { get; set; }
        public List<Vrem> vrem { get; set; }
        public string currentAction { get; set; }
        public string currentActionId { get; set; }

        //public Domain.Action Action { get; set; }


        public DescriptionForm()
        {

        }
    }
}