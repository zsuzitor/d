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


        public ActionResult DescriptionSearch(string search=null,DescrSearchIInput inp=null, DescrSearchIOut outp = null)
        {
            if (search != null)
            {
                //поиск
                //List<int> list_id = new List<int>();
                int[] list_id;
                using (var db=new ApplicationDbContext())
                {
                    //находим все записи которые подходят по входным параметрам
                    var inp_query=db.FEActions.Where(x1 =>x1.Input==1&& 
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
                     list_id=inp_query.Join(out_query, x1 => x1.Idfe, x2 => x2.Idfe, (x1, x2) => x1.Idfe).ToArray();
                    //ViewBag.listFeId = list_id;
                    
                }
                ViewBag.search = true;

                return RedirectToAction("ListFeText", "Actions", list_id);
            }

            return View();
        }


        public ActionResult ListFeText(int[] listId)
        {

            List<FEText> res = new List<FEText>();
            if(listId!=null)
            using (var db = new ApplicationDbContext())

                res = db.FEText.Join(listId, x1 => x1.IDFE, x2 => x2, (x1, x2) => x1).ToList();



            return PartialView(res);
        }


        //[HttpPost]
        //public ActionResult DescriptionSearch(DescrSearchIInput inp, DescrSearchIOut outp)
        //{
        ////    (string actionIdI, string actionTypeI, string FizVelIdI,
        ////string parametricFizVelIdI, string listSelectedProsI, string listSelectedSpecI, string listSelectedVremI,
        ////string actionTypeO, string FizVelIdO, string parametricFizVelIdO, string listSelectedProsO, string listSelectedSpecO, string listSelectedVremO)

            //    return View();
            //}

        public ActionResult DescriptionSearchInput()
        {

            DescriptionForm res = new DescriptionForm();


            using (var db = new ApplicationDbContext())
            {
                // Получаем список всех воздействий и выбираем по-умолчанию первое в списке
                var listOfActions = db.AllActions.OrderBy(action => action.Id).ToList();
                var actionId = listOfActions.First().Id;

                // Получаем список типов воздействий     
                var actionType = db.ActionTypes.OrderByDescending(type => type.Name).ToList();//, "id", "name", "Не выбрано")

                // Получаем список физических величин для выбранного воздействия
                var listOfFizVels = db.FizVels.Where(fizVel => (fizVel.Parent == actionId + "_FIZVEL") ||
                                                                              (fizVel.Id == "NO_FIZVEL"))
                                                           .OrderBy(fizVel => fizVel.Id).ToList();

                // Выбираем первый из списка раздел физики
                var fizVelId = listOfFizVels.First().Id;

                // Получаем список физических величин для параметрических воздействий
                var listOfParametricFizVels = db.FizVels.Where(parametricFizVel => (parametricFizVel.Parent == fizVelId))
                                                                     .OrderBy(parametricFizVel => parametricFizVel.Id).ToList();

                // Получаем список пространственных характеристик для выбранного воздействия
                var prosList = db.Pros.Where(pros => pros.Parent == actionId + "_PROS").ToList();
                //var listSelectedPros = GetListSelectedItem(prosList);

                // Получаем список специальных характеристик для выбранного воздействия
                var specList = db.Specs.Where(spec => spec.Parent == actionId + "_SPEC").ToList();
               // var listSelectedSpec = GetListSelectedItem(specList);

                // Получаем список временных характеристик для выбранного воздействия
                var vremList = db.Vrems.Where(vrem => vrem.Parent == actionId + "_VREM").ToList();
                //var listSelectedVrem = GetListSelectedItem(vremList);

                // Готовим данные для отправки в представление
                res.actionId = listOfActions;
                res.actionType = actionType;
                res.fizVelId = listOfFizVels;
                res.parametricFizVelId = listOfParametricFizVels;
                res.pros = prosList;// listSelectedPros;
                res.spec = specList;//listSelectedSpec;
                res.vrem = vremList;// listSelectedVrem;
                res.currentAction = actionId;
                res.currentActionId = "-1";
            }



           
            //ViewBag.postfix = postfix;
            return PartialView(res);
        }



        //------------------------------------------------------


        //////------------------------++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        public ActionResult ChangeAction(string fizVelId, string prosId, string specId, string vremId, string type)
        {
            //List<FizVel> listOfFizVels;

            //using (var db = new ApplicationDbContext())
            //    if (fizVelId != "VOZ11") // непараметрическое воздействие
            //                       // Получаем обновленный список физических величин
            //        listOfFizVels = db.FizVels.Where(fizVel => (fizVel.Parent == fizVelId + "_FIZVEL") ||
            //                                                                  (fizVel.Id == "NO_FIZVEL"))
            //                                               .OrderBy(fizVel => fizVel.Id).ToList();
            //    else
            //        // Получаем обновленный список физических величин
            //        listOfFizVels = db.FizVels.Where(fizVel => (fizVel.Parent == fizVelId + "_FIZVEL"))
            //                                               .OrderBy(fizVel => fizVel.Id).ToList();

            //// Отправляем его в представление
            //ViewBag.fizVelId = listOfFizVels;
            //ViewBag.currentActionId = fizVelId;


            ////-----

            //List<Pro> prosList = new List<Pro>();
            //using (var db = new ApplicationDbContext())
            //    prosList = db.Pros.Where(pros => pros.Parent == prosId + "_PROS").ToList();
            //// var listSelectedPros = GetListSelectedItem(prosList);

            //// Отправляем его в представление
            //ViewBag.pros = prosList;


            ////-----


            //List<Spec> specList = new List<Spec>();
            //using (var db = new ApplicationDbContext())
            //    specList = db.Specs.Where(spec => spec.Parent == specId + "_SPEC").ToList();
            ////var listSelectedSpec = GetListSelectedItem(specList);

            //// Отправляем его в представление
            //ViewBag.spec = specList;


            ////-----


            //List<Vrem> vremList = new List<Vrem>();
            //using (var db = new ApplicationDbContext())
            //    vremList = db.Vrems.Where(vrem => vrem.Parent == vremId + "_VREM").ToList();


            //// Отправляем его в представление
            //ViewBag.vrem = vremList;



            ////-----
            
            ViewBag.fizVelId = fizVelId;
            ViewBag.prosId = prosId;
            ViewBag.specId = specId;
            ViewBag.vremId = vremId;
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

            using (var db = new ApplicationDbContext())
            {
                var listOfParametricFizVels = db.FizVels.Where(parametricFizVel => (parametricFizVel.Parent == id)).ToList();

                //TODO ошибка? условие должно быть елси записей 0?????
                if (listOfParametricFizVels.Count != 0)
                {
                    listOfParametricFizVels.Add(db.FizVels.Where(parametricFizVel => parametricFizVel.Id == "NO_FIZVEL").First());

                    var selectListOfParametricFizVels = listOfParametricFizVels
                                                                     .OrderBy(parametricFizVel => parametricFizVel.Id)
                                                                     .ToList();

                    // Отправляем его в представление
                    ViewBag.parametricFizVelId = selectListOfParametricFizVels;
                }
                else
                    // Отправляем его в представление
                    ViewBag.parametricFizVelId = listOfParametricFizVels;
            }

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
            using (var db = new ApplicationDbContext())
            {
                // Получаем список значений, соответствующий данной характеристике
                 prosList = db.Pros.Where(pros => pros.Parent == prosId).ToList();




                Models.Domain.Action action = null;
                if (actionId != -1) // значение идентификатора не равно -1
                //{
                    // Извлекаем воздействие из БД
                     action = db.Actions.Find(actionId);

                    // Преобразуем список характеристик к нужному типу
                    listSelectedPros = SelectedItem.GetListSelectedItem(prosList, action,db,2);
            //    }
            //    else
            //        // Преобразуем список характеристик к нужному типу
            //        listSelectedPros = GetListSelectedItem(prosList,null,db);
            }
                


            

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
            using (var db = new ApplicationDbContext())
            {

                // Получаем список значений, соответствующий данной характеристике
                var specList = db.Specs.Where(spec => spec.Parent == specId).ToList();




                Models.Domain.Action action = null;
                if (actionId != -1) // значение идентификатора не равно -1
                
                    // Извлекаем воздействие из БД
                     action = db.Actions.Find(actionId);

                    // Преобразуем список характеристик к нужному типу
                    listSelectedSpec = SelectedItem.GetListSelectedItem(specList, action,db,1);
                //}
                //else
                //    // Преобразуем список характеристик к нужному типу
                //    listSelectedSpec = GetListSelectedItem(specList,null,db);
            }
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

            using (var db = new ApplicationDbContext())
            {
                // Получаем список значений, соответствующий данной характеристике
                var vremList = db.Vrems.Where(vrem => vrem.Parent == vremId).ToList();


                Models.Domain.Action action = null;
                if (actionId != -1) // значение идентификатора не равно -1
               
                    // Извлекаем воздействие из БД
                     action = db.Actions.Find(actionId);

                    // Преобразуем список характеристик к нужному типу
                    listSelectedVrem = SelectedItem.GetListSelectedItem(vremList, action,db,0);
                //}
                //else
                //    // Преобразуем список характеристик к нужному типу
                //    listSelectedVrem = GetListSelectedItem(vremList,null,db);
            }
            // Отправляем полученный список в представление
            ViewBag.vremChild = listSelectedVrem;
            ViewBag.parent = vremId;
            ViewBag.type = type;
            return PartialView();
        }










        //----------------------------------------------------------------PRIVATE





     

       

    }
}