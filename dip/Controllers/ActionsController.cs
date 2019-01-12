using dip.Models;
using dip.Models.Domain;
using dip.Models.ViewModel;
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

       


        


        //[HttpPost]
        //public ActionResult DescriptionSearch(DescrSearchIInput inp, DescrSearchIOut outp)
        //{
        ////    (string actionIdI, string actionTypeI, string FizVelIdI,
        ////string parametricFizVelIdI, string listSelectedProsI, string listSelectedSpecI, string listSelectedVremI,
        ////string actionTypeO, string FizVelIdO, string parametricFizVelIdO, string listSelectedProsO, string listSelectedSpecO, string listSelectedVremO)

            //    return View();
            //}

        public ActionResult DescriptionInput(DescrSearchIInput inp=null, DescrSearchIOut outp = null)
        {

            ViewBag.inputForm = DescriptionForm.GetFormObject(inp?.actionIdI,inp?.FizVelIdI);
            ViewBag.outpForm = DescriptionForm.GetFormObject(outp?.actionIdO, outp?.FizVelIdO);

            var inp_ = new DescrSearchI(inp);
            var outp_ = new DescrSearchI(outp);
            if (DescrSearchI.IsNull(inp_) || DescrSearchI.IsNull(outp_))
            {
                inp_ = null;
                outp_ = null;
            }
            ViewBag.inputFormData = inp_;
            ViewBag.outpFormData = outp_;



            //ViewBag.postfix = postfix;
            return PartialView();
        }
        /// <summary>
        /// загрузить для отображения(не формой а текстом)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LoadDescr(int id)
        {
            //TODO сейчас полностью форма, а нужны только выбранные параметры
            FEAction inp = null;
            FEAction outp = null;
            FEAction.Get(id, ref inp, ref outp);
            ViewBag.inputForm = new DescrSearchIInput(inp);
            ViewBag.outpForm = new DescrSearchIOut(outp);
            return PartialView();
        }

        //------------------------------------------------------


        //////------------------------++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        public ActionResult ChangeAction(string fizVelId, string type)
        {
            
            if(fizVelId== "VOZ11")
            {
                ViewBag.CheckboxParamsId = null;
               
                ViewBag.ParametricFizVelsId = "VOZ11_FIZVEL_R1";
                
            }
            else
            {
                ViewBag.CheckboxParamsId = fizVelId;
           
                ViewBag.ParametricFizVelsId =null;
            }

            ////-----
            
            ViewBag.fizVelId = fizVelId;
           
            

            ViewBag.type = type;

            return PartialView();
        }


        /// <summary>
        /// GET-метод обновления физических величин
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns>
        [ChildActionOnly]
        public ActionResult GetFizVels(string id, string type = "")
        {
            List<FizVel> listOfFizVels;

            using (var db = new ApplicationDbContext())
                if (id != "VOZ11") // непараметрическое воздействие
                                   // Получаем обновленный список физических величин
                    listOfFizVels = db.FizVels.Where(fizVel => (fizVel.Parent == id + "_FIZVEL") ||
                                                                              (fizVel.Id == "NO_FIZVEL"))
                                                           .OrderBy(fizVel => fizVel.Id).ToList();
                else
                    // Получаем обновленный список физических величин
                    listOfFizVels = db.FizVels.Where(fizVel => (fizVel.Parent == id + "_FIZVEL"))
                                                           .OrderBy(fizVel => fizVel.Id).ToList();

            // Отправляем его в представление
            ViewBag.fizVelId = listOfFizVels;
            ViewBag.currentActionId = id;
            ViewBag.type = type;
            return PartialView();
        }




        /// <summary>
        /// GET-метод обновления пространственных характеристик
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns>
        [ChildActionOnly]
        public ActionResult GetPros(string id, string type)
        {
            // Получаем обновленный список пространственных характеристик

            List<Pro> prosList = new List<Pro>();
            if (!string.IsNullOrWhiteSpace(id))
                using (var db = new ApplicationDbContext())
                    prosList = db.Pros.Where(pros => pros.Parent == id + "_PROS").ToList();
            // var listSelectedPros = GetListSelectedItem(prosList);

            // Отправляем его в представление
            ViewBag.pros = prosList;
            ViewBag.type = type;
            return PartialView();
        }




        /// <summary>
        /// GET-метод обновления специальных характеристик
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        [ChildActionOnly]
        public ActionResult GetSpec(string id, string type)
        {
            // Получаем обновленный список специальных характеристик
            List<Spec> specList = new List<Spec>();
            if (!string.IsNullOrWhiteSpace(id))
                using (var db = new ApplicationDbContext())
                    specList = db.Specs.Where(spec => spec.Parent == id + "_SPEC").ToList();
            //var listSelectedSpec = GetListSelectedItem(specList);

            // Отправляем его в представление
            ViewBag.spec = specList;
            ViewBag.type = type;
            return PartialView();
        }



        /// <summary>
        /// GET-метод обновления временных характеристик
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        [ChildActionOnly]
        public ActionResult GetVrem(string id, string type)
        {
            // Получаем обновленный список временных характеристик
            List<Vrem> vremList = new List<Vrem>();
            if (!string.IsNullOrWhiteSpace(id))
                using (var db = new ApplicationDbContext())
                    vremList = db.Vrems.Where(vrem => vrem.Parent == id + "_VREM").ToList();


            // Отправляем его в представление
            ViewBag.vrem = vremList;
            ViewBag.type = type;
            return PartialView();
        }



        //////------------------------++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++



        /// <summary>
        /// GET-метод обновления параметрических физических величин
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns>
        //[ChildActionOnly]
        public ActionResult GetParametricFizVels(string id, string type)
        {
            // Получаем список физических величин для параметрических воздействий
            List<FizVel> res = new List<FizVel>();
            if (!string.IsNullOrWhiteSpace(id))
                 res = FizVel.GetParametricFizVels(id) ;
            ViewBag.parametricFizVelId = res;
            ViewBag.type = type;
            return PartialView();
        }





        /// <summary>
        /// GET-метод добавления на представление дополнительных значений пространственной характеристики
        /// </summary>
        /// <param name="id"> дескриптор выбранной пространственной характеристики + идентификатор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        //[ChildActionOnly]
        public ActionResult GetProsChild(string id, string type)
        {
            // Извлекаем дескриптор характеристики и идентификатор воздействия
            var args = id.Split('@');
            var prosId = args[0];
            var actId = args[1];

            // Преобразуем идентификатор воздействия из строки в число
            var actionId = int.Parse(actId);

            // Получаем список значений, соответствующий данной характеристике
            List<Pro> prosList = new List<Pro>();
            // Создаем новый список характеристик
            var listSelectedPros = new List<SelectedItem>();


            prosList = Pro.GetChild(prosId);
            Models.Domain.Action action = Models.Domain.Action.GetAction(actionId);
            listSelectedPros = SelectedItem.GetListSelectedItem(prosList, action, 2);
            //using (var db = new ApplicationDbContext())
            //{
            //    // Получаем список значений, соответствующий данной характеристике
            //     prosList = db.Pros.Where(pros => pros.Parent == prosId).ToList();




            //    Models.Domain.Action action = null;
            //    if (actionId != -1) // значение идентификатора не равно -1
            //    //{
            //        // Извлекаем воздействие из БД
            //         action = db.Actions.Find(actionId);

            //        // Преобразуем список характеристик к нужному типу
            //        listSelectedPros = SelectedItem.GetListSelectedItem(prosList, action,db,2);
            ////    }
            ////    else
            ////        // Преобразуем список характеристик к нужному типу
            ////        listSelectedPros = GetListSelectedItem(prosList,null,db);
            //}
                


            

            // Отправляем полученный список в представление
            ViewBag.prosChild = listSelectedPros;
            ViewBag.parent = prosId;
            ViewBag.type = type;
            return PartialView();
        }





        /// <summary>
        /// GET-метод удаления из представления дополнительных значений характеристики
        /// </summary>
        /// <param name="id"> дескриптор выбранной характеристики </param>
        /// <returns> результат действия ActionResult </returns>
        //[ChildActionOnly]
        public ActionResult GetEmptyChild(string id, string type)
        {
            // Передаем в представление дескриптор характеристики
            ViewBag.parent = id;
            ViewBag.type = type;
            return PartialView();
        }






        /// <summary>
        /// GET-метод добавления на представление дополнительных значений специальной характеристики
        /// </summary>
        /// <param name="id"> дескриптор выбранной специальной характеристики + идентификатор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        //[ChildActionOnly]
        public ActionResult GetSpecChild(string id, string type)
        {
            // Извлекаем дескриптор характеристики и идентификатор воздействия
            var args = id.Split('@');
            var specId = args[0];
            var actId = args[1];

            // Создаем новый список характеристик
            var listSelectedSpec = new List<SelectedItem>();

            // Преобразуем идентификатор воздействия из строки в число
            var actionId = int.Parse(actId);
            //using (var db = new ApplicationDbContext())
            //{

                // Получаем список значений, соответствующий данной характеристике
                var specList = Spec.GetChild(specId) ;




                Models.Domain.Action action = Models.Domain.Action.GetAction(actionId);
                //if (actionId != -1) // значение идентификатора не равно -1
                
                    // Извлекаем воздействие из БД
                     //action = db.Actions.Find(actionId);

                    // Преобразуем список характеристик к нужному типу
                    listSelectedSpec = SelectedItem.GetListSelectedItem(specList, action,1);
                //}
                //else
                //    // Преобразуем список характеристик к нужному типу
                //    listSelectedSpec = GetListSelectedItem(specList,null,db);
            //}
            // Отправляем полученный список в представление
            ViewBag.specChild = listSelectedSpec;
            ViewBag.parent = specId;
            ViewBag.type = type;
            return PartialView();
        }










        /// <summary>
        /// GET-метод добавления на представление дополнительных значений временной характеристики
        /// </summary>
        /// <param name="id"> дескриптор выбранной временной характеристики + идентификатор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        //[ChildActionOnly]
        public ActionResult GetVremChild(string id, string type)
        {
            // Извлекаем дескриптор характеристики и идентификатор воздействия
            var args = id.Split('@');
            var vremId = args[0];
            var actId = args[1];

            // Создаем новый список характеристик
            var listSelectedVrem = new List<SelectedItem>();

            // Преобразуем идентификатор воздействия из строки в число
            var actionId = int.Parse(actId);

            //using (var db = new ApplicationDbContext())
            //{
                // Получаем список значений, соответствующий данной характеристике
                var vremList = Vrem.GetChild(vremId);//db.Vrems.Where(vrem => vrem.Parent == vremId).ToList();


                Models.Domain.Action action = Models.Domain.Action.GetAction(actionId);
                //if (actionId != -1) // значение идентификатора не равно -1
               
                    // Извлекаем воздействие из БД
                     //action = db.Actions.Find(actionId);

                    // Преобразуем список характеристик к нужному типу
                    listSelectedVrem = SelectedItem.GetListSelectedItem(vremList, action,0);
                //}
                //else
                //    // Преобразуем список характеристик к нужному типу
                //    listSelectedVrem = GetListSelectedItem(vremList,null,db);
            //}
            // Отправляем полученный список в представление
            ViewBag.vremChild = listSelectedVrem;
            ViewBag.parent = vremId;
            ViewBag.type = type;
            return PartialView();
        }










        //----------------------------------------------------------------PRIVATE





     

       

    }
}