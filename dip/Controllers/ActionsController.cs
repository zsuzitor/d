using dip.Models;
using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Controllers
{
    public class ActionsController : Controller
    {
        // GET: Actions
        public ActionResult Index()
        {
            return View();
        }






        //------------------------------------------------------




        /// <summary>
        /// GET-метод обновления физических величин
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns>
        public ActionResult GetFizVels(string id)
        {
            SelectList listOfFizVels;

            using (var db=new ApplicationDbContext())
                if (id != "VOZ11") // непараметрическое воздействие
                                   // Получаем обновленный список физических величин
                    listOfFizVels = new SelectList(db.FizVels.Where(fizVel => (fizVel.Parent == id + "_FIZVEL") ||
                                                                              (fizVel.Id == "NO_FIZVEL"))
                                                           .OrderBy(fizVel => fizVel.Id), "id", "name");
                else
                    // Получаем обновленный список физических величин
                    listOfFizVels = new SelectList(db.FizVels.Where(fizVel => (fizVel.Parent == id + "_FIZVEL"))
                                                           .OrderBy(fizVel => fizVel.Id), "id", "name");

            // Отправляем его в представление
            ViewBag.fizVelId = listOfFizVels;
            ViewBag.currentActionId = id;

            return PartialView();
        }




        /// <summary>
        /// GET-метод обновления пространственных характеристик
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns>
        public ActionResult GetPros(string id)
        {
            // Получаем обновленный список пространственных характеристик

            List<Pro> prosList = new List<Pro>();
            using (var db = new ApplicationDbContext())
                 prosList = db.Pros.Where(pros => pros.Parent == id + "_PROS").ToList();
           // var listSelectedPros = GetListSelectedItem(prosList);

            // Отправляем его в представление
            ViewBag.pros = prosList;

            return PartialView();
        }





    }
}