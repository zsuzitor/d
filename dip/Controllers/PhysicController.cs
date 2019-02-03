﻿using dip.Models;
using dip.Models.Domain;
using dip.Models.ViewModel;
using dip.Models.ViewModel.PhysicV;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static dip.Models.Functions;

namespace dip.Controllers
{


    //TODO везде проверка на закрытый профиль
    public class PhysicController : Controller
    {
        // GET: Physic
        public ActionResult Index()
        {
            return View();
        }


        


        public ActionResult Details(int? id)//, string technicalFunctionId
        {
            DetailsV res = new DetailsV();
            res.Effect = FEText.Get(id);
            if(res.Effect == null)
                return new HttpStatusCodeResult(404);
            string check_id = ApplicationUser.GetUserId();

            res.Effect.LoadImage();
            res.EffectName = res.Effect.Name;
            //TODO почему именно так?
            res.TechnicalFunctionId = Request.Params.GetValues(0).First();

            if (check_id != null)
            {
                ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>();
                IList<string> roles = userManager?.GetRoles(check_id);
                if (roles != null)
                    if (roles.Contains("admin"))
                        res.Admin = true;
                res.Favourited=res.Effect.Favourited(check_id);
            }
         


            return View(res);
        }

        public ActionResult ShowSimilar(int id)//, string technicalFunctionId
        {
            ShowSimilarV res = new ShowSimilarV();
            res.ListSimilarIds = FEText.GetListSimilar(id);

            
            return PartialView(res);
        }


        public ActionResult ListFeText(int[] listId=null,int numLoad=1)
        {
            ListFeTextV res = new ListFeTextV();
            
            if (listId == null)
            {
                listId = (int[])TempData["list_fe_id"];
            }
            res.FeTexts = FEText.GetList(listId);
            res.NumLoad = numLoad;


            return PartialView(res);
        }


        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            EditV res = new EditV();
            res.Obj = FEText.Get(id);
            
            if(res==null)
                return new HttpStatusCodeResult(404);

            FEAction inp = null;
            FEAction outp = null;
            FEAction.Get((int)id, ref inp, ref outp);

            res.FormInput = new DescrSearchIInput(inp);
            res.FormOutput = new DescrSearchIOut(outp);

            //TODO
            res.FormInput.listSelectedProsI = Pro.GetAllIdsFor(res.FormInput.listSelectedProsI);
            res.FormInput.listSelectedVremI = Vrem.GetAllIdsFor(res.FormInput.listSelectedVremI);
            res.FormInput.listSelectedSpecI = Spec.GetAllIdsFor(res.FormInput.listSelectedSpecI);

            res.FormOutput.listSelectedProsO = Pro.GetAllIdsFor(res.FormOutput.listSelectedProsO);
            res.FormOutput.listSelectedVremO = Vrem.GetAllIdsFor(res.FormOutput.listSelectedVremO);
            res.FormOutput.listSelectedSpecO = Spec.GetAllIdsFor(res.FormOutput.listSelectedSpecO);


            

            res.Obj.LoadImage();


            
            return View(res);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(FEText obj, HttpPostedFileBase[] uploadImage, int[] deleteImg_, DescrSearchIInput inp = null, DescrSearchIOut outp = null)
        {
            
            if(!ModelState.IsValid)
                return new HttpStatusCodeResult(404);

           
                DescrSearchIInput.ValidationIfNeed(inp);
            
                DescrSearchIOut.ValidationIfNeed(outp);
            if (inp?.Valide == false || outp?.Valide == false)
                return new HttpStatusCodeResult(404);


            var list_img_byte = Get_photo_post(uploadImage);
            
            List<int> deleteImg = null;
            if (deleteImg_ != null)
                deleteImg = deleteImg_.Distinct().ToList();
            //using (ApplicationDbContext db=new ApplicationDbContext())
            //{

            //TODO валидация

            FEText oldObj = FEText.Get(obj.IDFE);
            if (oldObj == null)
                return new HttpStatusCodeResult(404);

            DescrSearchI inp_ = new DescrSearchI(inp);
            inp_.DeleteNotChildCheckbox();
            DescrSearchI outp_ = new DescrSearchI(outp);
            outp_.DeleteNotChildCheckbox();

            if (!oldObj.ChangeDb(obj, deleteImg, list_img_byte, inp_, outp_))
                return new HttpStatusCodeResult(404);
            Lucene_.UpdateDocument(obj.IDFE.ToString(),obj);

            //oldObj.LoadImage();
            //return View(@"~/Views/Physic/Details.cshtml", oldObj);
            return RedirectToAction("Details", "Physic",new {id= oldObj.IDFE });
        }


        




        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            
            FEText res = new FEText() ;
            
            
            return View(res);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(FEText obj, HttpPostedFileBase[] uploadImage,  DescrSearchIInput inp = null, DescrSearchIOut outp = null)
        {

            //if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            //{
            //    var pic = System.Web.HttpContext.Current.Request.Files["uploadImage[0]"];
            //}
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(404);

            if (!obj.Validation())
                return new HttpStatusCodeResult(404);

            
                DescrSearchIInput.ValidationIfNeed(inp);
            
                DescrSearchIOut.ValidationIfNeed(outp);
            if (inp?.Valide == false || outp?.Valide == false)
                return new HttpStatusCodeResult(404);

            DescrSearchI inp_ = new DescrSearchI(inp);
            inp_.DeleteNotChildCheckbox();
            DescrSearchI outp_ = new DescrSearchI(outp);
            outp_.DeleteNotChildCheckbox();

            var list_img_byte = Get_photo_post(uploadImage);
            
           
                //новая
                obj.AddToDb(inp_, outp_, list_img_byte);



            //obj.LoadImage();
            //return View(@"~/Views/Physic/Details.cshtml", oldObj);
            return RedirectToAction("Details", "Physic", new { id = obj.IDFE });
        }












        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult CreateSomething()//, string technicalFunctionId
        {

            return View();
        }



        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult CreateDescription()//, string technicalFunctionId
        {
            DescriptionForm res = new DescriptionForm();
            res= DescriptionForm.GetFormObject(null,null);
            return View(res);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult CreateDescription(string g)//, string technicalFunctionId
        {

            return View();
        }




        //-------------------------------------------------------------------------------------------------------------------------------------------












    }

    

}