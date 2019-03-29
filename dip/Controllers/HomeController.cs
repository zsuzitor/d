using dip.Models.DataBase;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;

//
using System.Configuration;
using System.Data.SqlClient;
using dip.Models.Domain;
using dip.Models;
using dip.Models.ViewModel.HomeV;
using System.Reflection;

namespace dip.Controllers
{
    public class HomeController : Controller
    {

        ////////List<FEObject> res = new List<FEObject>();
        public ActionResult Index()
        {
            //string[] mass = {"a","a1","a12","a9","ab1", "ab2", "ab10", "ab12", "aba1", "aba10", "ab9" };
            //mass.OrderBy(x1=>x1.com);
            //Array.Sort(mass);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //using (var transaction1 = db.Database.BeginTransaction())
                //{
                //    try
                //    {
                //        FEText fetext = new FEText() { Text = "11111111", Name = "111111111111" };
                //        db.FEText.Add(fetext);
                //        db.SaveChanges();
                //        using (var transaction2 = db.Database.BeginTransaction())
                //        {
                //            try
                //            {
                //                FEText fetext2 = new FEText() { Text = "11111111", Name = "111111111111" };
                //                db.FEText.Add(fetext2);
                //                db.SaveChanges();
                //                transaction2.Commit();
                //            }
                //            catch (Exception ex)
                //            {
                //                transaction2.Rollback();
                //            }
                //        }
                //        transaction1.Commit();
                //        //transaction1.Rollback();
                //        //transaction.Commit();
                //    }
                //    catch (Exception ex)
                //    {
                //        transaction1.Rollback();
                //    }

                //}


                //DeleteFromDb(db,db.Pros, new List<Pro>() { db.Pros.FirstOrDefault() });

            }



            return View();
        }


        //private void DeleteFromDb<T>(ApplicationDbContext db, System.Data.Entity.DbSet<T> collect, List<T> del) where T : ItemFormCheckbox<T>, new()
        //{
          
        //    Type typeT = typeof(T);
        //    MethodInfo meth = typeT.GetMethod("GetChild");
        //    List<T> forDeleted = new List<T>();
          
            
        //    int start = 0;

        //    foreach (var i in collect.Where(x1 => del.FirstOrDefault(x2 => x2.Id == x1.Id) != null&&x1.Parent != Constants.FeObjectBaseCharacteristic).ToList())
        //    {

        //        forDeleted.Add(i);
                
        //        for (; start < forDeleted.Count; ++start)
        //        {
        //            var gg = new object[] { forDeleted[start].Id };
        //            forDeleted.AddRange((List<T>)meth.Invoke(null, gg));
        //        }
        //    }

        //    collect.RemoveRange(forDeleted);
        //    db.SaveChanges();
        //}




        //private void DeleteCharacteristic<T>(ApplicationDbContext db, System.Data.Entity.DbSet<T> collect, List<T> del) where T : ItemFormCheckbox<T>, new()
        //{
        //    //запрещать удалять мейн характеристики
        //    //var db = db_ ?? new ApplicationDbContext();
        //    // -----
        //    Type typeT = typeof(T);
        //    MethodInfo meth = null;
        //    List<T> forDeleted = new List<T>();
        //    foreach (var i in typeT.GetMethods())
        //    {
        //        if (i.Name == "GetChild")
        //        {
        //            //i.Invoke(null, forDeleted[start].Id);
        //            meth = i;
        //            break;
        //        }
        //    }
        //    int start = 0;
        //    foreach (var i in del)
        //    {
        //        var pro = collect.FirstOrDefault(x1 => x1.Id == i.Id);

        //        if (pro == null || pro.Parent == Constants.FeObjectBaseCharacteristic)
        //            continue;
        //        forDeleted.Add(pro);
        //        //var childs=Pro.GetChild(pro.Id);
        //        //    forDeleted.AddRange(childs);
        //        for (; start < forDeleted.Count; ++start)
        //        {
        //            var gg = new object[] { forDeleted[start].Id };
        //            forDeleted.AddRange(meth.Invoke(null, (object[])gg));
        //        }
        //    }
        //    collect.RemoveRange(forDeleted);
        //    db.SaveChanges();




        //    //if (db_ == null)
        //    // db.Dispose();
        //}




        public ActionResult About()
        {
            return View();
        }

        public ActionResult MainHeader()//string textSearchStr, string textSearchType
        {
             

            MainHeaderV res = new MainHeaderV();
            res.SearchList= new List<string>() { "lucene", "fullTextSearchF", "fullTextSearchCf", "fullTextSearchCl" };

            //ViewBag.textSearchStr = textSearchStr;
            //ViewBag.textSearchType = ViewBag;
            return PartialView(res);
        }

        public ActionResult MainFooter()
        {

            return PartialView();
        }

        public ActionResult DecriptSearch()
        {
            //TODO
            return View();
        }


        public ActionResult TextSearch()
        {
            //TODO
            return View();
        }







        //[Authorize(Roles = "admin")]
        //public ActionResult ReloadDataBase()
        //{
        //    //OldData.ReloadDataBase();

            

        //    return View();
        //}




        //TODO old method for check view
        //public ActionResult CreateInput()
        //{

         

        //    return View();
        //}

    }
}