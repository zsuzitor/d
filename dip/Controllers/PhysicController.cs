//#define debug


using dip.Models;
using dip.Models.Domain;
using dip.Models.ViewModel;
using dip.Models.ViewModel.PhysicV;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;  //JSON.NET
using Newtonsoft.Json.Linq;

using static dip.Models.Functions;
using WpfMath;





namespace dip.Controllers
{


    //TODO везде проверка на закрытый профиль
    [Authorize]
    [RequireHttps]
    public class PhysicController : Controller
    {
        // GET: Physic
        public ActionResult Index()
        {
            return View();
        }




        /// <summary>
        /// Отрисовывает страницу для просмотра ФЭ
        /// </summary>
        /// <param name="id">id ФЭ</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult Details(int? id)//, string technicalFunctionId
        {
            //TODO проверять есть ли доступ
            if (id == Models.Constants.FEIDFORSEMANTICSEARCH)//id временной записи для сематического поиска у нее нет дескрипторов и text=="---"
                return new HttpStatusCodeResult(404);
            DetailsV res = new DetailsV();
            try
            {
                res.Data(id, HttpContext);
            }
            catch
            {
                return new HttpStatusCodeResult(404);
            }






            return View(res);
        }

        /// <summary>
        /// Отрисовывает список семантически похожих записей
        /// </summary>
        /// <param name="id"> id ФЭ для которого необходимо найти схожие</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ShowSimilar(int id)//, string technicalFunctionId
        {
            ShowSimilarV res = new ShowSimilarV();
            res.ListSimilarIds = FEText.GetListSimilar(id, HttpContext, 5);


            return PartialView(res);
        }

        /// <summary>
        /// Отрисовывает список Фэ
        /// </summary>
        /// <param name="listId">Список id ФЭ</param>
        /// <param name="numLoad">Номер загрузки</param>
        /// <param name="emptyResult">При true и пустом списке вернет emptyresult</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ListFeText(int[] listId = null, int numLoad = 1, bool emptyResult = false)
        {
            ListFeTextV res = new ListFeTextV();

            if (listId == null)
            {
                listId = (int[])TempData["list_fe_id"];
            }
            listId = listId ?? new int[0];
            if (emptyResult && listId.Length == 0)
                return new EmptyResult();
            res.FeTexts = FEText.GetListIfAccess(HttpContext, listId);
            res.NumLoad = numLoad;


            return PartialView(res);
        }

        /// <summary>
        /// Отрисовывает страницу для редактирования ФЭ
        /// </summary>
        /// <param name="id">id ФЭ который нужно отредактировать</param>
        /// <returns>результат действия ActionResult</returns>
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == Models.Constants.FEIDFORSEMANTICSEARCH)//id временной записи для сематического поиска у нее нет дескрипторов и text=="---"
                return new HttpStatusCodeResult(404);
            EditV res = new EditV();
            res.Obj = FEText.Get(id);

            if (res.Obj == null)
                return new HttpStatusCodeResult(404);

            res.Data(id);



