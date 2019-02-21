using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using static dip.Models.Functions;


using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Web.Hosting;
//using Microsoft.SqlServer.Management.Common;




namespace dip.Models.Domain
{
    enum RolesProject { admin, subscriber, user };//vip
    //var a = (RolesProject)Enum.Parse(typeof(RolesProject), "", true);



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

        [ScaffoldColumn(false)]
        public bool? Valide { get;private set; }

        public DescrSearchIInput()
        {


            Valide = null;
        }
        public DescrSearchIInput(FEAction a)
        {
           
            this.actionIdI = a.Name;
            this.actionTypeI = a.Type;
            this.FizVelIdI = a.FizVelId;
            this.listSelectedProsI = a.Pros;
            this.listSelectedSpecI = a.Spec;
            this.listSelectedVremI = a.Vrem;
            this.parametricFizVelIdI = a.FizVelSection;
           
        }


        public static bool ValidationIfNeed(DescrSearchIInput a)
        {
            var res = a?.Valide?? DescrSearchIInput.Validation(a);
            //if (res == null)
            //    res = DescrSearchIInput.Validation(a);
            return res;
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


                a.listSelectedProsI = Pro.SortIds(a?.listSelectedProsI);
                //if (a?.listSelectedProsI != null)
                //    this.listSelectedPros = string.Join(" ", (a.listSelectedProsI).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
                //        OrderBy(x1 =>int.Parse( x1.Split(new string[] { "PROS" }, StringSplitOptions.RemoveEmptyEntries)[1])).ToList());
                //else
                //    this.listSelectedPros = null;

                a.listSelectedSpecI = Spec.SortIds(a?.listSelectedSpecI);
                //if (a?.listSelectedSpecI != null)
                //    this.listSelectedSpec = string.Join(" ", (a.listSelectedSpecI).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
                //        OrderBy(x1 => int.Parse(x1.Split(new string[] { "SPEC" }, StringSplitOptions.RemoveEmptyEntries)[1])).ToList());
                //else
                //    this.listSelectedSpec = null;

                a.listSelectedVremI = Vrem.SortIds(a?.listSelectedVremI);
                //if (a?.listSelectedVremI != null)
                //    this.listSelectedVrem = string.Join(" ", (a.listSelectedVremI).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
                //        OrderBy(x1 => int.Parse(x1.Split(new string[] { "VREM" }, StringSplitOptions.RemoveEmptyEntries)[1])).ToList());
                //else
                //    this.listSelectedVrem = null;


                a.Valide = true;
                
            }
            //a.Valide = false;
            return true;
        }


        


    }

    public class DescrSearchI
    {
       
        public string ActionId { get; set; }
        public bool? Parametric { get; set; }
        public string ActionType { get; set; }
        public string FizVelId { get; set; }
        public string ParametricFizVelId { get; set; }
        public string ListSelectedPros { get; set; }
        public string ListSelectedSpec { get; set; }
        public string ListSelectedVrem { get; set; }


        public DescrSearchI()
        {
            Parametric = null;
        }
        /// <summary>
        /// параметр может измениться
        /// </summary>
        /// <param name="a"></param>
        public  DescrSearchI(DescrSearchIInput a)
        {

            var valid = false;
            if (a?.Valide == null)
                valid = DescrSearchIInput.Validation(a);
            //TODO
            //if (!valid)
            //    throw new Exception("валидация");

            this.ActionId = a?.actionIdI;
            this.ActionType = a?.actionTypeI;
            this.FizVelId = a?.FizVelIdI;
            this.ParametricFizVelId = a?.parametricFizVelIdI;
            this.ListSelectedPros = a?.listSelectedProsI;
            this.ListSelectedSpec = a?.listSelectedSpecI;
            this.ListSelectedVrem = a?.listSelectedVremI;


        }

        /// <summary>
        /// параметр может измениться
        /// </summary>
        /// <param name="a"></param>
        public DescrSearchI(DescrSearchIOut a)
        {
            var valid = false;
            if (a?.Valide == null)
                valid=DescrSearchIOut.Validation(a);
            //TODO
            //if (!valid)
            //    throw new Exception("валидация");

            this.ActionId = a?.actionIdO;
            this.ActionType = a?.actionTypeO;
            this.FizVelId = a?.FizVelIdO;
            this.ParametricFizVelId = a?.parametricFizVelIdO;
            this.ListSelectedPros = a?.listSelectedProsO;
            this.ListSelectedSpec = a?.listSelectedSpecO;
            this.ListSelectedVrem = a?.listSelectedVremO;

        }




        public bool? CheckParametric()
        {
             this.Parametric=AllAction.CheckParametric(this.ActionId);
            return this.Parametric;
        }



        public static bool IsNull(DescrSearchI a)
        {
            if (a == null)
                return true;
            if (string.IsNullOrWhiteSpace( a.ActionId ) && string.IsNullOrWhiteSpace(a.ActionType)  && string.IsNullOrWhiteSpace(a.FizVelId )
                && string.IsNullOrWhiteSpace(a.ParametricFizVelId) && string.IsNullOrWhiteSpace(a.ListSelectedPros )
                && string.IsNullOrWhiteSpace(a.ListSelectedSpec)  && string.IsNullOrWhiteSpace(a.ListSelectedVrem ))
                return true;


            return false;
        }


        public void DeleteNotChildCheckbox()
        {

            //pro
            this.ListSelectedPros = Pro.DeleteNotChildCheckbox(this.ListSelectedPros);



            //spec
            this.ListSelectedSpec = Spec.DeleteNotChildCheckbox(this.ListSelectedSpec);



            //vrem
            this.ListSelectedVrem = Vrem.DeleteNotChildCheckbox(this.ListSelectedVrem);


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

        [ScaffoldColumn(false)]
        public bool? Valide { get; private set; }

        public DescrSearchIOut()
        {


            Valide = null;
        }
        public DescrSearchIOut(FEAction a)
        {
            
            this.actionIdO = a.Name;
            this.actionTypeO = a.Type;
            this.FizVelIdO = a.FizVelId;
            this.listSelectedProsO = a.Pros;
            this.listSelectedSpecO = a.Spec;
            this.listSelectedVremO = a.Vrem;
            this.parametricFizVelIdO = a.FizVelSection;

        }



        public static bool ValidationIfNeed(DescrSearchIOut a)
        {
            var res = a?.Valide ?? DescrSearchIOut.Validation(a);
            //var res = a.Valide;
            //if (a.Valide == null)
            //    res = DescrSearchIOut.Validation(a);
            return res;
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


                a.listSelectedProsO = Pro.SortIds(a?.listSelectedProsO);
                //if (a?.listSelectedProsO != null)
                //    this.listSelectedPros = string.Join(" ", (a.listSelectedProsO).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
                //        OrderBy(x1 => int.Parse(x1.Split(new string[] { "PROS" }, StringSplitOptions.RemoveEmptyEntries)[1])).ToList());
                //else
                //    this.listSelectedPros = null;

                a.listSelectedSpecO = Spec.SortIds(a?.listSelectedSpecO);
                //if (a?.listSelectedSpecO != null)
                //    this.listSelectedSpec = string.Join(" ", (a.listSelectedSpecO).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
                //        OrderBy(x1 => int.Parse(x1.Split(new string[] { "SPEC" }, StringSplitOptions.RemoveEmptyEntries)[1])).ToList());
                //else
                //    this.listSelectedSpec = null;

                a.listSelectedVremO = Vrem.SortIds(a?.listSelectedVremO);
                //if (a?.listSelectedVremO != null)
                //    this.listSelectedVrem = string.Join(" ", (a.listSelectedVremO).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
                //        OrderBy(x1 => int.Parse(x1.Split(new string[] { "VREM" }, StringSplitOptions.RemoveEmptyEntries)[1])).ToList());
                //else
                //    this.listSelectedVrem = null;

              


                


                a.Valide = true;
                
            }
            //a.Valide = false;
            return true;
        }

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



    public class FullTextSearch_
    {


        public static bool Create()
        {
            //string sqlConnectionString = Constants.sql_0;



            //SqlConnection conn = new SqlConnection(sqlConnectionString);

            //Server server = new Server(new ServerConnection(conn));//Microsoft.SqlServer.

            //string script = File.ReadAllText(HostingEnvironment.MapPath("~/tsqlscripts/create_catalog.txt"));

            //server.ConnectionContext.ExecuteNonQuery(script);
            //script = File.ReadAllText(HostingEnvironment.MapPath("~/tsqlscripts/create_index.txt"));
            //server.ConnectionContext.ExecuteNonQuery(script);


            //1-создать каталог
            //2- добавить индекс
            //3- апнуть индекс до семантического
             List<string> files = new List<string>() { "create_catalog", "create_index", "alter_index_semantic" };

            var connection = new SqlConnection();
            connection.ConnectionString = Constants.sql_0;
            connection.Open();
            foreach (var i in files)
            {
                string script = File.ReadAllText(HostingEnvironment.MapPath($"~/tsqlscripts/{i}.txt"));
                
                using (var command = new SqlCommand(script, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
            return true;
        }
    }



    class JsonSaveDescription
    {
        public string Id { get; set; }
        public string NewId { get; set; }
        public string ParentId { get; set; }
        public int Type { get; set; }
        public string Text { get; set; }
        public bool Parametric { get; set; }
        public int TypeAction { get; set; }


        public JsonSaveDescription()
        {
            NewId = null;
        }
    }



}