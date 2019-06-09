using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HomeV
{
    //класс-ViewModel
    public class MainHeaderV
    {
        public List<string> SearchList { get; set; }

        public MainHeaderV()
        {
            SearchList = null;
        }

    }
}