#define debug


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






namespace dip.Controllers
{


    //TODO везде проверка на закрытый профиль
    [Authorize]
    public class PhysicController : Controller
    {
        // GET: Physic
        public ActionResult Index()
        {
            return View();
        }





        public ActionResult Details(int? id)//, string technicalFunctionId
        {
            //TODO проверять есть ли доступ
            if(id== Models.Constants.FEIDFORSEMANTICSEARCH)//id временной записи для сематического поиска у нее нет дескрипторов и text=="---"
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

        public ActionResult ShowSimilar(int id)//, string technicalFunctionId
        {
            ShowSimilarV res = new ShowSimilarV();
            res.ListSimilarIds = FEText.GetListSimilar(id, HttpContext,5);


            return PartialView(res);
        }


        public ActionResult ListFeText(int[] listId = null, int numLoad = 1)
        {
            ListFeTextV res = new ListFeTextV();

            if (listId == null)
            {
                listId = (int[])TempData["list_fe_id"];
            }
            listId = listId ?? new int[0];
            res.FeTexts = FEText.GetListIfAccess(HttpContext,listId);
            res.NumLoad = numLoad;


            return PartialView(res);
        }


        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == Models.Constants.FEIDFORSEMANTICSEARCH)//id временной записи для сематического поиска у нее нет дескрипторов и text=="---"
                return new HttpStatusCodeResult(404);
            EditV res = new EditV();
            res.Obj = FEText.Get(id);

            if (res.Obj == null)
                return new HttpStatusCodeResult(404);

            //res.ChangedObject=res.obj
            List < FEAction> inp = null;
            List<FEAction> outp = null;
            FEAction.Get((int)id, ref inp, ref outp);
            List < FEObject> inpObj = null;
            List < FEObject> outpObj = null;
            FEObject.Get((int)id, ref inpObj, ref outpObj);

            res.FormsInput = inp.Select(x1=> {
                var rs = new DescrSearchI(x1);
                rs.ListSelectedPros = Pro.GetAllIdsFor(rs.ListSelectedPros);
                rs.ListSelectedVrem = Vrem.GetAllIdsFor(rs.ListSelectedVrem);
                rs.ListSelectedSpec = Spec.GetAllIdsFor(rs.ListSelectedSpec);
                return rs;
            }).ToList();
            res.FormsOutput = outp.Select(x1 => {
                var rs = new DescrSearchI(x1);
                rs.ListSelectedPros = Pro.GetAllIdsFor(rs.ListSelectedPros);
                rs.ListSelectedVrem = Vrem.GetAllIdsFor(rs.ListSelectedVrem);
                rs.ListSelectedSpec = Spec.GetAllIdsFor(rs.ListSelectedSpec);
                return rs;
            }).ToList();// new DescrSearchI(outp);

            if (inpObj != null)
            {
                var objTmp = inpObj.FirstOrDefault(x1 => x1.NumPhase == 1);
                if (objTmp != null)
                    res.FormObjectBegin.ListSelectedPhase1 = new DescrPhaseI(objTmp);//.Select(x1=>new DescrPhaseI(x1));
                objTmp = inpObj.FirstOrDefault(x1 => x1.NumPhase == 2);
                if (objTmp != null)
                    res.FormObjectBegin.ListSelectedPhase2 = new DescrPhaseI(objTmp);
                objTmp = inpObj.FirstOrDefault(x1 => x1.NumPhase == 3);
                if (objTmp != null)
                    res.FormObjectBegin.ListSelectedPhase3 = new DescrPhaseI(objTmp);
                res.CountPhaseBegin = res.FormObjectBegin.GetCountPhase();
            }
           
            if (outpObj != null)
            {
                var objTmp = outpObj.FirstOrDefault(x1 => x1.NumPhase == 1);
                if (objTmp != null)
                    res.FormObjectEnd.ListSelectedPhase1 = new DescrPhaseI(objTmp);
                 objTmp = outpObj.FirstOrDefault(x1 => x1.NumPhase == 2);
                if (objTmp != null)
                    res.FormObjectEnd.ListSelectedPhase2 = new DescrPhaseI(objTmp);// outpObj.FirstOrDefault(x1 => x1.NumPhase == 2));
                 objTmp = outpObj.FirstOrDefault(x1 => x1.NumPhase == 3);
                if (objTmp != null)
                    res.FormObjectEnd.ListSelectedPhase3 = new DescrPhaseI(objTmp);//outpObj.FirstOrDefault(x1 => x1.NumPhase == 3));
                res.CountPhaseEnd = res.FormObjectEnd.GetCountPhase();
            }
           
            

