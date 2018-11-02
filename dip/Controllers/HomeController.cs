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



using (ApplicationDbContext db = new ApplicationDbContext())
            {

            }

namespace dip.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }





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