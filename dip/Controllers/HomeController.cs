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
using dip.Models;
using dip.Models.ViewModel.HomeV;

namespace dip.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                //db.Database.ExecuteSqlCommand(@"EXEC initFullTextSearch;");
                //db.SaveChanges();
            }

            //string Kappa = true.ToString();


            return View();
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult MainHeader()//string textSearchStr, string textSearchType
        {
             

            MainHeaderV res = new MainHeaderV();
            res.SearchList= new List<string>() { "lucene", "fullTextSearchF", "fullTextSearchCf", "fullTextSearchCl" };

            //ViewBag.textSearchStr = textSearchStr;
            //ViewBag.textSearchType = ViewBag;
            return PartialView(res);
        }

        public ActionResult MainFooter()
        {

            return PartialView();
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