            res.Obj.LoadImage();



            return View(res);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(FEText obj, HttpPostedFileBase[] uploadImage, int[] deleteImg_, DescrSearchI[] forms = null, DescrObjectI[] objForms = null)
        {
            bool commited = false;
            if (obj.IDFE == Models.Constants.FEIDFORSEMANTICSEARCH)//id временной записи для сематического поиска у нее нет дескрипторов и text=="---"
                return new HttpStatusCodeResult(404);
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(404);
            if (!obj.Validation())
                return new HttpStatusCodeResult(404);
            if((obj.CountInput != 1 && forms.Length != 3)||(obj.CountInput == 1 && forms.Length != 2))
                return new HttpStatusCodeResult(404);
            if ((obj.ChangedObject&& objForms.Length!=2)||(!obj.ChangedObject && objForms.Length != 1))
                return new HttpStatusCodeResult(404);
            FEText oldObj = FEText.Get(obj.IDFE);
            if (oldObj == null)
                return new HttpStatusCodeResult(404);

            foreach (var i in forms)
            {
                DescrSearchI.Validation(i);
                if (i?.Valide == false)
                    return new HttpStatusCodeResult(404);
                i.DeleteNotChildCheckbox();
            }
            foreach (var i in objForms)
            {
                DescrObjectI.Validation(i);
                if (i?.Valide == false)
                    return new HttpStatusCodeResult(404);
            }
            

            var list_img_byte = Get_photo_post(uploadImage);

            List<int> deleteImg = null;
            if (deleteImg_ != null)
                deleteImg = deleteImg_.Distinct().ToList();


            //TODO валидация


            commited = oldObj.ChangeDb(obj, deleteImg, list_img_byte, forms.ToList(), objForms.ToList());
            if (commited)
            {
                //return new HttpStatusCodeResult(404);
                Lucene_.UpdateDocument(obj.IDFE.ToString(), obj);
                return Content(Url.Action("Details", "Physic", new { id = obj.IDFE }), "text/html");
            }
            else
                return new HttpStatusCodeResult(404);


            //oldObj.LoadImage();
            //return View(@"~/Views/Physic/Details.cshtml", oldObj);
            //return RedirectToAction("Details", "Physic", new { id = oldObj.IDFE });
            //if (commited)
            //    return Content(Url.Action("Details", "Physic", new { id = obj.IDFE }), "text/html");
            //else
            //    return new HttpStatusCodeResult(404);
            //return Content(Url.Action("Details", "Physic", new { id = obj.IDFE }), "text/html");
        }







        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {

            FEText res = new FEText();


            return View(res);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(FEText obj, HttpPostedFileBase[] uploadImage, DescrSearchI[] forms = null, DescrObjectI[] objForms = null)
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
                return new HttpStatusCodeResult(404);

            if (!obj.Validation())
                return new HttpStatusCodeResult(404);

            //if(forms.Length<2)
            //    return new HttpStatusCodeResult(404);
            //if (objForms.Length == 0)
            //    return new HttpStatusCodeResult(404);

            if ((obj.CountInput != 1 && forms.Length != 3) || (obj.CountInput == 1 && forms.Length != 2))
                return new HttpStatusCodeResult(404);
            if ((obj.ChangedObject && objForms.Length != 2) || (!obj.ChangedObject && objForms.Length != 1))
                return new HttpStatusCodeResult(404);



            foreach (var i in forms)
            {
                DescrSearchI.Validation(i);
                if (i?.Valide == false)
                    return new HttpStatusCodeResult(404);
                i.DeleteNotChildCheckbox();
            }
            
            foreach (var i in objForms)
            {
                DescrObjectI.Validation(i);
                if (i?.Valide == false)
                    return new HttpStatusCodeResult(404);
            }


            
            var list_img_byte = Get_photo_post(uploadImage);


            //DescrObjectI[] objForms = ; //TODO-objForms


            //новая
            bool success=obj.AddToDb(forms, objForms, list_img_byte);



            //obj.LoadImage();
            //return View(@"~/Views/Physic/Details.cshtml", oldObj);
            //return RedirectToAction("Details", "Physic", new { id = obj.IDFE });
            if(success&&obj!=null&&obj.IDFE>0)
            return Content(Url.Action("Details", "Physic",new {id=obj.IDFE }), "text/html");
            else
                return new HttpStatusCodeResult(404);

        }

