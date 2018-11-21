using dip.Models;
using dip.Models.Domain;
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



            int[] list_id = null;

            if (DescrSearchIInput.Validation(inp) && DescrSearchIOut.Validation(outp))
            {
                //поиск
                //List<int> list_id = new List<int>();

                using (var db = new ApplicationDbContext())
                {
                    //находим все записи которые подходят по входным параметрам
                    var inp_query = db.FEActions.Where(x1 => x1.Input == 1 &&
                      x1.Type == inp.actionTypeI &&
                      x1.FizVelId == inp.FizVelIdI &&
                      x1.Pros == inp.listSelectedProsI &&
                      x1.Spec == inp.listSelectedSpecI &&
                      x1.Vrem == inp.listSelectedVremI);

                    //находим все записи которые подходят по выходным параметрам
                    var out_query = db.FEActions.Where(x1 => x1.Input == 0 &&
                    x1.Type == outp.actionTypeO &&
                    x1.FizVelId == outp.FizVelIdO &&
                    x1.Pros == outp.listSelectedProsO &&
                    x1.Spec == outp.listSelectedSpecO &&
                    x1.Vrem == outp.listSelectedVremO);

                    //записи которые подходят по всем параметрам
                    list_id = inp_query.Join(out_query, x1 => x1.Idfe, x2 => x2.Idfe, (x1, x2) => x1.Idfe).ToArray();
                    //ViewBag.listFeId = list_id;

                }
            }


            if (search != null)
            {

                ViewBag.search = true;

                return RedirectToAction("ListFeText", "Actions", list_id);
            }

            return View(list_id);
        }


        [HttpPost]
        public ActionResult TextSearch(string str)
        {
            //TODO полнотекстовый поиск

            using (var db = new ApplicationDbContext())
            {
                System.Data.SqlClient.SqlParameter param1 = new System.Data.SqlClient.SqlParameter("@searched_str", "Затухание");
                System.Data.SqlClient.SqlParameter param2 = new System.Data.SqlClient.SqlParameter("@max_lev", 5);
                var lst = db.Database.SqlQuery<test>("SELECT * FROM GetListLev (@searched_str,@max_lev)", param1, param2).ToList();


            }




            return View();
        }




    }
}