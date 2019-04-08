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
        /// <returns></returns>
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
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRole(string roleName,string userId)
        {
            

            var user = ApplicationUser.GetUser(userId);
            var role = (RolesProject)Enum.Parse(typeof(RolesProject), roleName, true);

            if(user==null)//TODO проверять существует ли роль
                return new HttpStatusCodeResult(404);
            using (var db=new ApplicationDbContext())
            {
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                userManager.AddToRole(user.Id, role.ToString());
                
            }


            return new HttpStatusCodeResult(200);
        }

        /// <summary>
        /// post-удаление роли пользователя
        /// </summary>
        /// <param name="roleName">роль</param>
        /// <param name="userId">id пользователя</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveRole(string roleName, string userId)
        {

            RolesProject role;
            var user = ApplicationUser.GetUser(userId);
            try
            {
                 role = (RolesProject)Enum.Parse(typeof(RolesProject), roleName, true);

            }
            catch {
                return new HttpStatusCodeResult(404);
            }
            
            if (user == null)
                return new HttpStatusCodeResult(404);
            using (var db = new ApplicationDbContext())
            {
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                userManager.RemoveFromRole(user.Id, role.ToString());

            }

            return new HttpStatusCodeResult(200);
        }


        /// <summary>
        /// получение списка всех пользователей
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllUsersShortData()
        {
            List<ApplicationUser> users = ApplicationUser.GetAllUsers();
            
            return PartialView(users);
        }
        /// <summary>
        /// получение списка всех пользователей , рядом с каждым кнопка
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllUsersShortDataWithBut()
        {
            List<ApplicationUser> users = ApplicationUser.GetAllUsers();



            return PartialView(users);
        }
        /// <summary>
        /// страница на которой можно выбрать дальнейшие действия
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminsPage()//, string technicalFunctionId
        {

            return View();
        }


        

    }
}