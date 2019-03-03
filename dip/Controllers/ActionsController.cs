using dip.Models;
using dip.Models.Domain;
using dip.Models.ViewModel;
using dip.Models.ViewModel.ActionsV;
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

            //TODO мб ограничивать что бы не закинули слишком много
        public ActionResult DescriptionInput( List<DescrSearchI> inp, List<DescrSearchI> outp,int countInput = 1)
        {

            //DescrSearchIInput.ValidationIfNeed(inp);

            //DescrSearchIOut.ValidationIfNeed(outp);
            inp = inp ?? new List<DescrSearchI>() {null };
            outp = outp ?? new List<DescrSearchI>() { null };
            

            DescriptionInputV res = new DescriptionInputV();

            
            foreach(var i in inp)//TODO
            {
                DescrSearchI.Validation(i);
                if(i?.Valide==false)
                    return new HttpStatusCodeResult(404);
                var formObj = DescriptionForm.GetFormObject(i?.ActionId, i?.FizVelId, i?.ListSelectedPros, i?.ListSelectedSpec, i?.ListSelectedVrem);
                if (i?.CheckParametric() == null)
                    if (formObj.ActionId.Count > 0)
                        if(i!=null)
                        i.Parametric = formObj.ActionId[0].Parametric;
                res.InputForms.Add(new DescriptionFormWithData()
                {
                    Form = formObj,
                    FormData = i,
                    
                });

            }
            foreach (var i in outp)//TODO
            {
                DescrSearchI.Validation(i);
                if (i?.Valide==false)
                    return new HttpStatusCodeResult(404);
                var formObj = DescriptionForm.GetFormObject(i?.ActionId, i?.FizVelId, i?.ListSelectedPros, i?.ListSelectedSpec, i?.ListSelectedVrem);
                if (i?.CheckParametric() == null)
                    if (formObj.ActionId.Count > 0)
                        if (i != null)
                            i.Parametric = formObj.ActionId[0].Parametric;
                res.OutpForms.Add(new DescriptionFormWithData()
                {
                    Form = formObj,//DescriptionForm.GetFormObject(i.ActionId, i.FizVelId, i.ListSelectedPros, i.ListSelectedSpec, i.ListSelectedVrem),
                    FormData = i
                });

            }

            
            //var inp_ = new DescrSearchI(inp);
            //if(inp_.CheckParametric()==null)
            //    if (res.InputForm.ActionId.Count > 0)
            //        inp_.Parametric=res.InputForm.ActionId[0].Parametric;
            
            //var outp_ = new DescrSearchI(outp);
            //if (outp_.CheckParametric() == null)
            //    if (res.OutpForm.ActionId.Count > 0)
            //        outp_.Parametric = res.OutpForm.ActionId[0].Parametric;
            //outp_.CheckParametric();
            //if (DescrSearchI.IsNull(inp_) || DescrSearchI.IsNull(outp_))
            //{
            //    inp_ = null;
            //    outp_ = null;
            //}
            //res.InputFormData = inp_;
            //res.OutputFormData = outp_;
            res.SetAllParametricAction();

            


            return PartialView(res);
        }




        //stateId-последний выбранный ребенок состояния
        public ActionResult ObjectInput(bool changedObject=false,string stateIdBegin=null, string stateIdEnd = null, DescrObjectI objFormsBegin=null, DescrObjectI objFormsEnd = null)
        {//, DescrObjectI objFormsBegin, DescrObjectI objFormsEnd
         //12;
         //string[] CharacteristicStart = null, string[] CharacteristicEnd = null
            List<string> CharacteristicStart = new List<string>();
            if (objFormsBegin!=null)
             CharacteristicStart = objFormsBegin.GetList_();
            List<string> CharacteristicEnd = new List<string>();
            if (objFormsBegin != null)
                CharacteristicEnd = objFormsEnd.GetList_();
            //TODO если пришли пустые значения надо загружать пустую форму
            //TODO CharacteristicStart может содержать несколько конечных элементов

            ObjectInputV res = new ObjectInputV();
            res.changedObject = changedObject;

            //CharacteristicStart = CharacteristicStart == null ? new string[0] : CharacteristicStart;
            //if(changeStateObject)
            //CharacteristicEnd = CharacteristicEnd == null ? new string[0] : CharacteristicEnd;

            //CharacteristicObject
            //StateObject StateStart = null;
            //StateObject StateEnd = null;

            //List<CharacteristicObject> CharacteristicStart1 = new List<CharacteristicObject>();
            //List<CharacteristicObject> CharacteristicStart2 = new List<CharacteristicObject>();
            //List<CharacteristicObject> CharacteristicStart3 = new List<CharacteristicObject>();

            //List<CharacteristicObject> CharacteristicEnd1 = new List<CharacteristicObject>();
            //List<CharacteristicObject> CharacteristicEnd2 = new List<CharacteristicObject>();
            //List<CharacteristicObject> CharacteristicEnd3 = new List<CharacteristicObject>();


            List<StateObject> baseState=StateObject.GetBase();
            List<PhaseCharacteristicObject> basePhase= PhaseCharacteristicObject.GetBase(); 
            
                
                //db.PhaseCharacteristicObjects.Where(x1=>x1.Parent== "DESCOBJECT").ToList();
            
            res.StatesStart = baseState.Select(x1=>x1.CloneWithOutRef()).ToList();
           // if (changeStateObject)
                res.StatesEnd = baseState.Select(x1 => x1.CloneWithOutRef()).ToList();

            //
            //res.CharacteristicsStart.Phase1 = basePhase;
            //res.CharacteristicsStart.Phase2 = basePhase;
            //res.CharacteristicsStart.Phase3 = basePhase;

            //res.CharacteristicsEnd.Phase1 = basePhase;
            //res.CharacteristicsEnd.Phase2 = basePhase;
            //res.CharacteristicsEnd.Phase3 = basePhase;
            {
               
                StateObject state = StateObject.Get(stateIdBegin);
                if (state != null)
                {
                    var stateList = state.GetParentsList();
                    stateList.Add(state);
                    foreach(var i in stateList)
                        res.StateStartSelected += i.Id + " ";
                    //using (var db = new ApplicationDbContext())
                    //db.StateObjects.Where(x1=>x1.Parent== "STRUCTOBJECT").ToList();
                    foreach(var i in res.StatesStart)
                        if(i.Id== stateList[0].Id)//res.StateStart = stateList[0];
                        {
                            i.LoadPartialTree(stateList);
                            switch (i.CountPhase)//TODO в метод
                            {
                                case 1:
                                    res.CharacteristicsStart.Phase1 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList(); 
                                    break;


                                case 2:
                                    //res.CharacteristicsStart.Phase1 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList(); 
                                    res.CharacteristicsStart.Phase2 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                                    goto case 1;
                                //break;

                                case 3:
                                    //res.CharacteristicsStart.Phase1 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList(); 
                                    //res.CharacteristicsStart.Phase2 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList(); 
                                    res.CharacteristicsStart.Phase3 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                                    goto case 2;
                                    //break;

                            }
                        }
                }
                
            }
            if (changedObject)
            {
                StateObject state = StateObject.Get(stateIdEnd);

                if (state != null)
                {
                    var stateList = state.GetParentsList();
                stateList.Add(state);
                    foreach (var i in stateList)
                        res.StateEndSelected += i + " ";
                    // using (var db = new ApplicationDbContext())
                    // db.StateObjects.Where(x1 => x1.Parent == "STRUCTOBJECT").ToList();
                    foreach (var i in res.StatesEnd)
                        if (i.Id == stateList[0].Id)//res.StateStart = stateList[0];
                        {
                            i.LoadPartialTree(stateList);

                            switch (i.CountPhase)//TODO в метод
                            {
                                case 1:
                                    res.CharacteristicsEnd.Phase1 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                                    break;


                                case 2:
                                    //res.CharacteristicsEnd.Phase1 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                                    res.CharacteristicsEnd.Phase2 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                                    goto case 1;
                                    //break;

                                case 3:
                                    //res.CharacteristicsEnd.Phase1 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                                    //res.CharacteristicsEnd.Phase2 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                                    
                                    res.CharacteristicsEnd.Phase3 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                                    goto case 2;
                                    //break;

                            }
                        }
                           
                    //res.StateEnd = stateList[0];
                    //res.StateEnd.LoadPartialTree(stateList);
                }
            }





            //PhaseCharacteristicObject CharacteristicObjPhase1 = null;
            //{
            //    PhaseCharacteristicObject characteristic = CharacteristicStart.Length>0? PhaseCharacteristicObject.Get(CharacteristicStart[0]):null;
            //    if (characteristic != null)
            //    {
            //        var characteristicList = characteristic.GetParentsList();
            //        characteristicList.Add(characteristic);
            //        CharacteristicObjPhase1 = characteristicList[0];
            //        CharacteristicObjPhase1.LoadPartialTree(characteristicList);
            //    }

            //}



            //TODO че по оптимизации?

            for (var charac = 0; charac < CharacteristicStart.Count; ++charac)
            {
                //var prosIdList = CharacteristicStart[charac]?.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                List<PhaseCharacteristicObject> prosList = new List<PhaseCharacteristicObject>();
                List<List<PhaseCharacteristicObject>> treeProBase = null;
                var allids=PhaseCharacteristicObject.GetAllIdsFor(CharacteristicStart[charac]);
                if (allids == null)
                    break;
                var prosIdList = allids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (prosIdList.Length > 0)
                {
                    using (var db = new ApplicationDbContext())//TODO using in this controller
                    {
                        //var prosList = db.PhaseCharacteristicObjects.Where(x1 => x1.Parent == "DESCOBJECT").ToList();
                        
                        var allPros = db.PhaseCharacteristicObjects.Where(x1 => prosIdList.Contains(x1.Id)).ToList();
                        treeProBase = PhaseCharacteristicObject.GetQueueParent(allPros);


                        switch (charac)
                        {
                            case 0:
                                prosList = res.CharacteristicsStart.Phase1;
                                break;


                            case 1:

                                prosList = res.CharacteristicsStart.Phase2;
                                break;

                            case 2:

                                prosList = res.CharacteristicsStart.Phase3;
                                break;

                        }


                        //if(charac==0)
                        //    res.CharacteristicStart.Phase1.AddRange(prosList);
                        //else if(charac==1)
                        //    res.CharacteristicStart.Phase2.AddRange(prosList);
                        //else
                        //    res.CharacteristicStart.Phase3.AddRange(prosList);
                        // prosList = allPros.Where(x1 => x1.Parent.Split(new string[] { "PROS" }, StringSplitOptions.RemoveEmptyEntries).Length == 1).ToList();
                    }
                    foreach (var p in prosList)
                    {
                        foreach (var i in treeProBase)
                        {
                            if (p.Id == i[0].Id)
                                if (!p.LoadPartialTree(i))
                                    throw new Exception("TODO ошибка");
                        }
                    }
                }
            }


            //TODO че по оптимизации?
           
            if (changedObject)
                for (var charac = 0; charac < CharacteristicEnd.Count; ++charac)
            {
                    //var prosIdList = CharacteristicEnd[charac]?.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                    List<PhaseCharacteristicObject> prosList = new List<PhaseCharacteristicObject>();
                    List<List<PhaseCharacteristicObject>> treeProBase = null;
                    var allids = PhaseCharacteristicObject.GetAllIdsFor(CharacteristicStart[charac]);
                    if (allids == null)
                        break;
                    var prosIdList = allids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (prosIdList.Length > 0)
                    {
                        using (var db = new ApplicationDbContext())//TODO using in this controller
                {
                    //var prosList = db.PhaseCharacteristicObjects.Where(x1 => x1.Parent == "DESCOBJECT").ToList();
                    
                        var allPros = db.PhaseCharacteristicObjects.Where(x1 => prosIdList.Contains(x1.Id)).ToList();
                         treeProBase = PhaseCharacteristicObject.GetQueueParent(allPros);



                        //List<PhaseCharacteristicObject> prosList = new List<PhaseCharacteristicObject>();
                        switch (charac)
                        {
                            case 0:
                                prosList = res.CharacteristicsEnd.Phase1;
                                break;


                            case 1:

                                prosList = res.CharacteristicsEnd.Phase2;
                                break;

                            case 2:

                                prosList = res.CharacteristicsEnd.Phase3;
                                break;

                        }


                        foreach (var p in prosList)
                        {
                            foreach (var i in treeProBase)
                            {
                                if (p.Id == i[0].Id)
                                    if (!p.LoadPartialTree(i))
                                        throw new Exception("TODO ошибка");
                            }
                        }



                        //if (charac == 0)
                        //    res.CharacteristicEnd.Phase1.AddRange(prosList);
                        //else if (charac == 1)
                        //    res.CharacteristicEnd.Phase2.AddRange(prosList);
                        //else
                        //    res.CharacteristicEnd.Phase3.AddRange(prosList);
                        // prosList = allPros.Where(x1 => x1.Parent.Split(new string[] { "PROS" }, StringSplitOptions.RemoveEmptyEntries).Length == 1).ToList();
                    }
                }
            }


            

            return PartialView(res);
        }



        //public ActionResult ChangeObjectChanges()
        //{
        //    List<StateObject> res = StateObject.GetBase();


        //    return PartialView(res);
        //}







        /// <summary>
        /// загрузить для отображения(не формой а текстом)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LoadDescr(int id)
        {
            LoadDescrV res = new LoadDescrV();
           res.DictDescrData= DescriptionForm.GetFormShow(id);
            
            return PartialView(res);
        }

        //------------------------------------------------------


        //////------------------------++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        public ActionResult ChangeAction(string fizVelId, string type)
        {
            ChangeActionV res = new ChangeActionV();
            AllAction act =AllAction.Get(fizVelId);
            res.Parametric = act.Parametric;
            if (act.Parametric)
            {
                res.CheckboxParamsId = null;

                //res.ParametricFizVelsId = $"{act.Id}_FIZVEL_R1";
                using (var db=new ApplicationDbContext())//TODO using in this controller
                {
                    res.ParametricFizVelsId =db.FizVels.Where(x1=>x1.Parent==(act.Id+ "_FIZVEL")).OrderBy(fizVel => fizVel.Id).FirstOrDefault()?.Id;
                }





            }
            else
            {
                res.CheckboxParamsId = fizVelId;

                res.ParametricFizVelsId =null;
            }

            ////-----

            res.FizVelId = fizVelId;



            res.Type = type;

            return PartialView(res);
        }

        



        public ActionResult ChangeActionEdit(string fizVelId)
        {
            ChangeActionV res = new ChangeActionV();
            AllAction act = AllAction.Get(fizVelId);
            if(act==null)
                return new HttpStatusCodeResult(404);

            if (act.Parametric)
            {
                res.CheckboxParamsId = null;

                //res.ParametricFizVelsId = $"{act.Id}_FIZVEL_R1";
                using (var db = new ApplicationDbContext())
                {
                    res.ParametricFizVelsId = db.FizVels.Where(x1 => x1.Parent == (act.Id + "_FIZVEL")).OrderBy(fizVel => fizVel.Id).FirstOrDefault()?.Id;
                }

            }
            else
            {
                res.CheckboxParamsId = fizVelId;

                res.ParametricFizVelsId = null;
            }

            ////-----

            res.FizVelId = fizVelId;

            

            return PartialView(res);
        }



        /// <summary>
        /// GET-метод обновления физических величин
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns>
        [ChildActionOnly]
        public ActionResult GetFizVels(string id, string type = "") 
        {
            GetListSomethingV<FizVel> res = new GetListSomethingV<FizVel>();
            List<FizVel> listOfFizVels;
            AllAction act = AllAction.Get(id);
            using (var db = new ApplicationDbContext())
                if (!act.Parametric)
                 // непараметрическое воздействие

                    listOfFizVels = db.FizVels.Where(fizVel => (fizVel.Parent == act.Id + "_FIZVEL") ||
                                                                              (fizVel.Id == "NO_FIZVEL"))
                                                           .OrderBy(fizVel => fizVel.Id).ToList();
                else
                   
                    listOfFizVels = db.FizVels.Where(fizVel => (fizVel.Parent == act.Id + "_FIZVEL"))
                                                           .OrderBy(fizVel => fizVel.Id).ToList();
            
            res.List = listOfFizVels;
            res.CurrentActionId = act.Id;
            res.Type = type;
            res.ParentId = id;
            return PartialView(res);
        }


        [ChildActionOnly]
        public ActionResult GetFizVelsEdit(string id)
        {
            GetListSomethingV<FizVel> res = new GetListSomethingV<FizVel>();
            List<FizVel> listOfFizVels;
            AllAction act = AllAction.Get(id);
            using (var db = new ApplicationDbContext())
                if (!act.Parametric)
                    // непараметрическое воздействие

                    listOfFizVels = db.FizVels.Where(fizVel => (fizVel.Parent == act.Id + "_FIZVEL") ||
                                                                              (fizVel.Id == "NO_FIZVEL"))
                                                           .OrderBy(fizVel => fizVel.Id).ToList();
                else

                    listOfFizVels = db.FizVels.Where(fizVel => (fizVel.Parent == act.Id + "_FIZVEL"))
                                                           .OrderBy(fizVel => fizVel.Id).ToList();

            res.List = listOfFizVels;
            res.CurrentActionId = act.Id;
          
            res.ParentId = id;
            return PartialView(res);
        }



        /// <summary>
        /// GET-метод обновления пространственных характеристик
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns>
        [ChildActionOnly]
        public ActionResult GetPros(string id, string type)
        {
            GetListSomethingV<Pro> res = new GetListSomethingV<Pro>();
            List<Pro> prosList = new List<Pro>();
            AllAction allA = AllAction.Get(id);
            if (allA?.Parametric==false)
                using (var db = new ApplicationDbContext())
                    prosList = db.Pros.Where(pros => pros.Parent == id + "_PROS").ToList();
           
           
            res.List = prosList;
            res.Type = type;
            res.ParentId = id;
            return PartialView(res);
        }

        [ChildActionOnly]
        public ActionResult GetProsEdit(string id)
        {
            GetListSomethingV<Pro> res = new GetListSomethingV<Pro>();
            List<Pro> prosList = new List<Pro>();
            AllAction allA=AllAction.Get(id);
            if (allA?.Parametric == false)
                using (var db = new ApplicationDbContext())
                    prosList = db.Pros.Where(pros => pros.Parent == id + "_PROS").ToList();


            res.List = prosList;
            res.ParentId = id;

            return PartialView(res);
        }



        /// <summary>
        /// GET-метод обновления специальных характеристик
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        [ChildActionOnly]
        public ActionResult GetSpec(string id, string type)
        {
            GetListSomethingV<Spec> res = new GetListSomethingV<Spec>();
            // Получаем обновленный список специальных характеристик
            List<Spec> specList = new List<Spec>();
            AllAction allA = AllAction.Get(id);
            if (allA?.Parametric == false)
                using (var db = new ApplicationDbContext())
                    specList = db.Specs.Where(spec => spec.Parent == id + "_SPEC").ToList();
            //var listSelectedSpec = GetListSelectedItem(specList);

            // Отправляем его в представление
            res.List = specList;
            res.Type = type;
            res.ParentId = id;
            return PartialView(res);
        }

        [ChildActionOnly]
        public ActionResult GetSpecEdit(string id)
        {
            GetListSomethingV<Spec> res = new GetListSomethingV<Spec>();
            // Получаем обновленный список специальных характеристик
            List<Spec> specList = new List<Spec>();
            AllAction allA = AllAction.Get(id);
            if (allA?.Parametric == false)
                using (var db = new ApplicationDbContext())
                    specList = db.Specs.Where(spec => spec.Parent == id + "_SPEC").ToList();
            //var listSelectedSpec = GetListSelectedItem(specList);

            // Отправляем его в представление
            res.List = specList;
            res.ParentId = id;
            return PartialView(res);
        }



        /// <summary>
        /// GET-метод обновления временных характеристик
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        [ChildActionOnly]
        public ActionResult GetVrem(string id, string type)
        {
            GetListSomethingV<Vrem> res = new GetListSomethingV<Vrem>();
            // Получаем обновленный список временных характеристик
            List<Vrem> vremList = new List<Vrem>();
            AllAction allA = AllAction.Get(id);
            if (allA?.Parametric == false)
                using (var db = new ApplicationDbContext())
                    vremList = db.Vrems.Where(vrem => vrem.Parent == id + "_VREM").ToList();


            // Отправляем его в представление
            res.List = vremList;
            res.Type = type;
            res.ParentId = id;
            return PartialView(res);
        }


        [ChildActionOnly]
        public ActionResult GetVremEdit(string id)
        {
            GetListSomethingV<Vrem> res = new GetListSomethingV<Vrem>();
            // Получаем обновленный список временных характеристик
            List<Vrem> vremList = new List<Vrem>();
            AllAction allA = AllAction.Get(id);
            if (allA?.Parametric == false)
                using (var db = new ApplicationDbContext())
                    vremList = db.Vrems.Where(vrem => vrem.Parent == id + "_VREM").ToList();


            // Отправляем его в представление
            res.List = vremList;
            res.ParentId = id;
            return PartialView(res);
        }


       
        public ActionResult ChangeStateObject(string id = null, string type = "")
        {
            ChangeStateObjectV res = new ChangeStateObjectV();
            //GetStateObjectV res = new GetStateObjectV();
            //GetPhaseObjectV res = new GetPhaseObjectV();
            res.Type = type;
            //List<StateObject> res = new List<StateObject>();
            var obj = StateObject.Get(id);

            if (obj != null)
            {
                obj.ReLoadChild();
                res.States = obj.Childs;
                if (obj.CountPhase != null)
                {
                    List<PhaseCharacteristicObject> basePhase = PhaseCharacteristicObject.GetBase();
                    switch (obj.CountPhase)//TODO в метод
                    {
                        case 1:
                            res.Characteristics.Phase1 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                            break;


                        case 2:
                           
                            res.Characteristics.Phase2 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                            goto case 1;
                        

                        case 3:
                           
                            res.Characteristics.Phase3 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                            goto case 2;
                           

                    }
                }
            }
            


            return PartialView(res);
        }

        //[ChildActionOnly]
        //public ActionResult GetStateObject(string id=null, string type = "")
        //{
        //    GetStateObjectV res = new GetStateObjectV();
        //    res.Type= type;
        //    //List<StateObject> res = new List<StateObject>();
        //    var obj = StateObject.Get(id);
            
        //    if (obj != null)
        //    {
        //        obj.ReLoadChild();
        //        res.List = obj.Childs;
        //    }
        //    else if(id=="")
        //    {
        //        res.List=StateObject.GetBase();
        //    }
                


        //    return PartialView(res);
        //}

      


       
        public ActionResult GetPhaseObject(string id=null, string type = "")
        {
            GetPhaseObjectV res = new GetPhaseObjectV();
            res.Type = type;
            //List<PhaseCharacteristicObject> res = new List<PhaseCharacteristicObject>();
            var obj = PhaseCharacteristicObject.Get(id);
            
            if (obj != null)
            {
                obj.ReLoadChild();
                res.List = obj.Childs;
            }
            else if (id == "")
            {
                res.List = PhaseCharacteristicObject.GetBase();
            }



            return PartialView(res);
        }

        //////------------------------++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++



        /// <summary>
        /// GET-метод обновления параметрических физических величин
        /// </summary>
        /// <param name="id"> дескриптор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns>
        //[ChildActionOnly]
        public ActionResult GetParametricFizVels(string id, string type)//TODO GetParametricFizVelsEdit  оптимизация
        {
            GetListSomethingV<FizVel> res = new GetListSomethingV<FizVel>();
            // Получаем список физических величин для параметрических воздействий
            string[] actionId = id?.Split(new string[] {"_" }, StringSplitOptions.RemoveEmptyEntries);
            AllAction allA = null;
            if(actionId?.Length>0)
                allA=AllAction.Get(actionId[0]);
            if (allA?.Parametric == true)
                 res.List = FizVel.GetParametricFizVels(id) ;
          
            res.Type = type;
            res.ParentId = id;
            return PartialView(res);
        }

        public ActionResult GetParametricFizVelsEdit(string id)//TODO GetParametricFizVels  оптимизация
        {
            GetListSomethingV<FizVel> res = new GetListSomethingV<FizVel>();
            // Получаем список физических величин для параметрических воздействий
            string[] actionId = id?.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            AllAction allA = null;
            if (actionId.Length > 0)
                allA = AllAction.Get(actionId[0]);
            if ( allA?.Parametric == true)
                res.List = FizVel.GetParametricFizVels(id);

            
            res.ParentId = id;
            return PartialView(res);
        }





        /// <summary>
        /// GET-метод добавления на представление дополнительных значений пространственной характеристики
        /// </summary>
        /// <param name="id"> дескриптор выбранной пространственной характеристики + идентификатор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        //[ChildActionOnly]
        public ActionResult GetProsChild(string id, string type)
        {
            GetListSomethingV<Pro> res = new GetListSomethingV<Pro>();
            res.List = Pro.GetChild(id);
             res.List= res.List.Count>0?res.List : null;
            res.Type = type;
            res.ParentId = id;
            return PartialView(res);
        }


        public ActionResult GetProsChildEdit(string id)
        {
            GetListSomethingV<Pro> res = new GetListSomethingV<Pro>();
            res.List = Pro.GetChild(id);
            res.List = res.List.Count > 0 ? res.List : null;
            res.ParentId = id;

            return PartialView(res);
        }







        /// <summary>
        /// GET-метод добавления на представление дополнительных значений специальной характеристики
        /// </summary>
        /// <param name="id"> дескриптор выбранной специальной характеристики + идентификатор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        //[ChildActionOnly]
        public ActionResult GetSpecChild(string id, string type)
        {
            GetListSomethingV<Spec> res = new GetListSomethingV<Spec>();
            res.List = Spec.GetChild(id);
            res.List = res.List.Count > 0 ? res.List : null;
            res.Type = type;
            res.ParentId = id;

            return PartialView(res);
        }

        public ActionResult GetSpecChildEdit(string id)
        {
            GetListSomethingV<Spec> res = new GetListSomethingV<Spec>();
            res.List = Spec.GetChild(id);
            res.List = res.List.Count > 0 ? res.List : null;
            res.ParentId = id;

            return PartialView(res);
        }








        /// <summary>
        /// GET-метод добавления на представление дополнительных значений временной характеристики
        /// </summary>
        /// <param name="id"> дескриптор выбранной временной характеристики + идентификатор выбранного воздействия </param>
        /// <returns> результат действия ActionResult </returns> 
        //[ChildActionOnly]
        public ActionResult GetVremChild(string id, string type)
        {
            GetListSomethingV<Vrem> res = new GetListSomethingV<Vrem>();
            res.List = Vrem.GetChild(id);
            res.List = res.List.Count > 0 ? res.List : null;
            res.Type = type;
            res.ParentId = id;
            return PartialView(res);
        }

        public ActionResult GetVremChildEdit(string id)
        {
            GetListSomethingV<Vrem> res = new GetListSomethingV<Vrem>();
            res.List = Vrem.GetChild(id);
            res.List = res.List.Count > 0 ? res.List : null;
            res.ParentId = id;

            return PartialView(res);
        }






        //TODO хз что это и зачем, скорее всего не используется
        /// <summary>
        /// GET-метод удаления из представления дополнительных значений характеристики
        /// </summary>
        /// <param name="id"> дескриптор выбранной характеристики </param>
        /// <returns> результат действия ActionResult </returns>
        //[ChildActionOnly]
        public ActionResult GetEmptyChild(string id, string type)
        {
            //TODO для отладки
            throw new Exception("Используется? TODO");
            // Передаем в представление дескриптор характеристики
            ViewBag.parent = id;
            ViewBag.type = type;
            return PartialView();
        }




        //----------------------------------------------------------------PRIVATE









    }
}