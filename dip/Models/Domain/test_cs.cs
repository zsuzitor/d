﻿using System;
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



        //public static bool Validation(DescrSearchIInput a)
        //{
        //    if (a != null)
        //    {
        //        a.actionIdI = NullToEmpryStr(a.actionIdI);
        //        a.actionTypeI = NullToEmpryStr(a.actionTypeI);
        //        a.FizVelIdI = NullToEmpryStr(a.FizVelIdI);
        //        a.parametricFizVelIdI = NullToEmpryStr(a.parametricFizVelIdI);
        //        a.listSelectedProsI = NullToEmpryStr(a.listSelectedProsI);
        //        a.listSelectedSpecI = NullToEmpryStr(a.listSelectedSpecI);
        //        a.listSelectedVremI = NullToEmpryStr(a.listSelectedVremI);
        //        return true;
        //    }
        //    return false;
        //}
        //public static bool IsNull(DescrSearchIInput a)
        //{
        //    if (a == null)
        //        return true;
        //    if (a.actionIdI == null && a.actionTypeI == null && a.FizVelIdI == null
        //        && a.parametricFizVelIdI == null && a.listSelectedProsI == null
        //        && a.listSelectedSpecI == null && a.listSelectedVremI == null)
        //        return true;


        //    return false;
        //}


    }

    public class DescrSearchI
    {
        public string actionId { get; set; }
        public string actionType { get; set; }
        public string FizVelId { get; set; }
        public string parametricFizVelId { get; set; }
        public string listSelectedPros { get; set; }
        public string listSelectedSpec { get; set; }
        public string listSelectedVrem { get; set; }


        public DescrSearchI()
        {

        }
        public  DescrSearchI(DescrSearchIInput a)
        {
            
            
            this.actionId = a.actionIdI;
            this.actionType = a.actionTypeI;
            this.FizVelId = a.FizVelIdI;
            this.parametricFizVelId = a.parametricFizVelIdI;
            this.listSelectedPros = a.listSelectedProsI;
            this.listSelectedSpec = a.listSelectedSpecI;
            this.listSelectedVrem = a.listSelectedVremI;
            
        }

        public DescrSearchI(DescrSearchIOut a)
        {


            this.actionId = a.actionIdO;
            this.actionType = a.actionTypeO;
            this.FizVelId = a.FizVelIdO;
            this.parametricFizVelId = a.parametricFizVelIdO;
            this.listSelectedPros = a.listSelectedProsO;
            this.listSelectedSpec = a.listSelectedSpecO;
            this.listSelectedVrem = a.listSelectedVremO;
            
        }


        public static bool Validation(DescrSearchI a)
        {

            if (a != null)
            {
                a.actionId = NullToEmpryStr(a.actionId);
                a.actionType = NullToEmpryStr(a.actionType);
                a.FizVelId = NullToEmpryStr(a.FizVelId);
                a.parametricFizVelId = NullToEmpryStr(a.parametricFizVelId);
                a.listSelectedPros = NullToEmpryStr(a.listSelectedPros);
                a.listSelectedSpec = NullToEmpryStr(a.listSelectedSpec);
                a.listSelectedVrem = NullToEmpryStr(a.listSelectedVrem);
                return true;
            }
            return false;
        }

        public static bool IsNull(DescrSearchI a)
        {
            if (a == null)
                return true;
            if (a.actionId == null && a.actionType == null && a.FizVelId == null
                && a.parametricFizVelId == null && a.listSelectedPros == null
                && a.listSelectedSpec == null && a.listSelectedVrem == null)
                return true;


            return false;
        }

        //public static DescrSearchI Set(DescrSearchIInput a)
        //{
        //    if (a == null)
        //        return null;
        //    DescrSearchI res = new DescrSearchI();
        //    res.actionId = a.actionIdI;
        //    res.actionType = a.actionTypeI;
        //    res.FizVelId = a.FizVelIdI;
        //    res.parametricFizVelId = a.parametricFizVelIdI;
        //    res.listSelectedPros = a.listSelectedProsI;
        //    res.listSelectedSpec = a.listSelectedSpecI;
        //    res.listSelectedVrem = a.listSelectedVremI;
        //    return res;
        //}

        //public static DescrSearchI Set(DescrSearchIOut a)
        //{
        //    if (a == null)
        //        return null;
        //    DescrSearchI res = new DescrSearchI();
        //    res.actionId = a.actionIdO;
        //    res.actionType = a.actionTypeO;
        //    res.FizVelId = a.FizVelIdO;
        //    res.parametricFizVelId = a.parametricFizVelIdO;
        //    res.listSelectedPros = a.listSelectedProsO;
        //    res.listSelectedSpec = a.listSelectedSpecO;
        //    res.listSelectedVrem = a.listSelectedVremO;
        //    return res;
        //}

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


        //public static bool Validation(DescrSearchIOut a)
        //{
            
        //    if (a != null)
        //    {
        //        a.actionIdO = NullToEmpryStr(a.actionIdO);
        //        a.actionTypeO = NullToEmpryStr(a.actionTypeO);
        //        a.FizVelIdO = NullToEmpryStr(a.FizVelIdO);
        //        a.parametricFizVelIdO = NullToEmpryStr(a.parametricFizVelIdO);
        //        a.listSelectedProsO = NullToEmpryStr(a.listSelectedProsO);
        //        a.listSelectedSpecO = NullToEmpryStr(a.listSelectedSpecO);
        //        a.listSelectedVremO = NullToEmpryStr(a.listSelectedVremO);
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool IsNull(DescrSearchIOut a)
        //{
        //    if (a == null)
        //        return true;
        //    if (a.actionIdO == null && a.actionTypeO == null && a.FizVelIdO == null
        //        && a.parametricFizVelIdO == null && a.listSelectedProsO == null
        //        && a.listSelectedSpecO == null && a.listSelectedVremO == null)
        //        return true;


        //    return false;
        //}
    }

}