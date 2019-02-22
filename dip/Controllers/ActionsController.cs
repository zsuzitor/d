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

        public ActionResult DescriptionInput(DescrSearchIInput inp=null, DescrSearchIOut outp = null)
        {
            
                DescrSearchIInput.ValidationIfNeed(inp);
            
                DescrSearchIOut.ValidationIfNeed(outp);
            if(inp?.Valide==false|| outp?.Valide==false)
                return new HttpStatusCodeResult(404);
            DescriptionInputV res = new DescriptionInputV();
            res.InputForm = DescriptionForm.GetFormObject(inp?.actionIdI,inp?.FizVelIdI, inp?.listSelectedProsI, inp?.listSelectedSpecI, inp?.listSelectedVremI);
            res.OutpForm = DescriptionForm.GetFormObject(outp?.actionIdO, outp?.FizVelIdO, outp?.listSelectedProsO, outp?.listSelectedSpecO, outp?.listSelectedVremO);

            var inp_ = new DescrSearchI(inp);
            if(inp_.CheckParametric()==null)
                if (res.InputForm.ActionId.Count > 0)
                    inp_.Parametric=res.InputForm.ActionId[0].Parametric;
            
            var outp_ = new DescrSearchI(outp);
            if (outp_.CheckParametric() == null)
                if (res.OutpForm.ActionId.Count > 0)
                    outp_.Parametric = res.OutpForm.ActionId[0].Parametric;
            //outp_.CheckParametric();
            //if (DescrSearchI.IsNull(inp_) || DescrSearchI.IsNull(outp_))
            //{
            //    inp_ = null;
            //    outp_ = null;
            //}
            res.InputFormData = inp_;
            res.OutputFormData = outp_;
            res.SetAllParametricAction();




            return PartialView(res);
        }
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