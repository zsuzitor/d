using dip.Models;
using dip.Models.Domain;
using dip.Models.ViewModel.ListPhysicsV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Controllers
{
    [Authorize(Roles = "admin")]
    public class ListPhysicsController : Controller
    {
        // GET: ListPhysics
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListUserAct()
        {
            //ListActV res = new ListActV();
            //res.Lists = ListPhysics.GetAll();

            return View();
        }

        public ActionResult ListAct(int? currentListId=null)
        {
            ListActV res = new ListActV() {CurrentListId= currentListId };
            res.Lists = ListPhysics.GetAll();

            return View(res);
        }

        [HttpPost]
        public ActionResult CreateList(string name)
        {
            if(string.IsNullOrWhiteSpace(name)|| name.Length<5)//TODO в метод валидации
                return new HttpStatusCodeResult(404);//TODO сообщение об ошибке
            ListPhysics res = ListPhysics.Create(name);

            return PartialView(res);
        }

        public ActionResult LoadPhysInList(int id)
        {
            ListPhysics res = ListPhysics.LoadPhysics(id);

            return PartialView(res);
        }


        public ActionResult LoadUsersForList(int id)
        {
            ListPhysics res = ListPhysics.LoadUsers(id);

            return PartialView(res);
        }


        [HttpPost]
        public ActionResult EditList(int id, string name)
        {
            ListPhysics res=ListPhysics.Edit(id, name);

            return PartialView(res);
        }

        [HttpPost]
        public ActionResult DeleteList(int id)
        {
            ListPhysics res = ListPhysics.Delete(id);

            return PartialView(res);
        }

        [HttpPost]
        public ActionResult AddToList(int idphys,int idlist)
        {
            ListPhysics res =ListPhysics.AddPhys(idphys, idlist);

            return PartialView(res?.Physics.FirstOrDefault(x1=>x1.IDFE== idphys));
        }

        [HttpPost]
        public ActionResult DeleteFromList(int idphys, int idlist)
        {

            ListPhysics res =ListPhysics.DeletePhys(idphys, idlist);

            return PartialView(res);
        }



        [HttpPost]
        public ActionResult AssignListToUser(string iduser, int idlist)
        {

             ApplicationUser.AddList(iduser, idlist);

            return PartialView();
        }

        [HttpPost]
        public ActionResult RemoveListFromUser(string iduser, int idlist)
        {
            ApplicationUser.RemoveList(iduser, idlist);
           

            return PartialView();
        }

        [HttpPost]
        public ActionResult AssignPhysicToUser(string iduser, int idphys)
        {

            ApplicationUser.AddPhysics(iduser, idphys);
            
            return PartialView();
        }

        [HttpPost]
        public ActionResult RemovePhysicFromUser(string iduser, int idphys)
        {

            ApplicationUser.RemovePhysics(iduser, idphys);

            return PartialView();
        }


        
        public ActionResult AssignsUsersList(string iduser)
        {
            var user = ApplicationUser.GetUser(iduser);
            user?.LoadListPhysics();
            
            return PartialView(user?.ListPhysics);
        }
        public ActionResult AssignsUsersPhysics(string iduser)
        {
            var user = ApplicationUser.GetUser(iduser);
            user?.LoadPhysics();
            
            return PartialView(user?.Physics);
        }

    }
}