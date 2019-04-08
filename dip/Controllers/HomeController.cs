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
using System.Reflection;
using System.Data;

namespace dip.Controllers
{
    [RequireHttps]
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

        public ActionResult MainHeader()
        {
             

            MainHeaderV res = new MainHeaderV();
            res.SearchList= new List<string>() { "lucene", "fullTextSearchF", "fullTextSearchCf", "fullTextSearchCl" };

            return PartialView(res);
        }

        public ActionResult MainFooter()
        {

            return PartialView();
        }


    }
}