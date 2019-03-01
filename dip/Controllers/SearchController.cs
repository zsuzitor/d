using dip.Models;
using dip.Models.Domain;
using dip.Models.ViewModel.SearchV;
using Lucene.Net.Analysis.Ru;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace dip.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }


        
        //TODO search- переименовать+ в js тоже поменять на partial
        public ActionResult DescriptionSearch(string search = null, DescrSearchI[] forms= null, DescrObjectI[] objForms=null)//, DescrSearchIInput inp_ = null, DescrSearchIOut outp_ = null
        {

            //TODO сейчас просто проставил null временно
            //DescrSearchIInput.ValidationIfNeed(inp_);

            //DescrSearchIOut.ValidationIfNeed(outp_);
            //if (inp_?.Valide == false || outp_?.Valide == false)
            //    return new HttpStatusCodeResult(404);

            DescriptionSearchV res = new DescriptionSearchV();


            //var inp = new DescrSearchI(inp:null);//inp_
            //var outp = new DescrSearchI(outp:null);//outp_
            //if (!DescrSearchI.IsNull(inp) && !DescrSearchI.IsNull(outp))
            foreach (var i in forms)
            {
                DescrSearchI.Validation(i);
                if(i?.Valide==false)
                    return new HttpStatusCodeResult(404);
            }
            if (forms!=null)
            {

                res.SearchList = FEText.GetByDescr(forms, objForms);

                res.FormInput = forms.Where(x1=>x1.InputForm).ToList();
                res.FormOutput = forms.Where(x1 => !x1.InputForm).ToList(); ;


            }

            else
                //res.ItsSearch = true;


            if (search != null)
            {

                //res.Search = true;
                TempData["list_fe_id"] = res.SearchList;



                //var dict = new RouteValueDictionary();
                //dict.Add("listId", list_id);

                Log log = new Log((String)RouteData.Values["action"], (String)RouteData.Values["controller"],
                ApplicationUser.GetUserId(), true);
                log.SetDescrParam(forms);
                log.AddLogDb();
                return RedirectToAction("ListFeText", "Physic");
                //return RedirectToRoute(new { controller = "Physic", action = "ListFeText", listId = list_id });//new { listId = list_id }
                //return RedirectToAction("ListFeText", "Physic", new { listId = list_id });

            }
            //DescrSearchIInput.Validation(res.FormInput);
            return View(res);
        }

        public ActionResult TextSearchPartial(string type, string str, int lastId = 0, int countLoad = 1)
        {
            //str = "газ";
            TextSearchPartialV res = new TextSearchPartialV();
            res.ListPhysId = Search.GetListPhys(type, str, lastId, countLoad);
            if (res.ListPhysId.Count == 0)
            {
                Response.StatusCode = 204;
                return Content("", "text/html");//Emty
            }
            res.CountLoad = countLoad;

            Log log = new Log((String)RouteData.Values["action"], (String)RouteData.Values["controller"],
               ApplicationUser.GetUserId(), true, null, type, str, lastId.ToString(), countLoad.ToString());
            log.AddLogDb();
            return PartialView(res);
        }



        //[HttpPost]
        //type - тип запроса lucene и др
        //lucCount- номер запроса 
        public ActionResult TextSearch(string type, string str)//,bool semanticParse=false
        {
            //TODO валидация строки от sql иньекций и тд
            //TODO полнотекстовый поиск

            TextSearchV res = new TextSearchV();

            //устанавливаем параметры для представления mainHeader

            TempData["textSearchStr"] = str;
            TempData["textSearchType"] = type;



            Log log = new Log((String)RouteData.Values["action"], (String)RouteData.Values["controller"],
               ApplicationUser.GetUserId(), true, null, type, str);
            var listmaramslog = log.AddLogDb();
            
            if (str.Split().Length > 10)
                str = Search.StringSemanticParse(listmaramslog[1]);



            
            if (!string.IsNullOrWhiteSpace(str))

                res.ListPhysId = Search.GetListPhys(type, str, 0, 1);

            if (res.ListPhysId.Count < Constants.CountForLoad)
                res.ShowBtLoad = false;
            //TODO если произошла ошибка, надо найти лог и поменять флаг
            return View(res);//.Select(x1=>x1.IDFE)
        }
        //create fulltext catalog DbaLogParamsCatalog;



    }
}