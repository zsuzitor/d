using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{

    class test
    {
        public int Id { get; set; }

        public string levName { get; set; }
        public string levText { get; set; }
        public string levTextInp { get; set; }
        public string levTextOut { get; set; }
        public string levTextObj { get; set; }
        public string levTextApp { get; set; }
        public string levTextLit { get; set; }


        public test()
        {

        }
    }

}