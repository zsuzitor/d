using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HelpersV
{
    //класс-ViewModel
    public class DescrFormListDataV<T>
    {

        public List<T> List { get; set; }
        public DescrSearchI Param { get; set; }
        public string Type { get; set; }//входное\выходное
        public string ParentId { get; set; }//для редактирования формы

        public DescrFormListDataV()
        {
            List = null;
            Param = null;
            Type = null;
            ParentId = "";
        }
    }
}