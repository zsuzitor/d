using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.Actions
{
    public class LoadDescrV
    {

        public Dictionary<string, string> DictDescrData { get; set; }


        public LoadDescrV()
        {
            DictDescrData = new Dictionary<string, string>();

        }



    }
}