        [Authorize(Roles = "admin")]
        public ActionResult LoadLists(int idfe)//, string technicalFunctionId
        {
            var fe = FEText.Get(idfe);
            fe.LoadLists();

            return PartialView(fe.Lists);
        }





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

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult CreateDescription(SaveDescription obj, string currentActionId)//, string technicalFunctionId
        {
            //type: 1-actionid ,2-fizvell,3-paramfizvell,4-pros,5-spec,6-vrem
            //TypeAction: 1-добавление ,2-редактирование, 3- удаление
            //return Content("+", "text/html");//TODO delete

            obj.SetNotNullArray();
            if (string.IsNullOrEmpty(currentActionId))
                throw new Exception();
            bool notValide = false;
            bool commited = false;
            List<int> blockFe = new List<int>();

            //TODO надо найти то к чему все будет относиться, это может вернуть null
            //string currentActionId = obj.TryGetCurrentActionId();// massData.FirstOrDefault(x1 => x1.Type == 1);
            bool? currentActionParametric = null;

            //TODO throw new Exception(); убрать

            if (obj.MassAddActionId.Length > 1)//TODO
                throw new Exception("добавлено слишком много ActionId, это не предусмотрено");

            if (notValide)//TODO
                return new HttpStatusCodeResult(404);
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
                            //.OrderBy(x1 => int.Parse(x1.Id.Split(new string[] { "VOZ" }, StringSplitOptions.RemoveEmptyEntries)[0]))
                            //.Select(x1 => int.Parse(x1.Id.Split(new string[] { "VOZ" }, StringSplitOptions.RemoveEmptyEntries)[0])).Last();
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
                        //TODO удаление всего в самом конце начиная с детей
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            if (blockFe.Count > 0)
                return Content("Изменения сохранены(кроме удаления). Записи которые блокируют удаление:" + string.Join(",", blockFe.Distinct()), "text/html");


            if (commited )
                return Content("+", "text/html");
            else
                return new HttpStatusCodeResult(404);

            //return RedirectToAction("CreateDescription");
            //return View();
        }

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

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult CreateDescriptionObject(SaveDescriptionObject obj)//, string technicalFunctionId
        {
            bool commited = false;
            obj.SetNotNullArray();
            List<int>blockFe= obj.Save(out commited);
            
            if (blockFe.Count>0)
                return  Content("Изменения сохранены(кроме удаления). Записи которые блокируют удаление:" + string.Join(",",blockFe.Distinct()), "text/html");

            if (commited)
                return Content("+", "text/html");
            else
                return new HttpStatusCodeResult(404);

            //return View();
        }



        public ActionResult GoNextPhysics(int id)
        {
            //TODO проверять есть ли доступ, и мб загружать ту к которой доступ есть
            FEText phys= FEText.GetNextAccessPhysic(id,HttpContext);
            DetailsV res = new DetailsV();
            try
            {
                res.Data(phys, HttpContext);
            }
            catch
            {
                return new HttpStatusCodeResult(404);
            }
            return View("Details",res);
        }


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

        public ActionResult GoFirstPhysics()
        {
            //TODO проверять есть ли доступ, и мб загружать ту к которой доступ есть
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


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult ReloadSemanticRecord()
        {
            FEText.ReloadSemanticRecord();
            Response.StatusCode = 200;
            return Content("", "text/html");//Emty
        }

            //-------------------------------------------------------------------------------------------------------------------------------------------












        }



}