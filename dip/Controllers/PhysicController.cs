using dip.Models;
using dip.Models.Domain;
using System;
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



        public ActionResult TextSearch(string str)
        {
            //TODO полнотекстовый поиск
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
        public ActionResult Create(FEText obj, HttpPostedFileBase[] uploadImage, int[]deleteImg)
        {
            //TODO добавление записи
            var list_img_byte = Get_photo_post(uploadImage);

            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                


                //TODO валидация
                var oldObj = db.FEText.FirstOrDefault(x1=>x1.IDFE==obj.IDFE);
                if (oldObj == null)
                {
                    //новая
                    db.FEText.Add(obj);
                    db.SaveChanges();
                    foreach(var i in list_img_byte)
                    {
                        db.Images.Add(new Image() { Data=i, FeTextId= obj.IDFE });
                    }
                    db.SaveChanges();
                }
                else
                {
                    //обновляем
                    oldObj.Equal(obj);
                    var imgs = db.Images.Where(x1 => x1.FeTextId == obj.IDFE).
                        Join(deleteImg, x1 => x1.Id, x2 => x2, (x1, x2) => x1).ToList();//Where(x1 => x1.FeTextId == obj.IDFE);
                    db.Images.RemoveRange(imgs);
                    db.SaveChanges();
                    foreach (var i in list_img_byte)
                    {
                        db.Images.Add(new Image() { Data = i, FeTextId = obj.IDFE });
                    }
                    db.SaveChanges();
                }
                if (!db.Entry(obj).Collection(x1 => x1.Images).IsLoaded)
                    db.Entry(obj).Collection(x1 => x1.Images).Load();
            }



                return View(obj);
        }

    }

    

}