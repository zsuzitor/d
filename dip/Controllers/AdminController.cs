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

        //public ActionResult ListAct()
        //{
        //    ListActV res = new ListActV();
        //    res.Lists=ListPhysics.GetAll();

        //    return View(res);
        //}

        //public ActionResult CreateList(string name)
        //{
        //    var res=ListPhysics.Create(name);

        //    return PartialView(res);
        //}

        //public ActionResult LoadPhysInList(int id)
        //{
        //    var res = ListPhysics.LoadPhysics(id);

        //    return PartialView(res);
        //}

        //public ActionResult EditList(int id,string name)
        //{
        //    ListPhysics.Edit(id,name);

        //    return PartialView();
        //}
        //public ActionResult DeleteList(int id, string name)
        //{
        //    ListPhysics.Delete(id);

        //    return PartialView();
        //}

        //public ActionResult AddToList(int id)
        //{
        //    ListPhysics.AddPhys(id);

        //    return PartialView();
        //}
        //public ActionResult DeleteFromList(int id)
        //{
        //    ListPhysics.DeletePhys(id);

        //    return PartialView();
        //}


    }
}