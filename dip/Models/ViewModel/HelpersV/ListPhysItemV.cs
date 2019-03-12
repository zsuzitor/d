using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HelpersV
{
    public class ListPhysItemV
    {

        public ListPhysics ListPhysic { get; set; }
            public bool Select { get; set; }
        public ListPhysItemV()
        {
            Select = false;
        }
    }
}