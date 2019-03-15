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


        public ActionResult DescriptionFormAll(List<DescrSearchI> inp, List<DescrSearchI> outp, int countInput = 1, 
            bool changedObject = false, string stateIdBegin = null,
            string stateIdEnd = null, DescrObjectI objFormsBegin = null, DescrObjectI objFormsEnd = null)
        {
            DescriptionFormAllV res = new DescriptionFormAllV()
            {
                DescrInp = inp,
                DescrOutp = outp,
                DescrCountInput = countInput,
                ChangedObject=changedObject,
                ObjectStateIdBegin=stateIdBegin,
                ObjectStateIdEnd=stateIdEnd,
                ObjectFormsBegin=objFormsBegin,
                ObjectFormsEnd=objFormsEnd
            };

            return PartialView(res);
        }





            //TODO мб ограничивать что бы не закинули слишком много
        public ActionResult DescriptionInput( List<DescrSearchI> inp, List<DescrSearchI> outp,int countInput = 1)
        {

            //DescrSearchIInput.ValidationIfNeed(inp);

            //DescrSearchIOut.ValidationIfNeed(outp);
           // bool withOutDataInput = false;

            inp = inp ?? new List<DescrSearchI>();
            //if (inp.Count == 0)
            //    withOutDataInput = true;// countInput = 2;
            for (; inp.Count< countInput;) {
                inp.Add(null);
            }
            if(outp==null|| outp.Count==0)
            outp = new List<DescrSearchI>() { null };// outp ??


            DescriptionInputV res = new DescriptionInputV() { CountInput= countInput };

            
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
            if(res.InputForms.Count<2)//if (withOutDataInput)
                res.InputForms.Add(res.InputForms[0]);


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

            
           
            res.SetAllParametricAction();

            


            return PartialView(res);
        }




        //stateId-последний выбранный ребенок состояния
        public ActionResult ObjectInput(bool changedObject=false,string stateIdBegin=null, 
            string stateIdEnd = null, DescrObjectI objFormsBegin=null, DescrObjectI objFormsEnd = null)
        {//, DescrObjectI objFormsBegin, DescrObjectI objFormsEnd
         //12;
         //string[] CharacteristicStart = null, string[] CharacteristicEnd = null
            List<string> CharacteristicStart = new List<string>();
            if (objFormsBegin!=null)
             CharacteristicStart = objFormsBegin.GetList_();
            List<string> CharacteristicEnd = new List<string>();
            if (objFormsEnd != null)
                CharacteristicEnd = objFormsEnd.GetList_();
            //TODO если пришли пустые значения надо загружать пустую форму
            //TODO CharacteristicStart может содержать несколько конечных элементов

            ObjectInputV res = new ObjectInputV();
            res.changedObject = changedObject;

        

            List<StateObject> baseState=StateObject.GetBase();
            List<PhaseCharacteristicObject> basePhase= PhaseCharacteristicObject.GetBase(); 
            
                
                //db.PhaseCharacteristicObjects.Where(x1=>x1.Parent== "DESCOBJECT").ToList();
            
            res.StatesBegin = baseState.Select(x1=>x1.CloneWithOutRef()).ToList();
           // if (changeStateObject)
                res.StatesEnd = baseState.Select(x1 => x1.CloneWithOutRef()).ToList();

            {
               
                StateObject state = StateObject.Get(stateIdBegin);
                if (state != null)
                {
                    //int countPhase;
                    var massparent=state.GetParentsList();
                    foreach (var asd in res.StatesBegin)
                    {
                        if(massparent.FirstOrDefault(x1=>x1.Id== asd.Id)==null)
                        {
                            if (asd.Id == state.Id)
                            {
                                asd.LoadPartialTree(massparent);
                            }
                            else
                                continue;
                        }
                        asd.LoadPartialTree(massparent);
                    }
                    res.StateBeginSelected = string.Join(" ",massparent.Select(x1 => x1.Id).ToList());
                    res.StateBeginSelected +=" "+ state.Id;
                    //res.StateBeginSelected=state.LoadPartialTree(res.StatesBegin);//, out countPhase
                    res.CharacteristicsStart.SetFirstLvlStates(state.CountPhase, basePhase);
                   
                }
                
            }
            if (changedObject)
            {
                StateObject state = StateObject.Get(stateIdEnd);

                if (state != null)
                {
                    //int countPhase;
                    //res.StateEndSelected = state.LoadPartialTree(res.StatesEnd);//, out countPhase
                    //res.CharacteristicsEnd.SetFirstLvlStates(state.CountPhase, basePhase);

                    var massparent = state.GetParentsList();
                    foreach (var asd in res.StatesEnd)
                    {
                        asd.LoadPartialTree(massparent);
                    }
                    res.StateEndSelected = string.Join(" ", massparent.Select(x1 => x1.Id).ToList());
                    res.StateEndSelected += " " + state.Id;
                    //res.StateBeginSelected=state.LoadPartialTree(res.StatesBegin);//, out countPhase
                    res.CharacteristicsEnd.SetFirstLvlStates(state.CountPhase, basePhase);


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
            // CharacteristicObject
            {
                List<string> CharacteristicsStartNeedSelect = new List<string>();
                res.CharacteristicsStart.LoadTreePhasesForChilds(CharacteristicStart, CharacteristicsStartNeedSelect);
                res.CharacteristicsStart.ParamPhase1 = CharacteristicsStartNeedSelect.Count>0? CharacteristicsStartNeedSelect[0]:null;
                res.CharacteristicsStart.ParamPhase2 = CharacteristicsStartNeedSelect.Count > 1 ? CharacteristicsStartNeedSelect[1] : null;
                res.CharacteristicsStart.ParamPhase3 = CharacteristicsStartNeedSelect.Count > 2 ? CharacteristicsStartNeedSelect[2]:null;
            }
            


            //TODO че по оптимизации?

            if (changedObject)
            {
                List<string> CharacteristicsStartNeedSelect = new List<string>();
                res.CharacteristicsEnd.LoadTreePhasesForChilds(CharacteristicEnd, CharacteristicsStartNeedSelect);
                res.CharacteristicsEnd.ParamPhase1 = CharacteristicsStartNeedSelect.Count > 0 ? CharacteristicsStartNeedSelect[0]:null;
                res.CharacteristicsEnd.ParamPhase2 = CharacteristicsStartNeedSelect.Count > 1 ? CharacteristicsStartNeedSelect[1] : null;
                res.CharacteristicsEnd.ParamPhase3 = CharacteristicsStartNeedSelect.Count > 2 ? CharacteristicsStartNeedSelect[2] : null;
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
                    res.Characteristics.SetFirstLvlStates(obj.CountPhase, basePhase);
                    
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