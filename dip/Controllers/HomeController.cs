using dip.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }












        public ActionResult ReadDbOld()
        {
            using (OldDbContext db=new OldDbContext())
            {
                //var comps = db.Database.ExecuteSqlCommand("SELECT * FROM Companies");
                var t=db.ActionPros.Select(x1=>x1).ToList();
                int f = 10;
            }




                return View();
        }

    }
}