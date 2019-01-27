using dip.Models;
using dip.Models.Domain;
using dip.Models.ViewModel.Search;
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
        public ActionResult DescriptionSearch(string search = null, DescrSearchIInput inp_ = null, DescrSearchIOut outp_ = null)
        {
            DescriptionSearchV res = new DescriptionSearchV();

            int[] list_id = null;
            var inp = new DescrSearchI(inp_);
            var outp = new DescrSearchI(outp_);
            if (!DescrSearchI.IsNull(inp) && !DescrSearchI.IsNull(outp))
            {
                list_id = FEText.GetByDescr(inp, outp);
                //TODO временный костыль пока не разберусь с типами DescrSearchIInput и DescrSearchI
                {
                    inp_.listSelectedProsI = inp.listSelectedPros;
                    inp_.listSelectedSpecI = inp.listSelectedSpec;
                    inp_.listSelectedVremI = inp.listSelectedVrem;

                    outp_.listSelectedProsO = outp.listSelectedPros;
                    outp_.listSelectedSpecO = outp.listSelectedSpec;
                    outp_.listSelectedVremO = outp.listSelectedVrem;
                    res.FormInput = inp_;
                    res.FormOutput = outp_;
                }
                
            }

            else
                res.ItsSearch = true;


            if (search != null)
            {

                res.Search = true;
                TempData["list_fe_id"] = list_id;



                //var dict = new RouteValueDictionary();
                //dict.Add("listId", list_id);

                Log log = new Log((String)RouteData.Values["action"], (String)RouteData.Values["controller"],
                ApplicationUser.GetUserId(), true);
                log.SetDescrParam(inp, outp);
                log.AddLogDb();
                return RedirectToAction("ListFeText", "Physic");
                //return RedirectToRoute(new { controller = "Physic", action = "ListFeText", listId = list_id });//new { listId = list_id }
                //return RedirectToAction("ListFeText", "Physic", new { listId = list_id });

            }

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
            
            if (str.Length > 10)
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