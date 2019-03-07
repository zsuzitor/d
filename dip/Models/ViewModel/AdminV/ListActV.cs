using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.AdminV
{
    public class ListActV
    {
        public List<ListPhysics> Lists { get; set; }



        public ListActV()
        {
            Lists = new List<ListPhysics>();
        }
    }
}