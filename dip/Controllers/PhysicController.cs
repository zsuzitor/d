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
            DetailsV res = new DetailsV();
            try
            {
                 res.Data(id, HttpContext);
            }
            catch
            {
                return new HttpStatusCodeResult(404);
            }
            //res.Effect = FEText.Get(id);
            //if (res.Effect == null)
            //    return new HttpStatusCodeResult(404);
            //string check_id = ApplicationUser.GetUserId();

            //res.Effect.LoadImage();
            //res.EffectName = res.Effect.Name;
            ////TODO почему именно так?
            ////res.TechnicalFunctionId = Request.Params.GetValues(0).First();

            //if (check_id != null)
            //{
            //    ApplicationUserManager userManager = HttpContext.GetOwinContext()
            //                             .GetUserManager<ApplicationUserManager>();
            //    IList<string> roles = userManager?.GetRoles(check_id);
            //    if (roles != null)
            //        if (roles.Contains("admin"))
            //            res.Admin = true;
            //    res.Favourited = res.Effect.Favourited(check_id);
            //}



            return View(res);
        }

        public ActionResult ShowSimilar(int id)//, string technicalFunctionId
        {
            ShowSimilarV res = new ShowSimilarV();
            res.ListSimilarIds = FEText.GetListSimilar(id);


            return PartialView(res);
        }


        public ActionResult ListFeText(int[] listId = null, int numLoad = 1)
        {
            ListFeTextV res = new ListFeTextV();

            if (listId == null)
            {
                listId = (int[])TempData["list_fe_id"];
            }
            res.FeTexts = FEText.GetList(listId);
            res.NumLoad = numLoad;


            return PartialView(res);
        }


        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            EditV res = new EditV();
            res.Obj = FEText.Get(id);

            if (res == null)
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
                var objTmp =  inpObj.FirstOrDefault(x1 => x1.NumPhase == 1);
                if(objTmp!=null)
                res.FormObjectBegin.ListSelectedPhase1 = new DescrPhaseI(objTmp);//.Select(x1=>new DescrPhaseI(x1));
                 objTmp = inpObj.FirstOrDefault(x1 => x1.NumPhase == 2);
                if (objTmp != null)
                    res.FormObjectBegin.ListSelectedPhase2 = new DescrPhaseI(objTmp);
                  objTmp = inpObj.FirstOrDefault(x1 => x1.NumPhase == 3);
                if (objTmp != null)
                    res.FormObjectBegin.ListSelectedPhase3 = new DescrPhaseI(objTmp);
            }
           
            if (outpObj != null)
            {
                var objTmp = outpObj.FirstOrDefault(x1 => x1.NumPhase == 1);
                if (objTmp != null)
                    res.FormObjectEnd.ListSelectedPhase1 = new DescrPhaseI(objTmp);
                 objTmp = outpObj.FirstOrDefault(x1 => x1.NumPhase == 2);
                if (objTmp != null)
                    res.FormObjectEnd.ListSelectedPhase2 = new DescrPhaseI(outpObj.FirstOrDefault(x1 => x1.NumPhase == 2));
                 objTmp = outpObj.FirstOrDefault(x1 => x1.NumPhase == 3);
                if (objTmp != null)
                    res.FormObjectEnd.ListSelectedPhase3 = new DescrPhaseI(outpObj.FirstOrDefault(x1 => x1.NumPhase == 3));
            }
           
            //TODO
            //res.FormInput.listSelectedProsI = Pro.GetAllIdsFor(res.FormInput.listSelectedProsI);
            //res.FormInput.listSelectedVremI = Vrem.GetAllIdsFor(res.FormInput.listSelectedVremI);
            //res.FormInput.listSelectedSpecI = Spec.GetAllIdsFor(res.FormInput.listSelectedSpecI);

            //res.FormOutput.listSelectedProsO = Pro.GetAllIdsFor(res.FormOutput.listSelectedProsO);
            //res.FormOutput.listSelectedVremO = Vrem.GetAllIdsFor(res.FormOutput.listSelectedVremO);
            //res.FormOutput.listSelectedSpecO = Spec.GetAllIdsFor(res.FormOutput.listSelectedSpecO);


           // DescrObjectI[] objForms = ;//TODO-objForms

            res.Obj.LoadImage();



            return View(res);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(FEText obj, HttpPostedFileBase[] uploadImage, int[] deleteImg_, DescrSearchI[] forms = null, DescrObjectI[] objForms = null)
        {

            if (!ModelState.IsValid)
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
            //DescrSearchIInput.ValidationIfNeed(inp);

            //DescrSearchIOut.ValidationIfNeed(outp);
            //if (inp?.Valide == false || outp?.Valide == false)
            //    return new HttpStatusCodeResult(404);


            var list_img_byte = Get_photo_post(uploadImage);

            List<int> deleteImg = null;
            if (deleteImg_ != null)
                deleteImg = deleteImg_.Distinct().ToList();
            //using (ApplicationDbContext db=new ApplicationDbContext())
            //{

            //TODO валидация


            //foreach (var i in forms)
            //    i.DeleteNotChildCheckbox();
            //DescrSearchI inp_ = new DescrSearchI(inp);
            //inp_.DeleteNotChildCheckbox();
            //DescrSearchI outp_ = new DescrSearchI(outp);
            //outp_.DeleteNotChildCheckbox();

            if (!oldObj.ChangeDb(obj, deleteImg, list_img_byte, forms.ToList(), objForms.ToList()))
                return new HttpStatusCodeResult(404);
            Lucene_.UpdateDocument(obj.IDFE.ToString(), obj);

            //DescrObjectI[] objForms = ;//TODO-objForms

            //oldObj.LoadImage();
            //return View(@"~/Views/Physic/Details.cshtml", oldObj);
            return RedirectToAction("Details", "Physic", new { id = oldObj.IDFE });
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

            //if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            //{
            //    var pic = System.Web.HttpContext.Current.Request.Files["uploadImage[0]"];
            //}
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(404);

            if (!obj.Validation())
                return new HttpStatusCodeResult(404);

            foreach (var i in forms)
            {
                DescrSearchI.Validation(i);
                if (i?.Valide == false)
                    return new HttpStatusCodeResult(404);
                i.DeleteNotChildCheckbox();
            }

            

            //DescrSearchI inp_ = new DescrSearchI(inp);
            //inp_.DeleteNotChildCheckbox();
            //DescrSearchI outp_ = new DescrSearchI(outp);
            //outp_.DeleteNotChildCheckbox();

            var list_img_byte = Get_photo_post(uploadImage);


            //DescrObjectI[] objForms = ; //TODO-objForms


            //новая
            obj.AddToDb(forms, objForms, list_img_byte);



            //obj.LoadImage();
            //return View(@"~/Views/Physic/Details.cshtml", oldObj);
            return RedirectToAction("Details", "Physic", new { id = obj.IDFE });
        }












        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult CreateSomething()//, string technicalFunctionId
        {

            return View();
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

 
            obj.SetNotNullArray();
            if (string.IsNullOrEmpty(currentActionId))
                throw new Exception();
            bool notValide = false;



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

                //ActionId
                int lastAllActionId = 0;
                if(obj.MassAddActionId.Length>0)
                {
                    var allAction = db.AllActions.ToList();
                    if (allAction.Count > 0)
                        lastAllActionId = allAction.Max(x1 => int.Parse(x1.Id.Split(new string[] { "VOZ" }, StringSplitOptions.RemoveEmptyEntries)[0]));
                    //.OrderBy(x1 => int.Parse(x1.Id.Split(new string[] { "VOZ" }, StringSplitOptions.RemoveEmptyEntries)[0]))
                    //.Select(x1 => int.Parse(x1.Id.Split(new string[] { "VOZ" }, StringSplitOptions.RemoveEmptyEntries)[0])).Last();
                }

                currentActionParametric=obj.AddActionId(lastAllActionId, ref currentActionId,  db);
                //foreach (var i in obj.MassAddActionId)//возможно вытащить из цикла тк сейчас нельзя добавить больше 1
                //{
                //    var allAction = new Models.Domain.AllAction() { Name = i.Text, Parent = "ALLACTIONS", Parametric = i.Parametric, Id = ("VOZ" + ++lastAllActionId) };
                //    db.AllActions.Add(allAction);
                //    db.SaveChanges();
                //    //i.NewId = allAction.Id;
                //    currentActionId = allAction.Id;
                //    currentActionParametric = i.Parametric;
                //    //break;//TODO


                ////надо обновить в pros... parentid там где =="VOZ0"? 
                //foreach(var i2 in obj.MassAddPros)
                //    {
                //        i2.ParentId = i2.ParentId.Replace("VOZ0", currentActionId);
                //        i2.Id = i2.Id.Replace("VOZ0", currentActionId);
                //    }

                //    foreach (var i2 in obj.MassAddVrems)
                //    {
                //        i2.ParentId = i2.ParentId.Replace("VOZ0", currentActionId);
                //        i2.Id = i2.Id.Replace("VOZ0", currentActionId);
                //    }

                //    foreach (var i2 in obj.MassAddSpecs)
                //    {
                //        i2.ParentId = i2.ParentId.Replace("VOZ0", currentActionId);
                //        i2.Id = i2.Id.Replace("VOZ0", currentActionId);
                //    }


                //}

                if ( currentActionParametric == null)
                    currentActionParametric = db.AllActions.FirstOrDefault(x1 => x1.Id == currentActionId)?.Parametric;

                obj.EditActionId(db);
                //foreach (var i in obj.MassEditActionId)
                //{
                //    var act = db.AllActions.FirstOrDefault(x1 => x1.Id == i.Id);
                //    if (act != null)
                //        act.Name = i.Text;
                //    else
                //        i.Id = null;


                //    db.SaveChanges();
                //}



                //FizVels
                if ( obj.MassAddFizVels.Length > 0 && currentActionParametric != null)
                {
                    obj.AddFizVels(currentActionId, currentActionParametric, db);
                    //List<FizVel> fizvels = db.FizVels.Where(x1 => x1.Id.Contains(currentActionId + "_FIZVEL")).ToList(); 
                    ////if (currentActionParametric == false)
                    ////    fizvels=//выберет и не параметрические
                    ////else
                    ////    fizvels = db.FizVels.Where(x1 => x1.Id.Contains(currentActionId + "_FIZVEL_R")).ToList();
                    //int lastFizVel = 0;
                    //if (fizvels.Count > 0)
                    //    if (currentActionParametric == false)
                    //    {
                    //        lastFizVel = fizvels.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentActionId + "_FIZVEL_") }, StringSplitOptions.RemoveEmptyEntries)[0]));
                    //    }
                    //    else
                    //    {
                    //        lastFizVel = fizvels.Max(x1 =>
                    //        {
                    //            int rs;
                    //            int.TryParse(x1.Id.Split(new string[] { (currentActionId + "_FIZVEL_R") }, StringSplitOptions.RemoveEmptyEntries)[0],out rs);
                    //            return rs;
                    //        });
                    //    }

                    //foreach (var i in obj.MassAddFizVels)
                    //{
                    //    string fizVelId = "";
                    //    if (currentActionParametric == false)
                    //        fizVelId = (currentActionId + "_FIZVEL_" + ++lastFizVel);
                    //    else
                    //        fizVelId = (currentActionId + "_FIZVEL_R" + ++lastFizVel);


                    //    db.FizVels.Add(new Models.Domain.FizVel()
                    //    {
                    //        Name = i.Text,
                    //        Parent = currentActionId + "_FIZVEL",
                    //        Id = fizVelId
                    //    });
                    //    db.SaveChanges();
                    //    foreach(var i2 in obj.MassAddParamFizVels)
                    //    {
                    //        if (i2.ParentId ==i.Id)
                    //            i2.ParentId = fizVelId;
                    //    }
                    //}
                }
                obj.EditFizVels( db);
                //foreach (var i in obj.MassEditFizVels)
                //{
                //    var fizVel = db.FizVels.FirstOrDefault(x1 => x1.Id == i.Id);
                //    if (fizVel != null)
                //        fizVel.Name = i.Text;
                //    else
                //        i.Id = null;

                //    db.SaveChanges();
                //}



                //ParamFizVels
                if(currentActionParametric == true)
                {

                    obj.AddParamFizVels(db);
                    //if (obj.MassAddParamFizVels.Length > 0)
                    //{


                    //    string currentFizVels = obj.MassAddParamFizVels[0].ParentId;

                    //    var fizvels = db.FizVels.Where(x1 => x1.Id.Contains(currentFizVels + "_")).ToList();//("VOZ" + currentActionId + "_FIZVEL_R"+ currentFizVels)
                    //    int lastFizVel = 0;
                    //    if (fizvels.Count > 0)
                    //        lastFizVel = fizvels.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentFizVels + "_") }, StringSplitOptions.RemoveEmptyEntries)[0]));



                    //    foreach (var i in obj.MassAddParamFizVels)
                    //    {
                    //        db.FizVels.Add(new Models.Domain.FizVel()
                    //        {
                    //            Name = i.Text,
                    //            Parent = currentFizVels,
                    //            Id = (currentFizVels + "_" + ++lastFizVel),
                    //            Parametric = true
                    //        });
                    //        db.SaveChanges();
                    //    }
                    //}

                    //foreach (var i in obj.MassEditParamFizVels)
                    //{
                    //    var fizVel = db.FizVels.FirstOrDefault(x1 => x1.Id == i.Id);
                    //    if (fizVel != null)
                    //        fizVel.Name = i.Text;
                    //    else
                    //        i.Id = null;

                    //    db.SaveChanges();
                    //}

                    obj.EditParamFizVels(db);
                }





                if ( currentActionParametric == false)
                {
                    //add checkbox

                    //pro
                    {
                        obj.AddPro(currentActionId,db);
                        //int last = 0;
                        //var pros = db.Pros.Where(x1 => x1.Id.Contains(currentActionId + "_PROS")).ToList();
                        //if (pros.Count > 0)
                        //    last = pros.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentActionId + "_PROS") }, StringSplitOptions.RemoveEmptyEntries)[0]));
                        ////List<JsonSaveDescription> done = new List<JsonSaveDescription>();

                        //int countLastIter = 0;
                        //List<SaveDescriptionEntry> massAddProsList = obj.MassAddPros.ToList();
                        //while (massAddProsList.Count > 0)
                        //{

                        //    if (countLastIter == massAddProsList.Count)
                        //        throw new Exception("непонятные PRO");
                        //    countLastIter = massAddProsList.Count;
                        //    for (int i = 0; i < massAddProsList.Count; ++i)
                        //    {
                        //        //проверить можно ли добавить, и добавить и вынести в массив
                        //        if (!massAddProsList[i].ParentId.Contains("_NEW"))
                        //        {
                        //            var pro = new Models.Domain.Pro()
                        //            {
                        //                Name = massAddProsList[i].Text,
                        //                Parent = (massAddProsList[i].ParentId.Contains("_PROS") ? massAddProsList[i].ParentId : massAddProsList[i].ParentId + "_PROS"),
                        //                Id = (currentActionId + "_PROS" + ++last)//VOZ1_PROS1
                        //            };

                        //            db.Pros.Add(pro);
                        //            db.SaveChanges();
                        //            for (int i2 = 0; i2 < massAddProsList.Count; ++i2)
                        //            {
                        //                if (massAddProsList[i2].ParentId == massAddProsList[i].Id)
                        //                {
                        //                    massAddProsList[i2].ParentId = pro.Id;
                        //                }
                        //            }
                        //            massAddProsList.Remove(massAddProsList[i--]);

                        //        }


                        //    }
                        //}
                    }

                    //vrem
                    {
                        obj.AddVrem(currentActionId,db);
                        //int last = 0;
                        //var vrems = db.Vrems.Where(x1 => x1.Id.Contains(currentActionId + "_VREM")).ToList();
                        //if (vrems.Count > 0)
                        //    last = vrems.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentActionId + "_VREM") }, StringSplitOptions.RemoveEmptyEntries)[0]));
                        ////List<JsonSaveDescription> done = new List<JsonSaveDescription>();

                        //int countLastIter = 0;

                        //List<SaveDescriptionEntry> massAddVremsList = obj.MassAddVrems.ToList();
                        //while (massAddVremsList.Count > 0)
                        //{

                        //    if (countLastIter == massAddVremsList.Count)
                        //        throw new Exception("непонятные Vrem");
                        //    countLastIter = massAddVremsList.Count;
                        //    for (int i = 0; i < massAddVremsList.Count; ++i)
                        //    {
                        //        //проверить можно ли добавить, и добавить и вынести в массив
                        //        if (!massAddVremsList[i].ParentId.Contains("_NEW"))
                        //        {
                        //            var vrem = new Models.Domain.Vrem()
                        //            {
                        //                Name = massAddVremsList[i].Text,
                        //                Parent = (massAddVremsList[i].ParentId.Contains("_VREM") ? massAddVremsList[i].ParentId : massAddVremsList[i].ParentId + "_VREM"),

                        //                Id = (currentActionId + "_VREM" + ++last)
                        //            };

                        //            db.Vrems.Add(vrem);
                        //            db.SaveChanges();
                        //            for (int i2 = 0; i2 < massAddVremsList.Count; ++i2)
                        //            {
                        //                if (massAddVremsList[i2].ParentId == massAddVremsList[i].Id)
                        //                {
                        //                    massAddVremsList[i2].ParentId = vrem.Id;
                        //                }
                        //            }
                        //            massAddVremsList.Remove(massAddVremsList[i--]);

                        //        }


                        //    }
                        //}
                    }

                    //spec
                    {
                        obj.AddSpec(currentActionId,db);
                        //int last = 0;
                        //var specs = db.Specs.Where(x1 => x1.Id.Contains(currentActionId + "_SPEC")).ToList();
                        //if (specs.Count > 0)
                        //    last = specs.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentActionId + "_SPEC") }, StringSplitOptions.RemoveEmptyEntries)[0]));
                        ////List<JsonSaveDescription> done = new List<JsonSaveDescription>();

                        //int countLastIter = 0;
                        //List<SaveDescriptionEntry> massAddSpecsList = obj.MassAddSpecs.ToList();
                        //while (massAddSpecsList.Count > 0)
                        //{

                        //    if (countLastIter == massAddSpecsList.Count)
                        //        throw new Exception("непонятные Spec");
                        //    countLastIter = massAddSpecsList.Count;
                        //    for (int i = 0; i < massAddSpecsList.Count; ++i)
                        //    {
                        //        //проверить можно ли добавить, и добавить и вынести в массив
                        //        if (!massAddSpecsList[i].ParentId.Contains("_NEW"))
                        //        {
                        //            var spec = new Models.Domain.Spec()
                        //            {
                        //                Name = massAddSpecsList[i].Text,
                        //                Parent = (massAddSpecsList[i].ParentId.Contains("_SPEC") ? massAddSpecsList[i].ParentId : massAddSpecsList[i].ParentId + "_SPEC"),

                        //                Id = (currentActionId + "_SPEC" + ++last)
                        //            };

                        //            db.Specs.Add(spec);
                        //            db.SaveChanges();
                        //            for (int i2 = 0; i2 < massAddSpecsList.Count; ++i2)
                        //            {
                        //                if (massAddSpecsList[i2].ParentId == massAddSpecsList[i].Id)
                        //                {
                        //                    massAddSpecsList[i2].ParentId = spec.Id;
                        //                }
                        //            }
                        //            massAddSpecsList.Remove(massAddSpecsList[i--]);

                        //        }


                        //    }
                        //}
                    }


                    obj.EditPro(db);
                    obj.EditVrem(db);
                    obj.EditSpec(db);

                    //foreach (var i in obj.MassEditPros)
                    //{
                    //    var act = db.Pros.FirstOrDefault(x1 => x1.Id == i.Id);
                    //    if (act != null)
                    //        act.Name = i.Text;
                    //    else
                    //        i.Id = null;

                    //    db.SaveChanges();
                    //}
                    //foreach (var i in obj.MassEditSpecs)
                    //{
                    //    var act = db.Specs.FirstOrDefault(x1 => x1.Id == i.Id);
                    //    if (act != null)
                    //        act.Name = i.Text;
                    //    else
                    //        i.Id = null;

                    //    db.SaveChanges();
                    //}
                    //foreach (var i in obj.MassEditVrems)
                    //{
                    //    var act = db.Vrems.FirstOrDefault(x1 => x1.Id == i.Id);
                    //    if (act != null)
                    //        act.Name = i.Text;
                    //    else
                    //        i.Id = null;

                    //    db.SaveChanges();
                    //}

                    obj.DeletePros(db);
                    obj.DeleteSpec(db);
                    obj.DeleteVrem(db);
                    {
                        
                        //List<Pro> forDeleted = new List<Pro>();

                        //int start = 0;
                        //foreach (var i in obj.MassDeletedPros)
                        //{
                        //    var pro = db.Pros.FirstOrDefault(x1 => x1.Id == i.Id);
                        //    if (pro == null)
                        //        continue;
                        //    forDeleted.Add(pro);
                        //    //var childs=Pro.GetChild(pro.Id);
                        //    //    forDeleted.AddRange(childs);
                        //    for (; start < forDeleted.Count; ++start)
                        //    {
                        //        forDeleted.AddRange(Pro.GetChild(forDeleted[start].Id));
                        //    }
                        //}
                        //db.Pros.RemoveRange(forDeleted);
                        //db.SaveChanges();
                    }

                    {
                        //List<Spec> forDeleted = new List<Spec>();

                        //int start = 0;
                        //foreach (var i in obj.MassDeletedSpecs)
                        //{
                        //    var spec = db.Specs.FirstOrDefault(x1 => x1.Id == i.Id);
                        //    if (spec == null)
                        //        continue;
                        //    forDeleted.Add(spec);
                        //    //var childs=Pro.GetChild(pro.Id);
                        //    //    forDeleted.AddRange(childs);
                        //    for (; start < forDeleted.Count; ++start)
                        //    {
                        //        forDeleted.AddRange(Spec.GetChild(forDeleted[start].Id));
                        //    }
                        //}
                        //db.Specs.RemoveRange(forDeleted);
                        //db.SaveChanges();
                    }

                    {
                        //List<Vrem> forDeleted = new List<Vrem>();

                        //int start = 0;
                        //foreach (var i in obj.MassDeletedVrems)
                        //{
                        //    var vrem = db.Vrems.FirstOrDefault(x1 => x1.Id == i.Id);
                        //    if (vrem == null)
                        //        continue;
                        //    forDeleted.Add(vrem);
                        //    //var childs=Pro.GetChild(pro.Id);
                        //    //    forDeleted.AddRange(childs);
                        //    for (; start < forDeleted.Count; ++start)
                        //    {
                        //        forDeleted.AddRange(Vrem.GetChild(forDeleted[start].Id));
                        //    }
                        //}
                        //db.Vrems.RemoveRange(forDeleted);
                        //db.SaveChanges();
                    }

                }




                //TODO удаление всего в самом конце начиная с детей
            }
            return RedirectToAction("CreateDescription");
            //return View();
        }





        public ActionResult GoNextPhysics(int id)
        {
            //TODO проверять есть ли доступ, и мб загружать ту к которой доступ есть
            FEText phys= FEText.GetNext(id);
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
            FEText phys = FEText.GetPrev(id);
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
            FEText phys = FEText.GetLast();
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
            FEText phys = FEText.GetFirst();
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


        //-------------------------------------------------------------------------------------------------------------------------------------------












    }



}