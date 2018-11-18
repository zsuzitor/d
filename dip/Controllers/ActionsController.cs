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
                    var list_id=inp_query.Join(out_query, x1 => x1.Idfe, x2 => x2.Idfe, (x1, x2) => x1.Idfe).ToList();
                    ViewBag.listFeId = list_id;
                    ViewBag.search = true;
                }



            }

            return View();
        }


        public ActionResult ListFeText(int[] listId)
        {

            List<FEText> res = new List<FEText>();
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

        public ActionResult DescriptionSearchInput(string postfix="")
        {

            DescriptionForm res = new DescriptionForm();


            using (var db = new ApplicationDbContext())
            {
                // Получаем список всех воздействий и выбираем по-умолчанию первое в списке
                var listOfActions = new SelectList(db.AllActions.OrderBy(action => action.Id).ToList(), "id", "name");
                var actionId = listOfActions.First().Value;

                // Получаем список типов воздействий     
                var actionType = new SelectList(db.ActionTypes.OrderByDescending(type => type.Name).ToList(), "id", "name", "Не выбрано");

                // Получаем список физических величин для выбранного воздействия
                var listOfFizVels = new SelectList(db.FizVels.Where(fizVel => (fizVel.Parent == actionId + "_FIZVEL") ||
                                                                              (fizVel.Id == "NO_FIZVEL"))
                                                           .OrderBy(fizVel => fizVel.Id).ToList(), "id", "name");

                // Выбираем первый из списка раздел физики
                var fizVelId = listOfFizVels.First().Value;

                // Получаем список физических величин для параметрических воздействий
                var listOfParametricFizVels = new SelectList(db.FizVels.Where(parametricFizVel => (parametricFizVel.Parent == fizVelId))
                                                                     .OrderBy(parametricFizVel => parametricFizVel.Id), "id", "name")
                                                                     .ToList();

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



            res.Postfix= postfix;
            //ViewBag.postfix = postfix;
            return PartialView();
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
                                                           .OrderBy(fizVel => fizVel.Id).ToList(), "id", "name");
                else
                    // Получаем обновленный список физических величин
                    listOfFizVels = new SelectList(db.FizVels.Where(fizVel => (fizVel.Parent == id + "_FIZVEL"))
                                                           .OrderBy(fizVel => fizVel.Id).ToList(), "id", "name");

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




        /// <summary>
        /// GET-метод обновления специальных характеристик
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        public ActionResult GetSpec(string id)
        {
            // Получаем обновленный список специальных характеристик
            List<Spec> specList = new List<Spec>();
            using (var db = new ApplicationDbContext())
                 specList = db.Specs.Where(spec => spec.Parent == id + "_SPEC").ToList();
            //var listSelectedSpec = GetListSelectedItem(specList);

            // Отправляем его в представление
            ViewBag.spec = specList;

            return PartialView();
        }



        /// <summary>
        /// GET-метод обновления временных характеристик
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        public ActionResult GetVrem(string id)
        {
            // Получаем обновленный список временных характеристик
            List<Vrem> vremList = new List<Vrem>();
            using (var db = new ApplicationDbContext())
                 vremList = db.Vrems.Where(vrem => vrem.Parent == id + "_VREM").ToList();
            

            // Отправляем его в представление
            ViewBag.vrem = vremList;

            return PartialView();
        }





        /// <summary>
        /// GET-метод обновления параметрических физических величин
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns>
        public ActionResult GetParametricFizVels(string id)
        {
            // Получаем список физических величин для параметрических воздействий

            using (var db = new ApplicationDbContext())
            {
                var listOfParametricFizVels = db.FizVels.Where(parametricFizVel => (parametricFizVel.Parent == id)).ToList();

                //TODO ошибка? условие должно быть елси записей 0?????
                if (listOfParametricFizVels.Count != 0)
                {
                    listOfParametricFizVels.Add(db.FizVels.Where(parametricFizVel => parametricFizVel.Id == "NO_FIZVEL").First());

                    var selectListOfParametricFizVels = new SelectList(listOfParametricFizVels
                                                                     .OrderBy(parametricFizVel => parametricFizVel.Id), "id", "name")
                                                                     .ToList();

                    // Отправляем его в представление
                    ViewBag.parametricFizVelId = selectListOfParametricFizVels;
                }
                else
                    // Отправляем его в представление
                    ViewBag.parametricFizVelId = listOfParametricFizVels;
            }
                

            return PartialView();
        }





        /// <summary>
        /// GET-метод добавления на представление дополнительных значений пространственной характеристики
        /// </summary>
        /// <param name="id"> дескриптор выбранной пространственной характеристики + идентификатор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        public ActionResult GetProsChild(string id)
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
                    listSelectedPros = GetListSelectedItem(prosList, action,db,2);
            //    }
            //    else
            //        // Преобразуем список характеристик к нужному типу
            //        listSelectedPros = GetListSelectedItem(prosList,null,db);
            }
                


            

            // Отправляем полученный список в представление
            ViewBag.prosChild = listSelectedPros;
            ViewBag.parent = prosId;

            return PartialView();
        }





        /// <summary>
        /// GET-метод удаления из представления дополнительных значений характеристики
        /// </summary>
        /// <param name="id"> дескриптор выбранной характеристики </param>
        /// <returns> результат действия ActionResult </returns>
        public ActionResult GetEmptyChild(string id)
        {
            // Передаем в представление дескриптор характеристики
            ViewBag.parent = id;

            return PartialView();
        }






        /// <summary>
        /// GET-метод добавления на представление дополнительных значений специальной характеристики
        /// </summary>
        /// <param name="id"> дескриптор выбранной специальной характеристики + идентификатор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        public ActionResult GetSpecChild(string id)
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
                    listSelectedSpec = GetListSelectedItem(specList, action,db,1);
                //}
                //else
                //    // Преобразуем список характеристик к нужному типу
                //    listSelectedSpec = GetListSelectedItem(specList,null,db);
            }
            // Отправляем полученный список в представление
            ViewBag.specChild = listSelectedSpec;
            ViewBag.parent = specId;

            return PartialView();
        }










        /// <summary>
        /// GET-метод добавления на представление дополнительных значений временной характеристики
        /// </summary>
        /// <param name="id"> дескриптор выбранной временной характеристики + идентификатор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        public ActionResult GetVremChild(string id)
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
                    listSelectedVrem = GetListSelectedItem(vremList, action,db,0);
                //}
                //else
                //    // Преобразуем список характеристик к нужному типу
                //    listSelectedVrem = GetListSelectedItem(vremList,null,db);
            }
            // Отправляем полученный список в представление
            ViewBag.vremChild = listSelectedVrem;
            ViewBag.parent = vremId;

            return PartialView();
        }










        //----------------------------------------------------------------PRIVATE





     

        //type : 0-Vrems   1-spec
        private List<SelectedItem> GetListSelectedItem<T>(List<T> list, Models.Domain.Action action, ApplicationDbContext db,int type) where T : Item
        {
            // Сортируем список характеристик
            list = list.OrderBy(pros => pros.Parent).ToList();

            // Создаем список List<SelectedItem>
            var listSelectedPros = new List<SelectedItem>();

            // Приводим List<T> к List<SelectedItem>
            foreach (var item in list)
            {
                // Проверяем, отмечена ли характеристика в воздействии
                bool isContains = false;
                if (action != null)
                {
                    if(db==null)
                    {
                        db = new ApplicationDbContext();
                        db.Set<Models.Domain.Action>().Attach(action);
                    }
                    switch (type)
                    {
                        case 0:
                            if (!db.Entry(action).Collection(x1 => x1.Vrems).IsLoaded)
                                db.Entry(action).Collection(x1 => x1.Vrems).Load();
                            isContains = (action.Vrems.FirstOrDefault(x1=>x1.Id== item.Id)==null?false:true);
                            break;
                        case 1:
                            if (!db.Entry(action).Collection(x1 => x1.Specs).IsLoaded)
                                db.Entry(action).Collection(x1 => x1.Specs).Load();
                            isContains = (action.Specs.FirstOrDefault(x1 => x1.Id == item.Id) == null ? false : true);
                            break;
                        case 2:
                            if (!db.Entry(action).Collection(x1 => x1.Pros).IsLoaded)
                                db.Entry(action).Collection(x1 => x1.Pros).Load();
                            isContains = (action.Pros.FirstOrDefault(x1 => x1.Id == item.Id) == null ? false : true);
                            break;
                    }

                    
                }

                var selectedPros = new SelectedItem(item.Id, item.Name, isContains);
                listSelectedPros.Add(selectedPros);
            }

            return listSelectedPros;
        }

    }
}