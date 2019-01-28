using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HelpersV
{
    public class DescrFormListDataV<T>
    {

        public List<T> List { get; set; }
        public DescrSearchI Param { get; set; }
        public string Type { get; set; }

        public DescrFormListDataV()
        {
            List = null;
            Param = null;
            Type = null;
        }
    }
}