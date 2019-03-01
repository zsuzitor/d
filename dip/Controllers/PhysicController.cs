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
            DetailsV res = new DetailsV();
            res.Effect = FEText.Get(id);
            if (res.Effect == null)
                return new HttpStatusCodeResult(404);
            string check_id = ApplicationUser.GetUserId();

            res.Effect.LoadImage();
            res.EffectName = res.Effect.Name;
            //TODO почему именно так?
            res.TechnicalFunctionId = Request.Params.GetValues(0).First();

            if (check_id != null)
            {
                ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>();
                IList<string> roles = userManager?.GetRoles(check_id);
                if (roles != null)
                    if (roles.Contains("admin"))
                        res.Admin = true;
                res.Favourited = res.Effect.Favourited(check_id);
            }



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

            List < FEAction> inp = null;
            List<FEAction> outp = null;
            FEAction.Get((int)id, inp, outp);

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

            //TODO
            //res.FormInput.listSelectedProsI = Pro.GetAllIdsFor(res.FormInput.listSelectedProsI);
            //res.FormInput.listSelectedVremI = Vrem.GetAllIdsFor(res.FormInput.listSelectedVremI);
            //res.FormInput.listSelectedSpecI = Spec.GetAllIdsFor(res.FormInput.listSelectedSpecI);

            //res.FormOutput.listSelectedProsO = Pro.GetAllIdsFor(res.FormOutput.listSelectedProsO);
            //res.FormOutput.listSelectedVremO = Vrem.GetAllIdsFor(res.FormOutput.listSelectedVremO);
            //res.FormOutput.listSelectedSpecO = Spec.GetAllIdsFor(res.FormOutput.listSelectedSpecO);




            res.Obj.LoadImage();



            return View(res);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(FEText obj, HttpPostedFileBase[] uploadImage, int[] deleteImg_, DescrSearchIInput inp = null, DescrSearchIOut outp = null)
        {

            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(404);


            DescrSearchIInput.ValidationIfNeed(inp);

            DescrSearchIOut.ValidationIfNeed(outp);
            if (inp?.Valide == false || outp?.Valide == false)
                return new HttpStatusCodeResult(404);


            var list_img_byte = Get_photo_post(uploadImage);

            List<int> deleteImg = null;
            if (deleteImg_ != null)
                deleteImg = deleteImg_.Distinct().ToList();
            //using (ApplicationDbContext db=new ApplicationDbContext())
            //{

            //TODO валидация

            FEText oldObj = FEText.Get(obj.IDFE);
            if (oldObj == null)
                return new HttpStatusCodeResult(404);

            DescrSearchI inp_ = new DescrSearchI(inp);
            inp_.DeleteNotChildCheckbox();
            DescrSearchI outp_ = new DescrSearchI(outp);
            outp_.DeleteNotChildCheckbox();

            if (!oldObj.ChangeDb(obj, deleteImg, list_img_byte, inp_, outp_))
                return new HttpStatusCodeResult(404);
            Lucene_.UpdateDocument(obj.IDFE.ToString(), obj);

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
        public ActionResult Create(FEText obj, HttpPostedFileBase[] uploadImage, DescrSearchIInput inp = null, DescrSearchIOut outp = null)
        {

            //if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            //{
            //    var pic = System.Web.HttpContext.Current.Request.Files["uploadImage[0]"];
            //}
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(404);

            if (!obj.Validation())
                return new HttpStatusCodeResult(404);


            DescrSearchIInput.ValidationIfNeed(inp);

            DescrSearchIOut.ValidationIfNeed(outp);
            if (inp?.Valide == false || outp?.Valide == false)
                return new HttpStatusCodeResult(404);

            DescrSearchI inp_ = new DescrSearchI(inp);
            inp_.DeleteNotChildCheckbox();
            DescrSearchI outp_ = new DescrSearchI(outp);
            outp_.DeleteNotChildCheckbox();

            var list_img_byte = Get_photo_post(uploadImage);


            //новая
            obj.AddToDb(inp_, outp_, list_img_byte);



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
        public ActionResult CreateDescription(string json)//, string technicalFunctionId
        {
            //type: 1-actionid ,2-fizvell,3-paramfizvell,4-pros,5-spec,6-vrem
            //TypeAction: 1-добавление ,2-редактирование, 3- удаление

            List<JsonSaveDescription> massAddActionId = new List<JsonSaveDescription>();
            List<JsonSaveDescription> massAddFizVels = new List<JsonSaveDescription>();
            List<JsonSaveDescription> massAddParamFizVels = new List<JsonSaveDescription>();
            List<JsonSaveDescription> massAddPros = new List<JsonSaveDescription>();
            List<JsonSaveDescription> massAddVrems = new List<JsonSaveDescription>();
            List<JsonSaveDescription> massAddSpecs = new List<JsonSaveDescription>();

            List<JsonSaveDescription> massEditActionId = new List<JsonSaveDescription>();
            List<JsonSaveDescription> massEditFizVels = new List<JsonSaveDescription>();
            List<JsonSaveDescription> massEditParamFizVels = new List<JsonSaveDescription>();
            List<JsonSaveDescription> massEditPros = new List<JsonSaveDescription>();
            List<JsonSaveDescription> massEditVrems = new List<JsonSaveDescription>();
            List<JsonSaveDescription> massEditSpecs = new List<JsonSaveDescription>();

            List<JsonSaveDescription> massDeletedPros = new List<JsonSaveDescription>();
            List<JsonSaveDescription> massDeletedVrems = new List<JsonSaveDescription>();
            List<JsonSaveDescription> massDeletedSpecs = new List<JsonSaveDescription>();


            bool notValide = false;


            var massData = JsonConvert.DeserializeObject<JsonSaveDescription[]>(json).ToList();

            //на всякий случай чистим, если будет попытка вмешатльства в js
            foreach (var i in massData)
                i.NewId = null;

            //TODO надо найти то к чему все будет относиться, это может вернуть null
            string currentActionId = null;// massData.FirstOrDefault(x1 => x1.Type == 1);
            bool? currentActionParametric = null;

            //TODO throw new Exception(); убрать
            //при заполнении currentActionId в этом цикле проверка на !=VOZ0 лишняя тк при VOZ0 переменная дальше перезаписывается нормальным id но пусть лучше будет
            foreach (var i in massData)
            {
                switch (i.Type)
                {
                    case 1://actionid

                        switch (i.TypeAction)
                        {
                            case 1://добавление
                                massAddActionId.Add(i);
                                break;
                            case 2://редактирование
                                massEditActionId.Add(i);
                                break;
                            case 3://удаление
                                   //.Add(i);
                                throw new Exception();
                                break;
                        }

                        break;

                    case 2://fizvell
                        if (currentActionId == null && i.ParentId != "VOZ0")
                            currentActionId = i.ParentId;
                        switch (i.TypeAction)
                        {
                            case 1://добавление
                                massAddFizVels.Add(i);
                                break;
                            case 2://редактирование
                                massEditFizVels.Add(i);
                                break;
                            case 3://удаление
                                //.Add(i);
                                throw new Exception();
                                break;
                        }

                        break;

                    case 3://paramfizvell
                           //TODO проверять является ли параметрическим  actionId и тогда уже решать добавлять или нет

                        if (currentActionId == null && !i.ParentId.Contains("VOZ0"))
                            currentActionId = i.ParentId.Split('_')[0];

                        switch (i.TypeAction)
                        {
                            //TODO проверять является ли параметрическим
                            case 1://добавление

                                massAddParamFizVels.Add(i);
                                break;
                            case 2://редактирование
                                massEditParamFizVels.Add(i);
                                break;
                            case 3://удаление
                                   //.Add(i);
                                throw new Exception();
                                break;
                        }
                        break;

                    case 4://pros
                           //TODO проверять является ли параметрическим и сравнивать с типом actionId и тогда уже решать добавлять или нет
                        if (currentActionId == null && !i.ParentId.Contains("VOZ0"))
                            currentActionId = i.ParentId.Split('_')[0];
                        //VOZ2_PROS4
                        //VOZ0_PROS_NEW4



                        switch (i.TypeAction)
                        {
                            case 1://добавление
                                massAddPros.Add(i);
                                break;
                            case 2://редактирование
                                massEditPros.Add(i);
                                break;
                            case 3://удаление
                                massDeletedPros.Add(i);
                                break;
                        }
                        break;

                    case 5://spec
                           //TODO проверять является ли параметрическим и сравнивать с типом actionId и тогда уже решать добавлять или нет
                        if (currentActionId == null && !i.ParentId.Contains("VOZ0"))
                            currentActionId = i.ParentId.Split('_')[0];

                        switch (i.TypeAction)
                        {
                            case 1://добавление
                                massAddSpecs.Add(i);
                                break;
                            case 2://редактирование
                                massEditSpecs.Add(i);
                                break;
                            case 3://удаление
                                massDeletedSpecs.Add(i);
                                break;
                        }
                        break;

                    case 6://vrem
                           //TODO проверять является ли параметрическим и сравнивать с типом actionId и тогда уже решать добавлять или нет

                        if (currentActionId == null && !i.ParentId.Contains("VOZ0"))
                            currentActionId = i.ParentId.Split('_')[0];
                        switch (i.TypeAction)
                        {
                            case 1://добавление
                                massAddVrems.Add(i);
                                break;
                            case 2://редактирование
                                massEditVrems.Add(i);
                                break;
                            case 3://удаление
                                massDeletedVrems.Add(i);
                                break;
                        }
                        break;


                }
            }
            if (massAddActionId.Count > 1)//TODO
                throw new Exception("добавлено слишком много ActionId, это не предусмотрено");

            if (notValide)//TODO
                return new HttpStatusCodeResult(404);
            using (var db = new ApplicationDbContext())
            {

                //ActionId
                int lastAllActionId = 0;
                if(massAddActionId.Count>0)
                {
                    var allAction = db.AllActions.ToList();
                    if (allAction.Count > 0)
                        lastAllActionId = allAction.Max(x1 => int.Parse(x1.Id.Split(new string[] { "VOZ" }, StringSplitOptions.RemoveEmptyEntries)[0]));
                    //.OrderBy(x1 => int.Parse(x1.Id.Split(new string[] { "VOZ" }, StringSplitOptions.RemoveEmptyEntries)[0]))
                    //.Select(x1 => int.Parse(x1.Id.Split(new string[] { "VOZ" }, StringSplitOptions.RemoveEmptyEntries)[0])).Last();
                }
                foreach (var i in massAddActionId)//возможно вытащить из цикла тк сейчас нельзя добавить больше 1
                {
                    var obj = new Models.Domain.AllAction() { Name = i.Text, Parent = "ALLACTIONS", Parametric = i.Parametric, Id = ("VOZ" + ++lastAllActionId) };
                    db.AllActions.Add(obj);
                    db.SaveChanges();
                    i.NewId = obj.Id;
                    currentActionId = obj.Id;
                    currentActionParametric = i.Parametric;
                    //break;//TODO


                //надо обновить в pros... parentid там где =="VOZ0"? 
                foreach(var i2 in massAddPros)
                    {
                        i2.ParentId = i2.ParentId.Replace("VOZ0", currentActionId);
                        i2.Id = i2.Id.Replace("VOZ0", currentActionId);
                    }
                        
                    foreach (var i2 in massAddVrems)
                    {
                        i2.ParentId = i2.ParentId.Replace("VOZ0", currentActionId);
                        i2.Id = i2.Id.Replace("VOZ0", currentActionId);
                    }
                       
                    foreach (var i2 in massAddSpecs)
                    {
                        i2.ParentId = i2.ParentId.Replace("VOZ0", currentActionId);
                        i2.Id = i2.Id.Replace("VOZ0", currentActionId);
                    }
                        

                }

                if (currentActionId != null & currentActionParametric == null)
                    currentActionParametric = db.AllActions.FirstOrDefault(x1 => x1.Id == currentActionId)?.Parametric;

                foreach (var i in massEditActionId)
                {
                    var act = db.AllActions.FirstOrDefault(x1 => x1.Id == i.Id);
                    if (act != null)
                        act.Name = i.Text;
                    else
                        i.Id = null;


                    db.SaveChanges();
                }

                //if (currentActionId == null)
                //{
                //    if (massAddFizVels.Count > 0)
                //    {
                //        currentActionId = massAddFizVels[0].ParentId;
                //        goto currentActionIdIsDefined;
                //    }


                //        massAddParamFizVels = new List<JsonSaveDescription>();
                //     massAddPros = new List<JsonSaveDescription>();
                //     massAddVrems = new List<JsonSaveDescription>();
                //     massAddSpecs = new List<JsonSaveDescription>();

                //     massEditActionId = new List<JsonSaveDescription>();
                //     massEditFizVels = new List<JsonSaveDescription>();
                //    massEditParamFizVels = new List<JsonSaveDescription>();
                //   massEditPros = new List<JsonSaveDescription>();
                //     massEditVrems = new List<JsonSaveDescription>();
                //     massEditSpecs = new List<JsonSaveDescription>();

                //     massDeletedPros = new List<JsonSaveDescription>();
                //     massDeletedVrems = new List<JsonSaveDescription>();
                //     massDeletedSpecs = new List<JsonSaveDescription>();
                //}

                ////GOTO currentActionId == null
                //currentActionIdIsDefined:


                //currentActionId need:
                //FizVels- add
                //checkbox-add


                //FizVels
                if (currentActionId != null && massAddFizVels.Count > 0 && currentActionParametric != null)
                {
                    List<FizVel> fizvels = db.FizVels.Where(x1 => x1.Id.Contains(currentActionId + "_FIZVEL")).ToList(); 
                    //if (currentActionParametric == false)
                    //    fizvels=//выберет и не параметрические
                    //else
                    //    fizvels = db.FizVels.Where(x1 => x1.Id.Contains(currentActionId + "_FIZVEL_R")).ToList();
                    int lastFizVel = 0;
                    if (fizvels.Count > 0)
                        if (currentActionParametric == false)
                        {
                            lastFizVel = fizvels.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentActionId + "_FIZVEL_") }, StringSplitOptions.RemoveEmptyEntries)[0]));
                        }
                        else
                        {
                            lastFizVel = fizvels.Max(x1 =>
                            {
                                int rs;
                                int.TryParse(x1.Id.Split(new string[] { (currentActionId + "_FIZVEL_R") }, StringSplitOptions.RemoveEmptyEntries)[0],out rs);
                                return rs;
                            });
                        }

                    foreach (var i in massAddFizVels)
                    {
                        string fizVelId = "";
                        if (currentActionParametric == false)
                            fizVelId = (currentActionId + "_FIZVEL_" + ++lastFizVel);
                        else
                            fizVelId = (currentActionId + "_FIZVEL_R" + ++lastFizVel);


                        db.FizVels.Add(new Models.Domain.FizVel()
                        {
                            Name = i.Text,
                            Parent = currentActionId + "_FIZVEL",
                            Id = fizVelId
                        });
                        db.SaveChanges();
                        foreach(var i2 in massAddParamFizVels)
                        {
                            if (i2.ParentId ==i.Id)
                                i2.ParentId = fizVelId;
                        }
                    }
                }
                foreach (var i in massEditFizVels)
                {
                    var obj = db.FizVels.FirstOrDefault(x1 => x1.Id == i.Id);
                    if (obj != null)
                        obj.Name = i.Text;
                    else
                        i.Id = null;

                    db.SaveChanges();
                }



                //ParamFizVels
                if(currentActionParametric == true)
                {


                    if (currentActionId != null && massAddParamFizVels.Count > 0)
                    {


                        string currentFizVels = massAddParamFizVels[0].ParentId;

                        var fizvels = db.FizVels.Where(x1 => x1.Id.Contains(currentFizVels + "_")).ToList();//("VOZ" + currentActionId + "_FIZVEL_R"+ currentFizVels)
                        int lastFizVel = 0;
                        if (fizvels.Count > 0)
                            lastFizVel = fizvels.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentFizVels + "_") }, StringSplitOptions.RemoveEmptyEntries)[0]));



                        foreach (var i in massAddParamFizVels)
                        {
                            db.FizVels.Add(new Models.Domain.FizVel()
                            {
                                Name = i.Text,
                                Parent = currentFizVels,
                                Id = (currentFizVels + "_" + ++lastFizVel),
                                Parametric = true
                            });
                            db.SaveChanges();
                        }
                    }
                
                    foreach (var i in massEditParamFizVels)
                    {
                        var obj = db.FizVels.FirstOrDefault(x1 => x1.Id == i.Id);
                        if (obj != null)
                            obj.Name = i.Text;
                        else
                            i.Id = null;

                        db.SaveChanges();
                    }
                }





                if (currentActionId != null && currentActionParametric == false)
                {
                    //add checkbox

                    //pro
                    {
                        int last = 0;
                        var pros = db.Pros.Where(x1 => x1.Id.Contains(currentActionId + "_PROS")).ToList();
                        if (pros.Count > 0)
                            last = pros.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentActionId + "_PROS") }, StringSplitOptions.RemoveEmptyEntries)[0]));
                        //List<JsonSaveDescription> done = new List<JsonSaveDescription>();

                        int countLastIter = 0;
                        while (massAddPros.Count > 0)
                        {

                            if (countLastIter == massAddPros.Count)
                                throw new Exception("непонятные PRO");
                            countLastIter = massAddPros.Count;
                            for (int i = 0; i < massAddPros.Count; ++i)
                            {
                                //проверить можно ли добавить, и добавить и вынести в массив
                                if (!massAddPros[i].ParentId.Contains("_NEW"))
                                {
                                    var pro = new Models.Domain.Pro()
                                    {
                                        Name = massAddPros[i].Text,
                                        Parent = (massAddPros[i].ParentId.Contains("_PROS") ? massAddPros[i].ParentId : massAddPros[i].ParentId + "_PROS"),
                                        Id = (currentActionId + "_PROS" + ++last)//VOZ1_PROS1
                                    };

                                    db.Pros.Add(pro);
                                    db.SaveChanges();
                                    for (int i2 = 0; i2 < massAddPros.Count; ++i2)
                                    {
                                        if (massAddPros[i2].ParentId == massAddPros[i].Id)
                                        {
                                            massAddPros[i2].ParentId = pro.Id;
                                        }
                                    }
                                    massAddPros.Remove(massAddPros[i--]);

                                }


                            }
                        }
                    }

                    //vrem
                    {
                        int last = 0;
                        var vrems = db.Vrems.Where(x1 => x1.Id.Contains(currentActionId + "_VREM")).ToList();
                        if (vrems.Count > 0)
                            last = vrems.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentActionId + "_VREM") }, StringSplitOptions.RemoveEmptyEntries)[0]));
                        //List<JsonSaveDescription> done = new List<JsonSaveDescription>();

                        int countLastIter = 0;
                        while (massAddVrems.Count > 0)
                        {

                            if (countLastIter == massAddVrems.Count)
                                throw new Exception("непонятные Vrem");
                            countLastIter = massAddVrems.Count;
                            for (int i = 0; i < massAddVrems.Count; ++i)
                            {
                                //проверить можно ли добавить, и добавить и вынести в массив
                                if (!massAddVrems[i].ParentId.Contains("_NEW"))
                                {
                                    var vrem = new Models.Domain.Vrem()
                                    {
                                        Name = massAddVrems[i].Text,
                                        Parent = (massAddVrems[i].ParentId.Contains("_VREM") ? massAddVrems[i].ParentId : massAddVrems[i].ParentId + "_VREM"),

                                        Id = (currentActionId + "_VREM" + ++last)
                                    };

                                    db.Vrems.Add(vrem);
                                    db.SaveChanges();
                                    for (int i2 = 0; i2 < massAddVrems.Count; ++i2)
                                    {
                                        if (massAddVrems[i2].ParentId == massAddVrems[i].Id)
                                        {
                                            massAddVrems[i2].ParentId = vrem.Id;
                                        }
                                    }
                                    massAddVrems.Remove(massAddVrems[i--]);

                                }


                            }
                        }
                    }

                    //spec
                    {
                        int last = 0;
                        var specs = db.Specs.Where(x1 => x1.Id.Contains(currentActionId + "_SPEC")).ToList();
                        if (specs.Count > 0)
                            last = specs.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentActionId + "_SPEC") }, StringSplitOptions.RemoveEmptyEntries)[0]));
                        //List<JsonSaveDescription> done = new List<JsonSaveDescription>();

                        int countLastIter = 0;
                        while (massAddSpecs.Count > 0)
                        {

                            if (countLastIter == massAddSpecs.Count)
                                throw new Exception("непонятные Spec");
                            countLastIter = massAddSpecs.Count;
                            for (int i = 0; i < massAddSpecs.Count; ++i)
                            {
                                //проверить можно ли добавить, и добавить и вынести в массив
                                if (!massAddSpecs[i].ParentId.Contains("_NEW"))
                                {
                                    var spec = new Models.Domain.Spec()
                                    {
                                        Name = massAddSpecs[i].Text,
                                        Parent = (massAddSpecs[i].ParentId.Contains("_SPEC") ? massAddSpecs[i].ParentId : massAddSpecs[i].ParentId + "_SPEC"),

                                        Id = (currentActionId + "_SPEC" + ++last)
                                    };

                                    db.Specs.Add(spec);
                                    db.SaveChanges();
                                    for (int i2 = 0; i2 < massAddSpecs.Count; ++i2)
                                    {
                                        if (massAddSpecs[i2].ParentId == massAddSpecs[i].Id)
                                        {
                                            massAddSpecs[i2].ParentId = spec.Id;
                                        }
                                    }
                                    massAddSpecs.Remove(massAddSpecs[i--]);

                                }


                            }
                        }
                    }


                    foreach (var i in massEditPros)
                    {
                        var act = db.Pros.FirstOrDefault(x1 => x1.Id == i.Id);
                        if (act != null)
                            act.Name = i.Text;
                        else
                            i.Id = null;

                        db.SaveChanges();
                    }
                    foreach (var i in massEditSpecs)
                    {
                        var act = db.Specs.FirstOrDefault(x1 => x1.Id == i.Id);
                        if (act != null)
                            act.Name = i.Text;
                        else
                            i.Id = null;

                        db.SaveChanges();
                    }
                    foreach (var i in massEditVrems)
                    {
                        var act = db.Vrems.FirstOrDefault(x1 => x1.Id == i.Id);
                        if (act != null)
                            act.Name = i.Text;
                        else
                            i.Id = null;

                        db.SaveChanges();
                    }


                    {
                        List<Pro> forDeleted = new List<Pro>();

                        int start = 0;
                        foreach (var i in massDeletedPros)
                        {
                            var pro = db.Pros.FirstOrDefault(x1 => x1.Id == i.Id);
                            if (pro == null)
                                continue;
                            forDeleted.Add(pro);
                            //var childs=Pro.GetChild(pro.Id);
                            //    forDeleted.AddRange(childs);
                            for (; start < forDeleted.Count; ++start)
                            {
                                forDeleted.AddRange(Pro.GetChild(forDeleted[start].Id));
                            }
                        }
                        db.Pros.RemoveRange(forDeleted);
                        db.SaveChanges();
                    }

                    {
                        List<Spec> forDeleted = new List<Spec>();

                        int start = 0;
                        foreach (var i in massDeletedSpecs)
                        {
                            var spec = db.Specs.FirstOrDefault(x1 => x1.Id == i.Id);
                            if (spec == null)
                                continue;
                            forDeleted.Add(spec);
                            //var childs=Pro.GetChild(pro.Id);
                            //    forDeleted.AddRange(childs);
                            for (; start < forDeleted.Count; ++start)
                            {
                                forDeleted.AddRange(Spec.GetChild(forDeleted[start].Id));
                            }
                        }
                        db.Specs.RemoveRange(forDeleted);
                        db.SaveChanges();
                    }

                    {
                        List<Vrem> forDeleted = new List<Vrem>();

                        int start = 0;
                        foreach (var i in massDeletedVrems)
                        {
                            var vrem = db.Vrems.FirstOrDefault(x1 => x1.Id == i.Id);
                            if (vrem == null)
                                continue;
                            forDeleted.Add(vrem);
                            //var childs=Pro.GetChild(pro.Id);
                            //    forDeleted.AddRange(childs);
                            for (; start < forDeleted.Count; ++start)
                            {
                                forDeleted.AddRange(Vrem.GetChild(forDeleted[start].Id));
                            }
                        }
                        db.Vrems.RemoveRange(forDeleted);
                        db.SaveChanges();
                    }

                }




                //TODO удаление всего в самом конце начиная с детей
            }
            return View();
        }




        //-------------------------------------------------------------------------------------------------------------------------------------------












    }



}