using dip.Models;
using dip.Models.ViewModel.PersonV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Controllers
{
    public class PersonController : Controller
    {
        // GET: Person
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult PersonalRecord(string personId)
        {
            PersonalRecordV res = new PersonalRecordV();
            res.CurrenUserId = ApplicationUser.GetUserId();
            if (string.IsNullOrWhiteSpace(personId))
                personId = res.CurrenUserId;
            res.User = ApplicationUser.GetUser(personId);


            
            return View(res);
        }
    }
}