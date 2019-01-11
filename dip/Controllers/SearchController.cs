using dip.Models;
using dip.Models.Domain;
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


            int[] list_id = null;
            var inp = new DescrSearchI(inp_);
            var outp = new DescrSearchI(outp_);
            if (!DescrSearchI.IsNull(inp) && !DescrSearchI.IsNull(outp))
            {
                list_id = FEText.GetByDescr(inp, outp);
                ViewBag.inputForm = inp;
                ViewBag.outpForm = outp;
            }

            else
                ViewBag.itsSearch = true;


            if (search != null)
            {

                ViewBag.search = true;
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

            return View(list_id);
        }

        public ActionResult TextSearchPartial(string type, string str, int lastId = 0, int countLoad = 1)
        {
            //str = "газ";

            var res = Search.GetListPhys(type, str, lastId, countLoad);
            if (res.Count == 0)
            {
                Response.StatusCode = 204;
                return Content("", "text/html");//Emty
            }
            ViewBag.countLoad = countLoad;

            Log log = new Log((String)RouteData.Values["action"], (String)RouteData.Values["controller"],
               ApplicationUser.GetUserId(), true, null, type, str, lastId.ToString(), countLoad.ToString());
            log.AddLogDb();
            return PartialView(res.ToArray());
        }



        //[HttpPost]
        //type - тип запроса lucene и др
        //lucCount- номер запроса 
        public ActionResult TextSearch(string type, string str)//,bool semanticParse=false
        {
            //TODO валидация строки от sql иньекций и тд
            //TODO полнотекстовый поиск

            //устанавливаем параметры для представления mainHeader

            TempData["textSearchStr"] = str;
            TempData["textSearchType"] = type;



            Log log = new Log((String)RouteData.Values["action"], (String)RouteData.Values["controller"],
               ApplicationUser.GetUserId(), true, null, type, str);
            var listmaramslog = log.AddLogDb();

            if (str.Length > 10)
                str = Search.StringSemanticParse(listmaramslog[1]);



            var res = new List<int>();
            if (!string.IsNullOrWhiteSpace(str))

                res = Search.GetListPhys(type, str, 0, 1);

            if (res.Count < Constants.CountForLoad)
                ViewBag.ShowBtLoad = false;
            //TODO если произошла ошибка, надо найти лог и поменять флаг
            return View(res.ToArray());//.Select(x1=>x1.IDFE)
        }
        //create fulltext catalog DbaLogParamsCatalog;



    }
}