using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.SearchV
{
    public class DescriptionSearchV
    {

        public int[] SearchList { get; set; }
        public List<DescrSearchI> FormInput { get; set; }
        public List<DescrSearchI> FormOutput { get; set; }
        //TODO не нужно?
        //public bool? ItsSearch { get; set; }
        //TODO не нужно?
        //public bool? Search { get; set; }

        public DescriptionSearchV()
        {
            SearchList = null;
            FormInput = null;
            FormOutput = null;
            //ItsSearch = null;
            //Search = null;
        }
    }
}