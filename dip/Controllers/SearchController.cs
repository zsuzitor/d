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



            int[] list_id = FEText.GetByDescr(inp, outp) ;
            




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

            var list_id=FEText.GetByText(str);




            return View();
        }




    }
}