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

                



                if (actionId != -1) // значение идентификатора не равно -1
                {
                    // Извлекаем воздействие из БД
                    var action = db.Actions.Find(actionId);

                    // Преобразуем список характеристик к нужному типу
                    listSelectedPros = GetListSelectedItem(prosList, action,db);
                }
                else
                    // Преобразуем список характеристик к нужному типу
                    listSelectedPros = GetListSelectedItem(prosList,null,db);
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





                if (actionId != -1) // значение идентификатора не равно -1
                {
                    // Извлекаем воздействие из БД
                    var action = db.Actions.Find(actionId);

                    // Преобразуем список характеристик к нужному типу
                    listSelectedSpec = GetListSelectedItem(specList, action,db);
                }
                else
                    // Преобразуем список характеристик к нужному типу
                    listSelectedSpec = GetListSelectedItem(specList,null,db);
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



                if (actionId != -1) // значение идентификатора не равно -1
                {
                    // Извлекаем воздействие из БД
                    var action = db.Actions.Find(actionId);

                    // Преобразуем список характеристик к нужному типу
                    listSelectedVrem = GetListSelectedItem(vremList, action,db);
                }
                else
                    // Преобразуем список характеристик к нужному типу
                    listSelectedVrem = GetListSelectedItem(vremList,null,db);
            }
            // Отправляем полученный список в представление
            ViewBag.vremChild = listSelectedVrem;
            ViewBag.parent = vremId;

            return PartialView();
        }










        //----------------------------------------------------------------PRIVATE





        /// <summary>
        /// приведение списка пространственных характеристик к типу List<SelectedItem> с учетом выбранного воздействия
        /// </summary>
        /// <param name="list"> список пространственных характеристик</param>
        /// <param name="action"> воздействие </param>
        /// <returns> список пространственных характеристик типа List<SelectedItem> </returns>
        private List<SelectedItem> GetListSelectedItem(List<Pro> list, Models.Domain.Action action,ApplicationDbContext db)
        {
            // Сортируем список характеристик
            list = list.OrderBy(pros => pros.Parent).ToList();

            // Создаем список List<SelectedItem>
            var listSelectedPros = new List<SelectedItem>();

            // Приводим List<Pros> к List<SelectedItem>
            foreach (var pros in list)
            {
                // Проверяем, отмечена ли характеристика в воздействии
                bool isContains = false;
                if (action != null)
                {
                    if (!db.Entry(action).Collection(x1 => x1.Pros).IsLoaded)
                        db.Entry(action).Collection(x1 => x1.Pros).Load();
                    isContains = action.Pros.Contains(pros);
                }
                 
                var selectedPros = new SelectedItem(pros.Id, pros.Name, isContains);
                listSelectedPros.Add(selectedPros);
            }

            return listSelectedPros;
        }

        
        private List<SelectedItem> GetListSelectedItem(List<Spec> list, Models.Domain.Action action, ApplicationDbContext db)
        {
            // Сортируем список характеристик
            list = list.OrderBy(pros => pros.Parent).ToList();

            // Создаем список List<SelectedItem>
            var listSelectedPros = new List<SelectedItem>();

            // Приводим List<Pros> к List<SelectedItem>
            foreach (var pros in list)
            {
                // Проверяем, отмечена ли характеристика в воздействии
                bool isContains = false;
                if (action != null)
                {
                    if (!db.Entry(action).Collection(x1 => x1.Specs).IsLoaded)
                        db.Entry(action).Collection(x1 => x1.Specs).Load();
                    isContains = action.Specs.Contains(pros);
                }

                var selectedPros = new SelectedItem(pros.Id, pros.Name, isContains);
                listSelectedPros.Add(selectedPros);
            }

            return listSelectedPros;
        }



        private List<SelectedItem> GetListSelectedItem(List<Vrem> list, Models.Domain.Action action, ApplicationDbContext db)
        {
            // Сортируем список характеристик
            list = list.OrderBy(pros => pros.Parent).ToList();

            // Создаем список List<SelectedItem>
            var listSelectedPros = new List<SelectedItem>();

            // Приводим List<Pros> к List<SelectedItem>
            foreach (var pros in list)
            {
                // Проверяем, отмечена ли характеристика в воздействии
                bool isContains = false;
                if (action != null)
                {
                    if (!db.Entry(action).Collection(x1 => x1.Vrems).IsLoaded)
                        db.Entry(action).Collection(x1 => x1.Vrems).Load();
                    isContains = action.Vrems.Contains(pros);
                }

                var selectedPros = new SelectedItem(pros.Id, pros.Name, isContains);
                listSelectedPros.Add(selectedPros);
            }

            return listSelectedPros;
        }

    }
}