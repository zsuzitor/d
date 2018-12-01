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
using static dip.Models.Functions;

namespace dip.Models.Domain
{

    public class test
    {
        //[Index("PK_FeTexts_cons", IsClustered = true, IsUnique = true)]//,IsClustered =true
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


    public class DescrSearchIInput
    {
        public string actionIdI { get; set; }
        public string actionTypeI { get; set; }
        public string FizVelIdI { get; set; }
        public string parametricFizVelIdI { get; set; }
        public string listSelectedProsI { get; set; }
        public string listSelectedSpecI { get; set; }
        public string listSelectedVremI { get; set; }
        
        public DescrSearchIInput()
        {

        }



        public static bool Validation(DescrSearchIInput a)
        {
            if (a != null)
            {
                a.actionIdI = NullToEmpryStr(a.actionIdI);
                a.actionTypeI = NullToEmpryStr(a.actionTypeI);
                a.FizVelIdI = NullToEmpryStr(a.FizVelIdI);
                a.parametricFizVelIdI = NullToEmpryStr(a.parametricFizVelIdI);
                a.listSelectedProsI = NullToEmpryStr(a.listSelectedProsI);
                a.listSelectedSpecI = NullToEmpryStr(a.listSelectedSpecI);
                a.listSelectedVremI = NullToEmpryStr(a.listSelectedVremI);
                return true;
            }
            return false;
        }
    }



    public class DescrSearchIOut
    {
        public string actionIdO { get; set; }
        public string actionTypeO { get; set; }
        public string FizVelIdO { get; set; }
        public string parametricFizVelIdO { get; set; }
        public string listSelectedProsO { get; set; }
        public string listSelectedSpecO { get; set; }
        public string listSelectedVremO { get; set; }


        public DescrSearchIOut()
        {

        }


        public static bool Validation(DescrSearchIOut a)
        {
            
            if (a != null)
            {
                a.actionIdO = NullToEmpryStr(a.actionIdO);
                a.actionTypeO = NullToEmpryStr(a.actionTypeO);
                a.FizVelIdO = NullToEmpryStr(a.FizVelIdO);
                a.parametricFizVelIdO = NullToEmpryStr(a.parametricFizVelIdO);
                a.listSelectedProsO = NullToEmpryStr(a.listSelectedProsO);
                a.listSelectedSpecO = NullToEmpryStr(a.listSelectedSpecO);
                a.listSelectedVremO = NullToEmpryStr(a.listSelectedVremO);
                return true;
            }
            return false;
        }
    }

}