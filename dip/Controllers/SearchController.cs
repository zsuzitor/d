using dip.Models;
using dip.Models.Domain;
using Lucene.Net.Analysis.Ru;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult DescriptionSearch(string search = null, DescrSearchIInput inp = null, DescrSearchIOut outp = null)
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



            int[] list_id = FEText.GetByDescr(inp, outp) ;
            




            if (search != null)
            {

                ViewBag.search = true;

                return RedirectToAction("ListFeText", "Actions", list_id);
            }

            return View(list_id);
        }


        //[HttpPost]
        //type - тип запроса lucene и др
        public ActionResult TextSearch(string type,string str)
        {
            //TODO полнотекстовый поиск
            var res = new int[0];
            //type = "fullTextSearch";
            switch (type)
            {
                case "lucene":
                    //var list_id=FEText.GetByText(str);
                    int count;
                    str = Lucene_.ChangeForMap(str);
                    //TODO убрать знаки препинания, стопслова
                    res = Lucene_.Search(str, 100, out count).ToArray();
                    break;
                case "fullTextSearch":
                    using (var db=new ApplicationDbContext())
                    {
                        //TODO вынести в функцию sql server и юзать уже из linq
                       var  res1 = db.Database.SqlQuery<int>($@"select IDFE from freetexttable(dbo.FeTexts,*,{str})as t join dbo.FeTexts as y on t.[KEY] = y.IDFE order by RANK desc;");
                    }



                        break;


            }


            





            //устанавливаем параметры для представления mainHeader
            TempData["textSearchStr"] = str;
            TempData["textSearchType"] = type;
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