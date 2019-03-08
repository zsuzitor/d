﻿using dip.Models.Domain;
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



        public ActionResult ListAct()
        {
            ListActV res = new ListActV();
            res.Lists = ListPhysics.GetAll();

            return View(res);
        }

        [HttpPost]
        public ActionResult CreateList(string name)
        {
            var res = ListPhysics.Create(name);

            return PartialView(res);
        }

        public ActionResult LoadPhysInList(int id)
        {
            var res = ListPhysics.LoadPhysics(id);

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
            var res=ListPhysics.AddPhys(idphys, idlist);

            return PartialView(res?.Physics.FirstOrDefault(x1=>x1.IDFE== idphys));
        }

        [HttpPost]
        public ActionResult DeleteFromList(int idphys, int idlist)
        {
           
            ListPhysics.DeletePhys(idphys, idlist);

            return PartialView();
        }








    }
}