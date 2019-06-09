using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.SearchV
{
    //класс-ViewModel
    public class TextSearchV
    {

        public int CountLoad { get; set; }
        public List<int> ListPhysId { get; set; }
        public bool? ShowBtLoad { get; set; }
        public bool Admin { get; set; }


        public TextSearchV()
        {
            CountLoad = 0;
            ListPhysId = new List<int>();
            ShowBtLoad = null;
            Admin = false;
        }
    }
}