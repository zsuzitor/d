using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class FEAction
    {
        public int Id { get; set; }
        public int Idfe { get; set; }
        public int Input { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string FizVelId { get; set; }
        public string FizVelSection { get; set; }
        public string FizVelChange { get; set; }
        public double FizVelLeftBorder { get; set; }
        public double FizVelRightBorder { get; set; }
        public string Pros { get; set; }
        public string Spec { get; set; }
        public string Vrem { get; set; }


        public FEAction()
        {

        }
    }
}