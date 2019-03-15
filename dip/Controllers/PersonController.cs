using dip.Models;
using dip.Models.Domain;
using dip.Models.ViewModel.PersonV;
using dip.Models.ViewModel.PhysicV;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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

        [Authorize]
        public ActionResult PersonalRecord(string personId)
        {
            PersonalRecordV res = new PersonalRecordV();
            res.CurrenUserId = ApplicationUser.GetUserId();
            

            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(res.CurrenUserId);
           
                res.Admin = roles.Contains("admin");
            if (string.IsNullOrWhiteSpace(personId))
                personId = res.CurrenUserId;

            if (res.Admin)
                res.User = ApplicationUser.GetUser(personId);

            //res.User.CheckAccessPhys(HttpContext);

            else if (personId!= res.CurrenUserId)
                return RedirectToAction("PersonalRecord",new { personId= res.CurrenUserId });
            //res.User = ApplicationUser.GetUser(res.CurrenUserId);



            return View(res);
        }


        [Authorize(Roles = "admin")]
        public ActionResult GetRoles(string personId)
        {
            List<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(personId).ToList();

            return PartialView(roles);
        }



        /// <summary>
        /// добавление в избранное
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FavouritePhysics(int id)
        {
            bool res = false;

            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            if (user != null)
            {
                List<int> accessList= user.CheckAccessPhys(new List<int>() {id },HttpContext);
                if(accessList.Count==0)
                    return PartialView(res);
                var fetext = FEText.Get(id);
                if (fetext == null)
                    return new HttpStatusCodeResult(404);
                res = fetext.ChangeFavourite(user.Id);
            }


            return PartialView(res);
        }



        public ActionResult ListFavouritePhysics(string personId)
        {
            ListFeTextV res = new ListFeTextV();
            string personId_ = personId ?? ApplicationUser.GetUserId();

            var user = ApplicationUser.GetUser(personId_);
            user.LoadFavouritedList();
            res.FeTexts = user.FavouritedPhysics.ToList();
            if (personId==null||personId == personId_)
                foreach (var i in res.FeTexts)
                {
                    i.FavouritedCurrentUser = true;
                }
            //return PartialView("ListFeText","",);
            return PartialView(res);
        }
    }
}