using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ListPhysicsV
{
    //класс-ViewModel
    public class ListActV
    {
        public List<ListPhysics> Lists { get; set; }
        public int? CurrentListId { get; set; }


        public ListActV()
        {
            CurrentListId = null;
            Lists = new List<ListPhysics>();
        }
    }
}