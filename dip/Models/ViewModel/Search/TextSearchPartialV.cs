using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.Search
{
    public class TextSearchPartialV
    {
        public int CountLoad { get; set; }
        public List<int> ListPhysId { get; set; }
        

        public TextSearchPartialV()
        {
            CountLoad = 0;
        }
    }
}