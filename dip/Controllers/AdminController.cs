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
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult AddRole()
        {
            ChangeRoleV res = new ChangeRoleV();

            return View(res);
        }
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


            return View();
        }

        public ActionResult DeleteRole()
        {
            ChangeRoleV res = new ChangeRoleV();

            return View(res);
        }
        [HttpPost]
        public ActionResult DeleteRole(string roleName, string userId)
        {


            var user = ApplicationUser.GetUser(userId);
            var role = (RolesProject)Enum.Parse(typeof(RolesProject), roleName, true);

            if (user == null)//TODO проверять существует ли роль
                return new HttpStatusCodeResult(404);
            using (var db = new ApplicationDbContext())
            {
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                userManager.RemoveFromRole(user.Id, role.ToString());

            }


            return View();
        }


    }
}