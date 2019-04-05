﻿using dip.Models;
using dip.Models.Domain;
using dip.Models.ViewModel.SearchV;
using Lucene.Net.Analysis.Ru;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace dip.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }


        
        //TODO search- переименовать+ в js тоже поменять на partial
        public ActionResult DescriptionSearch(string search = null,string stateBegin=null, string stateEnd = null, DescrSearchI[] forms= null, DescrObjectI[] objForms=null)//, DescrSearchIInput inp_ = null, DescrSearchIOut outp_ = null
        {

            
            DescriptionSearchV res = new DescriptionSearchV();


            
            
            if (forms!=null&& objForms!=null)
            {
                foreach (var i in forms)
                {
                    DescrSearchI.Validation(i);
                    if (i?.Valide == false)
                        return new HttpStatusCodeResult(404);
                }
                foreach (var i in objForms)
                {
                    DescrObjectI.Validation(i);
                    if (i?.Valide == false)
                        return new HttpStatusCodeResult(404);
                }


                res.SearchList = FEText.GetByDescr(stateBegin, stateEnd, forms, objForms,HttpContext);

                res.FormInput = forms.Where(x1=>x1.InputForm).ToList();
                res.FormOutput = forms.Where(x1 => !x1.InputForm).ToList(); 
                res.FormObjectBegin = objForms.FirstOrDefault(x1=>x1.Begin) ;
                res.FormObjectEnd = objForms.FirstOrDefault(x1 => !x1.Begin);
            }

           // else
                //res.ItsSearch = true;

            
            if (search != null)
            {

                //res.Search = true;
                TempData["list_fe_id"] = res.SearchList;



                //var dict = new RouteValueDictionary();
                //dict.Add("listId", list_id);

                Log log = new Log((String)RouteData.Values["action"], (String)RouteData.Values["controller"],
                ApplicationUser.GetUserId(), true);
                log.SetDescrParam(stateBegin, stateEnd,forms, objForms);
                log.AddLogDb();


                return RedirectToAction("ListFeText", "Physic");
            }
            
            return View(res);
        }

        public ActionResult TextSearchPartial(string type, string str, int lastId = 0, int countLoad = 1)
        {
            //str = "газ";
            TextSearchPartialV res = new TextSearchPartialV();


            //if (str.Split().Length > 10)
            //    str = Search.StringSemanticParse(listmaramslog[1]);


            res.ListPhysId = Search.GetListPhys(type, str, HttpContext, lastId, countLoad);
            if (res.ListPhysId == null)
            {
                Response.StatusCode = 207;
                return Content("", "text/html");//Emty
            }
            if (res.ListPhysId.Count == 0)
            {
                Response.StatusCode = 204;
                return Content("", "text/html");//Emty
            }
            res.CountLoad = countLoad;

            var dictParams = new Dictionary<string, string>();
            dictParams.Add("type", type);
            dictParams.Add("str", str);
            dictParams.Add("lastId", lastId.ToString());
            dictParams.Add("countLoad", countLoad.ToString());
            Log log = new Log((String)RouteData.Values["action"], (String)RouteData.Values["controller"],
               ApplicationUser.GetUserId(), true, dictParams);
            log.AddLogDb();
            return PartialView(res);
        }



        //[HttpPost]
        //type - тип запроса lucene и др

        //lucCount- номер запроса 
        /// <summary>
        /// страница для первого текстового запроса
        /// </summary>
        /// <param name="type"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public ActionResult TextSearch()//string type, string str,bool semanticParse=false
        {
            //TODO валидация строки от sql иньекций и тд
            //TODO полнотекстовый поиск

            TextSearchV res = new TextSearchV();//TODO возможно класс не используется

            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(ApplicationUser.GetUserId());
            if (roles.Contains(RolesProject.admin.ToString()))
            {
                res.Admin = true;
            }


         
            return View(res);//.Select(x1=>x1.IDFE)
        }
        //create fulltext catalog DbaLogParamsCatalog;



    }
}