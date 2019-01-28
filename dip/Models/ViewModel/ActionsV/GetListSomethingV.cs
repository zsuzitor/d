using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    public class GetListSomethingV<T>
    {
        public List<T> List { get; set; }
        public string CurrentActionId { get; set; }
        public string Type { get; set; }

        public GetListSomethingV()
        {
            List = null;
            CurrentActionId = null;
            Type = null;
        }
    }
}