            return View(res);
        }

        /// <summary>
        /// post- сохранение измененного ФЭ
        /// </summary>
        /// <param name="obj">Новые данные ФЭ</param>
        /// <param name="uploadImage">Добавленные изображения</param>
        /// <param name="deleteImg_">Удаленные изображения</param>
        /// <param name="forms">Входные\выходные дескрипторы</param>
        /// <param name="objForms">Начальные\конечные характеристики объекта</param>
        /// <returns>результат действия ActionResult</returns>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(FEText obj, HttpPostedFileBase[] uploadImage, int[] deleteImg_, DescrSearchI[] forms = null,
            DescrObjectI[] objForms = null, ChangeLatex[] latexformulas = null)//string[] latexformulasAdd = null, int[] latexformulasDel = null)
        {
            bool commited = false;
            if (obj.IDFE == Models.Constants.FEIDFORSEMANTICSEARCH)//id временной записи для сематического поиска у нее нет дескрипторов и text=="---"
                return new HttpStatusCodeResult(224);
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(220);
            if (!obj.Validation())
                return new HttpStatusCodeResult(220);
            if ((obj.CountInput != 1 && forms.Length != 3) || (obj.CountInput == 1 && forms.Length != 2))
                return new HttpStatusCodeResult(221);
            if ((obj.ChangedObject && objForms.Length != 2) || (!obj.ChangedObject && objForms.Length != 1))
                return new HttpStatusCodeResult(222);
            FEText oldObj = FEText.Get(obj.IDFE);
            if (oldObj == null)
                return new HttpStatusCodeResult(223);

            foreach (var i in forms)
            {
                DescrSearchI.Validation(i);
                if (i?.Valide == false)
                    return new HttpStatusCodeResult(221);
                //i.DeleteNotChildCheckbox();
            }
            foreach (var i in objForms)
            {
                DescrObjectI.Validation(i);
                if (i?.Valide == false)
                    return new HttpStatusCodeResult(222);
            }


            var list_img_byte = Get_photo_post(uploadImage);

            List<int> deleteImg = null;
            if (deleteImg_ != null)
                deleteImg = deleteImg_.Distinct().ToList();


            //TODO валидация


            commited = oldObj.ChangeDb(obj, deleteImg, list_img_byte, forms.ToList(), objForms.ToList(), latexformulas);
            if (commited)
            {
                //return new HttpStatusCodeResult(404);
                Response.StatusCode = 201;
                return Content(Url.Action("Details", "Physic", new { id = obj.IDFE }), "text/html");
            }
            else
                return new HttpStatusCodeResult(225);


        }






        /// <summary>
        /// Отрисовывает страницу создания нового ФЭ
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {

            FEText res = new FEText();


            return View(res);
        }

        /// <summary>
        /// post- сохраняет новый ФЭ
        /// </summary>
        /// <param name="obj">Данные нового ФЭ</param>
        /// <param name="uploadImage">Загруженные изображения</param>
        /// <param name="forms">Входные\выходные дескрипторы </param>
        /// <param name="objForms">Начальные\конечные характеристики объекта</param>
        /// <param name="latexformulas">Формулы latex</param>
        /// <returns>результат действия ActionResult</returns>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(FEText obj, HttpPostedFileBase[] uploadImage, DescrSearchI[] forms = null, DescrObjectI[] objForms = null, string[] latexformulas = null)
        {

#if debug
            if (!ModelState.IsValid)
            {
                List<string> errorModel = new List<string>();
                foreach(var i in ModelState.Keys)
                {
                    if (ModelState[i].Errors.Count > 0)
                    {
                        errorModel.AddRange( ModelState[i].Errors.Select(x1 => i + "___" + x1.ErrorMessage).ToList());
                    }
                }
                int stop = 0;
            }
#endif

            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(220);

            if (!obj.Validation())
                return new HttpStatusCodeResult(220);


            if ((obj.CountInput != 1 && forms.Length != 3) || (obj.CountInput == 1 && forms.Length != 2))
                return new HttpStatusCodeResult(221);
            if ((obj.ChangedObject && objForms.Length != 2) || (!obj.ChangedObject && objForms.Length != 1))
                return new HttpStatusCodeResult(222);



            foreach (var i in forms)
            {
                DescrSearchI.Validation(i);
                if (i?.Valide == false)
                    return new HttpStatusCodeResult(221);
                //i.DeleteNotChildCheckbox();
            }

            foreach (var i in objForms)
            {
                DescrObjectI.Validation(i);
                if (i?.Valide == false)
                    return new HttpStatusCodeResult(222);
            }



            var list_img_byte = Get_photo_post(uploadImage);


            //новая
            bool success = obj.AddToDb(forms, objForms, list_img_byte, latexformulas);


            if (success && obj != null && obj.IDFE > 0)
            {
                Response.StatusCode = 201;
                return Content(Url.Action("Details", "Physic", new { id = obj.IDFE }), "text/html");
            }
            else
                return new HttpStatusCodeResult(225);

        }


        /// <summary>
        /// Отрисовывает списки к которым относится ФЭ
        /// </summary>
        /// <param name="idfe">id ФЭ</param>
        /// <returns>результат действия ActionResult</returns>
        [Authorize(Roles = "admin")]
        public ActionResult LoadLists(int idfe)//, string technicalFunctionId
        {
            var fe = FEText.Get(idfe);
            fe.LoadLists();

            return PartialView(fe.Lists);
        }




        /// <summary>
        /// Отрисовывает страницу для редактирования входных\выходных дескрипторов
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult CreateDescription()//, string technicalFunctionId
        {
            CreateDescriptionV res = new CreateDescriptionV();
            //res.Form= DescriptionForm.GetFormObject(null,null);
            res.Form = new DescriptionForm();
            using (var db = new ApplicationDbContext())
            {
                // Получаем список всех воздействий 
                res.Form.ActionId = db.AllActions.OrderBy(action => action.Id).ToList();
            }
            res.SetAllParametricAction();
            return View(res);
        }

        /// <summary>
        /// post- сохраняет изменения входных\выходных дескрипторов
        /// </summary>
        /// <param name="obj">Изменные данные</param>
        /// <param name="currentActionId">id Action для которого нужно применить изменения</param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult CreateDescription(SaveDescription obj, string currentActionId)//, string technicalFunctionId
        {

            obj.SetNotNullArray();
            if (string.IsNullOrEmpty(currentActionId))
                //throw new Exception();
                return new HttpStatusCodeResult(404);
            // bool notValide = false;
            bool commited = false;
            List<int> blockFe = new List<int>();

            bool? currentActionParametric = null;

            //TODO throw new Exception(); убрать

            if (obj.MassAddActionId.Length > 1)

                return new HttpStatusCodeResult(404);//"добавлено слишком много ActionId, это не предусмотрено"

            using (var db = new ApplicationDbContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //ActionId
                        int lastAllActionId = 0;
                        if (obj.MassAddActionId.Length > 0)
                        {
                            var allAction = db.AllActions.Select(x1 => x1.Id).ToList();
                            if (allAction.Count > 0)
                                lastAllActionId = allAction.Max(x1 => int.Parse(x1.Split(new string[] { "VOZ" }, StringSplitOptions.RemoveEmptyEntries)[0]));

                        }

                        currentActionParametric = obj.AddActionId(lastAllActionId, ref currentActionId, db);


                        if (currentActionParametric == null)
                            currentActionParametric = db.AllActions.FirstOrDefault(x1 => x1.Id == currentActionId)?.Parametric;

                        obj.EditActionId(db);

                        //FizVels
                        if (obj.MassAddFizVels.Length > 0 && currentActionParametric != null)
                        {
                            obj.AddFizVels(currentActionId, currentActionParametric, db);

                        }
                        obj.EditFizVels(db);

                        //ParamFizVels
                        if (currentActionParametric == true)
                        {

                            obj.AddParamFizVels(db);
                            obj.EditParamFizVels(db);
                        }


                        if (currentActionParametric == false)
                        {
                            //add checkbox
                            obj.AddPro(currentActionId, db);
                            obj.AddVrem(currentActionId, db);
                            obj.AddSpec(currentActionId, db);

                            obj.EditPro(db);
                            obj.EditVrem(db);
                            obj.EditSpec(db);

                            blockFe.AddRange(obj.DeletePros(db));
                            blockFe.AddRange(obj.DeleteSpec(db));
                            blockFe.AddRange(obj.DeleteVrem(db));

                        }
                        blockFe.AddRange(obj.DeleteFizVels(db));
                        blockFe.AddRange(obj.DeleteActionId(db));

                        db.SaveChanges();
                        transaction.Commit();
                        commited = true;
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            if (blockFe.Count > 0)
                return Content("Изменения сохранены(кроме удаления). Записи которые блокируют удаление:" + string.Join(", ", blockFe.Distinct()), "text/html");


            if (commited)
                return Content("+", "text/html");
            else
                return new HttpStatusCodeResult(404);

        }

        /// <summary>
        /// Отрисовывает страницу для изменения начальных\конечных характеристик объекта и состояний объекта
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult CreateDescriptionObject()//, string technicalFunctionId
        {

            CreateDescriptionObjectV res = new CreateDescriptionObjectV();

            // Получаем список всех воздействий 
            res.Characteristic = PhaseCharacteristicObject.GetBase();// db.PhaseCharacteristicObjects.ToList();
            res.States = StateObject.GetBase(); //db.StateObjects.ToList();

            return View(res);
        }

        /// <summary>
        /// post -сохраняет изменения начальных\конечных характеристик объекта и состояний объекта
        /// </summary>
        /// <param name="obj">Изменные данные</param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult CreateDescriptionObject(SaveDescriptionObject obj)//, string technicalFunctionId
        {
            bool commited = false;
            obj.SetNotNullArray();
            List<int> blockFe = obj.Save(out commited);

            if (blockFe.Count > 0)
                return Content("Изменения сохранены(кроме удаления). Записи которые блокируют удаление:" + string.Join(", ", blockFe.Distinct()), "text/html");

            if (commited)
                return Content("+", "text/html");
            else
                return new HttpStatusCodeResult(404);

            //return View();
        }


        /// <summary>
        /// Отрисовывает страницу с следующим ФЭ(по бд)
        /// </summary>
        /// <param name="id">id тукущего ФЭ</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult GoNextPhysics(int id)
        {
            //проверять есть ли доступ, и? загружать ту к которой доступ есть
            FEText phys = FEText.GetNextAccessPhysic(id, HttpContext);
            DetailsV res = new DetailsV();
            try
            {
                res.Data(phys, HttpContext);
            }
            catch
            {
                return new HttpStatusCodeResult(404);
            }
            return View("Details", res);
        }

        /// <summary>
        /// Отрисовывает страницу с предыдущим ФЭ(по бд)
        /// </summary>
        /// <param name="id">id тукущего ФЭ</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult GoPrevPhysics(int id)
        {
            //TODO проверять есть ли доступ, и мб загружать ту к которой доступ есть
            FEText phys = FEText.GetPrevAccessPhysic(id, HttpContext);
            DetailsV res = new DetailsV();
            try
            {
                res.Data(phys, HttpContext);
            }
            catch
            {
                return new HttpStatusCodeResult(404);
            }
            return View("Details", res);
        }

        /// <summary>
        /// Отрисовывает страницу с последним ФЭ(по бд)
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult GoLastPhysics()
        {
            //TODO проверять есть ли доступ, и мб загружать ту к которой доступ есть
            FEText phys = FEText.GetLastAccessPhysic(HttpContext);
            DetailsV res = new DetailsV();
            try
            {
                res.Data(phys, HttpContext);
            }
            catch
            {
                return new HttpStatusCodeResult(404);
            }
            return View("Details", res);
        }

        /// <summary>
        /// Отрисовывает страницу с первым ФЭ(по бд)
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult GoFirstPhysics()
        {
            //проверять есть ли доступ, и? мб загружать ту к которой доступ есть
            FEText phys = FEText.GetFirstAccessPhysic(HttpContext);
            DetailsV res = new DetailsV();
            try
            {
                res.Data(phys, HttpContext);
            }
            catch
            {
                return new HttpStatusCodeResult(404);
            }
            return View("Details", res);
        }


        /// <summary>
        /// post- насильное обнуление записи ФЭ которая служит для семантического поиска
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult ReloadSemanticRecord()
        {
            FEText.ReloadSemanticRecord();
            Response.StatusCode = 200;
            return Content("", "text/html");//Emty
        }


    }

}