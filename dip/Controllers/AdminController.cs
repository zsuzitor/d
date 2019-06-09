using dip.Models;
using dip.Models.Domain;
using dip.Models.ViewModel.AdminV;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Controllers
{


    [Authorize(Roles = "admin")]
    [RequireHttps]

    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {

            return View();
        }
        /// <summary>
        /// страница для изменения ролей пользователей
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ChangeRole()
        {
            ChangeRoleV res = new ChangeRoleV();
            foreach (RolesProject roleName in (RolesProject[])Enum.GetValues(typeof(RolesProject)))
            {
                res.Roles.Add(roleName.ToString());

            }
            return View(res);
        }
        /// <summary>
        /// post- изменение проли пользователя
        /// </summary>
        /// <param name="roleName">роль</param>
        /// <param name="userId">id пользователя</param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        public ActionResult AddRole(string roleName, string userId)
        {

            RolesProject role;
            var user = ApplicationUser.GetUser(userId);
            try
            {
                role = (RolesProject)Enum.Parse(typeof(RolesProject), roleName, true);
            }
            catch
            {
                return new HttpStatusCodeResult(404);
            }

            if (user == null)//TODO проверять существует ли роль
                return Content("Пользователь не найден", "text/html");
            using (var db = new ApplicationDbContext())
            {
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                userManager.AddToRole(user.Id, role.ToString());

            }

            return Content("Роль успешно добавлена", "text/html");
            // return new HttpStatusCodeResult(200);
        }

        /// <summary>
        /// post-удаление роли пользователя
        /// </summary>
        /// <param name="roleName">роль</param>
        /// <param name="userId">id пользователя</param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        public ActionResult RemoveRole(string roleName, string userId)
        {

            RolesProject role;
            var user = ApplicationUser.GetUser(userId);
            try
            {
                role = (RolesProject)Enum.Parse(typeof(RolesProject), roleName, true);

            }
            catch
            {
                return new HttpStatusCodeResult(404);
            }

            if (user == null)
                return Content("Пользователь не найден", "text/html");
            using (var db = new ApplicationDbContext())
            {
                string roleStr = role.ToString();//что бы точно быть уверенным что будем сравнивать с правильным значением
                if (role == RolesProject.admin)
                {

                    string idAdminRole = db.Roles.FirstOrDefault(x1 => x1.Name == roleStr)?.Id;
                    int adminsCount = db.Users.Where(x1 => x1.Roles.FirstOrDefault(x2 => x2.RoleId == idAdminRole) != null).Count();
                    if (adminsCount < 2)
                        return Content("это последний администратор,удаление отменено", "text/html");
                    //var allusers = db.Users.ToList();
                    //var users = allusers.Where(x => x.Roles.Select(x1 => x1.Name).Contains("User")).ToList();
                }
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                userManager.RemoveFromRole(user.Id, roleStr);

            }
            return Content("Роль успешно удалена", "text/html");
            /* return new HttpStatusCodeResult(200);*/
        }


        /// <summary>
        /// получение списка всех пользователей
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult GetAllUsersShortData()
        {
            List<ApplicationUser> users = ApplicationUser.GetAllUsers();

            return PartialView(users);
        }
        /// <summary>
        /// получение списка всех пользователей , рядом с каждым кнопка
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult GetAllUsersShortDataWithBut()
        {
            List<ApplicationUser> users = ApplicationUser.GetAllUsers();



            return PartialView(users);
        }

        /// <summary>
        /// Возвращает id пользователя
        /// </summary>
        /// <param name="username">username пользователя для которого нужно определить id</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult GetUserIdByUsername(string username)
        {
            using (var db = new ApplicationDbContext())//TODO
            {
                var user = db.Users.FirstOrDefault(x1 => x1.UserName == username);
                if (user != null)
                {
                    return Content(user.Id, "text/html");
                }
                else
                    return new EmptyResult();
                //return Content("Не найдено", "text/html");
            }


        }

        /// <summary>
        ///  Возвращает список подходящих пользователей
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="surname">Фамилия</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult GetUserIdByFI(string name, string surname)
        {
            name = string.IsNullOrWhiteSpace(name) ? null : name;
            surname = string.IsNullOrWhiteSpace(surname) ? null : surname;
            List<ApplicationUser> users = null;
            using (var db = new ApplicationDbContext())//TODO
            {

                users = db.Users.Where(x1 => name != null ? x1.Name == name : true && surname != null ? x1.Surname == surname : true)
                   .Select(x1 => new { x1.Id, x1.UserName }).ToList().Select(x1 => new ApplicationUser() { Id = x1.Id, UserName = x1.UserName }).ToList();

                if (users.Count > 0)
                {
                    return PartialView(users);
                }
                else
                    return new EmptyResult();
            }
        }


        /// <summary>
        /// страница для просмотре информации и пользователях
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult UsersData()//, string technicalFunctionId
        {

            return View();
        }



        /// <summary>
        /// страница на которой можно выбрать дальнейшие действия
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult AdminsPage()//, string technicalFunctionId
        {

            return View();
        }


    }
}