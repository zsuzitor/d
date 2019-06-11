/*файл контроллера, отвечающий за действия связанные с учетной записью пользователя
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/


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
    [RequireHttps]
    public class PersonController : Controller
    {
        // GET: Person
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Отрисовывает страницу пользователя
        /// </summary>
        /// <param name="personId">id пользователя чью страницу запрашивают для отображения</param>
        /// <returns>результат действия ActionResult</returns>
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


            if (!res.Admin && personId != res.CurrenUserId)
                return RedirectToAction("PersonalRecord", new { personId = res.CurrenUserId });
            //res.User = ApplicationUser.GetUser(res.CurrenUserId);
            res.User = ApplicationUser.GetUser(personId);
            if (res.User == null)
                return new HttpStatusCodeResult(404);

            return View(res);
        }

        /// <summary>
        ///  Отрисовывает страницу пользователя для редактирования
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        [Authorize]
        public ActionResult EditPersonalRecord()
        {
            var res = ApplicationUser.GetUser(ApplicationUser.GetUserId());

            return View(res);
        }

        /// <summary>
        /// post- сохраняет изменения пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        [Authorize]
        public ActionResult EditPersonalRecord(ApplicationUser user)
        {
            user.Id = ApplicationUser.GetUserId();
            ApplicationUser.Edit(user);

            return Content("Сохранено", "text/html");
        }

        /// <summary>
        /// Отрисовывает список ролей пользователя
        /// </summary>
        /// <param name="personId">id пользователя</param>
        /// <returns>результат действия ActionResult</returns>
        [Authorize(Roles = "admin")]
        public ActionResult GetRoles(string personId)
        {
            List<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(personId).ToList();

            return PartialView(roles);
        }



        /// <summary>
        /// post-добавление в избранное ФЭ
        /// </summary>
        /// <param name="id"></param>
        /// <returns>результат действия ActionResult</returns>
        [Authorize]
        [HttpPost]
        public ActionResult FavouritePhysics(int id)
        {
            bool? res = false;
            if (id == Models.Constants.FEIDFORSEMANTICSEARCH)//id временной записи для сематического поиска у нее нет дескрипторов и text=="---"
                return PartialView(null);

            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            if (user != null)
            {
                List<int> accessList = user.CheckAccessPhys(new List<int>() { id }, HttpContext);
                if (accessList.Count == 0)
                    return PartialView(null);
                //return PartialView(res);
                var fetext = FEText.Get(id);
                if (fetext == null)
                    return PartialView(null);
                //return new HttpStatusCodeResult(404);
                res = fetext.ChangeFavourite(user.Id, HttpContext);
            }


            return PartialView(res);
        }


        /// <summary>
        /// Отрисовывает список фэ пользователя добавленных в избранное 
        /// </summary>
        /// <param name="personId">id пользователя</param>
        /// <returns>результат действия ActionResult</returns>
        [Authorize]
        public ActionResult ListFavouritePhysics(string personId)
        {
            ListFeTextV res = new ListFeTextV();
            string currenUserId = ApplicationUser.GetUserId();

            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(currenUserId);


            if (string.IsNullOrWhiteSpace(personId))
                personId = currenUserId;

            bool admin = roles.Contains("admin");
            if (!admin)
                if (personId != currenUserId)
                    //personId = currenUserId;
                    return RedirectToAction("ListFavouritePhysics", new { personId = currenUserId });


            var user = ApplicationUser.GetUser(personId);
            user.LoadFavouritedList();
            res.FeTexts = user.FavouritedPhysics;//.ToList();

            return PartialView(res);
        }
    }
}