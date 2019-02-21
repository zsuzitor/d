using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    //--
    public class NewFEIndex
    {

        public int Id { get; set; }
        public int Idfe { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string BeginObjectState { get; set; }
        public string EndObjectState { get; set; }
        public string BeginPhase { get; set; }
        public string EndPhase { get; set; }

        public NewFEIndex()
        {


        }
    }
}