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

            //TEST
            //{
            //    inp.actionTypeI = "INT_ACTIONS";
            //    inp.FizVelIdI = "VOZ1_FIZVEL_1";
            //    inp.listSelectedProsI = "VOZ1_PROS1 ";
            //    inp.listSelectedSpecI = "";
            //    inp.listSelectedVremI = "";


            //    outp.actionTypeO = "INT_ACTIONS";
            //    outp.FizVelIdO = "VOZ2_FIZVEL_1";
            //    outp.listSelectedProsO = "";
            //    outp.listSelectedSpecO = "";
            //    outp.listSelectedVremO = "";
            //}



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

                return RedirectToAction("ListFeText", "Physic");
                //return RedirectToRoute(new { controller = "Physic", action = "ListFeText", listId = list_id });//new { listId = list_id }
                //return RedirectToAction("ListFeText", "Physic", new { listId = list_id });
                
            }

            return View(list_id);
        }

        public ActionResult TextSearchPartial(string type, string str, int lastId = 0, int lucCount = 1)
        {
            var res = Search.GetList(type, str, lastId, lucCount);


            return PartialView(res);
        }
        //[HttpPost]
        //type - тип запроса lucene и др
        //lucCount- номер запроса 
        public ActionResult TextSearch(string type,string str,int lastId=0,int lucCount=1)
        {
            //TODO валидация строки от sql иньекций и тд
            //TODO полнотекстовый поиск

            //устанавливаем параметры для представления mainHeader
           
                TempData["textSearchStr"] = str;
                TempData["textSearchType"] = type;

            var res = Search.GetList(type, str, lastId, lucCount);
            


            //


            //var obj = res.Take(20);
            //if (obj != null)
            //{
            //    var st = new RussianLightStemmer();
            //    //foreach (var i in obj.Text.Split(' '))
            //    //{
            //    //    Lucene_.ChangeForMap(i);
            //    //    //var num = st.Stem(i.ToArray(), i.Length);
            //    //    // var word = i.Substring(0, num);
            //    //}
            //    foreach (var i in obj)
            //    {
            //        var bef = i.Text;
            //        var aft=Lucene_.ChangeForMap(bef);
            //        //var num = st.Stem(i.ToArray(), i.Length);
            //        // var word = i.Substring(0, num);
            //    }
            //}





            //

            //ViewBag.mark = res.Select(x1=>x1.);

            //TODO сейчас костыль просто потестить
            return View(res);//.Select(x1=>x1.IDFE)
        }




    }
}