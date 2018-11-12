using dip.Models;
using dip.Models.Domain;
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


        [HttpPost]
        public ActionResult TextSearch(string str)
        {
            //TODO полнотекстовый поиск

            using (var db=new ApplicationDbContext())
            {
                System.Data.SqlClient.SqlParameter param1 = new System.Data.SqlClient.SqlParameter("@searched_str", "Затухание");
                System.Data.SqlClient.SqlParameter param2 = new System.Data.SqlClient.SqlParameter("@max_lev", 5);
                var lst = db.Database.SqlQuery<test>("SELECT * FROM GetListLev (@searched_str,@max_lev)", param1, param2).ToList();
                

            }
                



            return View();
        }


        [Authorize(Roles = "admin")]
        public ActionResult Create(int? id)
        {
            //TODO страница добавления
            FEText res = new FEText() ;
            if (id != null)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    res = db.FEText.FirstOrDefault(x1 => x1.IDFE == id);
                    db.Entry(res).Collection(x1 => x1.Images).Load();
                }
            }
            return View(res);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(FEText obj, HttpPostedFileBase[] uploadImage, int[]deleteImg_)
        {
            //TODO добавление записи
            var list_img_byte = Get_photo_post(uploadImage);
            FEText oldObj = null;
            List<int> deleteImg = null;
            if (deleteImg_ != null)
                deleteImg = deleteImg_.Distinct().ToList();
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                
                //TODO валидация
                if (obj.IDFE != 0)
                    oldObj = db.FEText.FirstOrDefault(x1 => x1.IDFE == obj.IDFE);
                if (oldObj == null)
                {
                    //новая
                    db.FEText.Add(obj);
                    db.SaveChanges();
                    //foreach(var i in list_img_byte)
                    //{
                    //    db.Images.Add(new Image() { Data=i, FeTextId= obj.IDFE });
                    //}
                    //db.SaveChanges();
                }
                else
                {
                    //обновляем
                    oldObj.Equal(obj);
                    if(deleteImg!=null&& deleteImg.Count > 0)
                    {
                        var imgs = db.Images.Where(x1 => x1.FeTextIDFE == oldObj.IDFE).
                                                Join(deleteImg, x1 => x1.Id, x2 => x2, (x1, x2) => x1).ToList();//Where(x1 => x1.FeTextId == obj.IDFE);
                        db.Images.RemoveRange(imgs);
                        db.SaveChanges();
                    }
                    
                    //foreach (var i in list_img_byte)
                    //{
                    //    db.Images.Add(new Image() { Data = i, FeTextId = obj.IDFE });
                    //}
                    //db.SaveChanges();
                }
                foreach (var i in list_img_byte)
                {
                    db.Images.Add(new Image() { Data = i, FeTextIDFE = oldObj.IDFE });//FeTextId = oldObj.IDFE    //  FeText= oldObj
                }
                db.SaveChanges();

                if (!db.Entry(oldObj).Collection(x1 => x1.Images).IsLoaded)
                    db.Entry(oldObj).Collection(x1 => x1.Images).Load();
            }



                return View(@"~/Views/DescriptionQueries/Details.cshtml", oldObj);
        }

    }

    

}