using dip.Models;
using dip.Models.Domain;
using dip.Models.ViewModel;
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
    public class PhysicController : Controller
    {
        // GET: Physic
        public ActionResult Index()
        {
            return View();
        }


        


        public ActionResult Details(int id)//, string technicalFunctionId
        {
            FEText effect= FEText.Get(id);
            string check_id = ApplicationUser.GetUserId();

            effect.LoadImage();
            ViewBag.EffectName = effect.Name;
            ViewBag.TechnicalFunctionId = Request.Params.GetValues(0).First();
            

            ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                            .GetUserManager<ApplicationUserManager>();
            IList<string> roles = userManager.GetRoles(check_id);

            if (roles.Contains("admin"))
                ViewBag.Admin = true;


            return View(effect);
        }



        public ActionResult ListFeText(int[] listId)
        {

            List<FEText> res = new List<FEText>();
            res = FEText.GetList(listId);



            return PartialView(res);
        }




        [Authorize(Roles = "admin")]
        public ActionResult Create(int? id)
        {
            //TODO страница добавления
            FEText res = new FEText() ;
            //TODO мб убрать условие, но надо потестить тк там редактирование
            if (id != null)
            {
                
                    res = FEText.Get(id);
                    res.LoadImage();
                    
                
            }
            return View(res);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(FEText obj, HttpPostedFileBase[] uploadImage, int[] deleteImg_, DescrSearchIInput inp = null, DescrSearchIOut outp = null)
        {
            //TODO добавление записи
            var list_img_byte = Get_photo_post(uploadImage);
            FEText oldObj = null;
            List<int> deleteImg = null;
            if (deleteImg_ != null)
                deleteImg = deleteImg_.Distinct().ToList();
            //using (ApplicationDbContext db=new ApplicationDbContext())
            //{

            //TODO валидация
            if (obj.IDFE != 0)
                oldObj = FEText.Get(obj.IDFE);
            if (oldObj == null)
            {
                //новая
                obj.AddToDb(inp, outp);
                //foreach(var i in list_img_byte)
                //{
                //    db.Images.Add(new Image() { Data=i, FeTextId= obj.IDFE });
                //}
                //db.SaveChanges();
            }
            else
            {
                //обновляем
                oldObj.ChangeDb(obj, deleteImg);
                //oldObj.Equal(obj);
                //if(deleteImg!=null&& deleteImg.Count > 0)
                //{
                //    var imgs = db.Images.Where(x1 => x1.FeTextIDFE == oldObj.IDFE).
                //                            Join(deleteImg, x1 => x1.Id, x2 => x2, (x1, x2) => x1).ToList();//Where(x1 => x1.FeTextId == obj.IDFE);
                //    db.Images.RemoveRange(imgs);
                //    db.SaveChanges();
                //}

                //foreach (var i in list_img_byte)
                //{
                //    db.Images.Add(new Image() { Data = i, FeTextId = obj.IDFE });
                //}
                //db.SaveChanges();
            }
            oldObj.AddImages(list_img_byte);
            //foreach (var i in list_img_byte)
            //{
            //    db.Images.Add(new Image() { Data = i, FeTextIDFE = oldObj.IDFE });//FeTextId = oldObj.IDFE    //  FeText= oldObj
            //}
            //db.SaveChanges();
            oldObj.LoadImage();
            //if (!db.Entry(oldObj).Collection(x1 => x1.Images).IsLoaded)
            //    db.Entry(oldObj).Collection(x1 => x1.Images).Load();
            //}



            return View(@"~/Views/Physic/Details.cshtml", oldObj);
        }





















        //-------------------------------------------------------------------------------------------------------------------------------------------












    }

    

}