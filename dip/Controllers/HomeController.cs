using dip.Models.DataBase;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;

//
using System.Configuration;
using System.Data.SqlClient;
using dip.Models.Domain;

namespace dip.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }












        //public ActionResult ReadDbOld()
        //{
        //    //using (OldDbContext db=new OldDbContext())
        //    //{
        //    //    //var comps = db.Database.ExecuteSqlCommand("SELECT * FROM Companies");
        //    //    var t=db.ActionPros.Select(x1=>x1).ToList();
        //    //    int f = 10;
        //    //}


        //    try
        //    {
        //        string lastname = null;
        //        string firstname = null;
        //        string age = null;

        //        OldData db = new OldData();

        //        bool status = db.GetUsersData(ref lastname, ref firstname, ref age);
        //        if (status)
        //        {
        //            var f = 10;
        //        }
        //    }
        //    catch
        //    {

        //    }



        //    return View();
        //}



        public ActionResult DecriptSearch()
        {
            //TODO
            return View();
        }


        public ActionResult TextSearch()
        {
            //TODO
            return View();
        }







        [Authorize(Roles = "admin")]
        public ActionResult ReloadDataBase()
        {
            //OldData.ReloadDataBase();



            return View();
        }




        //TODO old method for check view
        public ActionResult CreateInput()
        {

            //ViewBag.vrem   --- mass
            //ViewBag.spec   --- mass
            //ViewBag.pros   --- mass

            //ViewBag.parametricFizVelId   ---mass


            ///ViewBag.currentAction  --str
            ///ViewBag.currentActionId  --- int

            return View();
        }

    }
}