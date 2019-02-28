using dip.Models.Domain;
using dip.Models.ViewModel.HelpersV;
using dip.Models.ViewModel.PhysicV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Controllers
{

    //контроллер заменяющий хелперы тк они ломаются
    [ChildActionOnly]
    public class HelpersController : Controller
    {
        // GET: Helpers
        public ActionResult Index()
        {
            return PartialView();
        }

        //отрисовывает форму(a) для дескрипторного поиска, форма уже заполнена и выбраны нужные параметры(param)
        public ActionResult PartFormDescrSearch(dip.Models.ViewModel.DescriptionForm a, string type = "", DescrSearchI param = null)
        {
            PartFormDescrSearchV res = new PartFormDescrSearchV();
            res.Form = a;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }


        //вся часть формы
        public ActionResult PartFormStateObject(List<StateObject> a, string type = "", string param = null)
        {
            //PartFormStateObjectV res = new PartFormStateObjectV();
            FormStateObjectV res = new FormStateObjectV();
            res.States  = a;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }

        //только список
        public ActionResult ListStateObject(List<StateObject> a, string type = "", string param = null)
        {
            FormStateObjectV res = new FormStateObjectV();
            res.States = a;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }


        public ActionResult PartFormCharacteristicObject(CharacteristicObject a, string type = "")//, string param = null
        {
            PartFormCharacteristicObjectV res = new PartFormCharacteristicObjectV();
            res.Characteristic = a;
            res.Type = type;
            //res.Param = param;
            return PartialView(res);
        }




        public ActionResult ListPhaseObject(List<PhaseCharacteristicObject> a, string type = "",string param=null)
        {
            ListPhaseObjectV res = new ListPhaseObjectV();
            res.Phases = a;
            res.Type = type;
            //res.Param = param;
            return PartialView(res);
        }

        //public ActionResult PartFormPhaseObject(PhaseCharacteristicObject a, string type = "", string param = null)
        //{
        //    PartFormPhaseObjectV res = new PartFormPhaseObjectV();
        //    res.Phases = a;
        //    res.Type = type;
        //    //res.Param = param;
        //    return PartialView(res);
        //}








        public ActionResult DescrFormFizVels(string type, List<FizVel> fizVelId, DescrSearchI param = null)
        {
            DescrFormListDataV<FizVel> res = new DescrFormListDataV<FizVel>();
            res.List= fizVelId;
            res.Type = type;
            res.Param = param;
            
            return PartialView(res);
        }
        public ActionResult DescrFormFizVelsEdit(List<FizVel> fizVelId)
        {
            DescrFormListDataV<FizVel> res = new DescrFormListDataV<FizVel>();
            res.List = fizVelId;

            return PartialView(res);
        }


        public ActionResult DescrFormParamFizVels(string type, List<FizVel> parametricFizVelId, DescrSearchI param = null)
        {
            DescrFormListDataV<FizVel> res = new DescrFormListDataV<FizVel>();
            res.List = parametricFizVelId;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }
        public ActionResult DescrFormParamFizVelsEdit( List<FizVel> parametricFizVelId)
        {
            DescrFormListDataV<FizVel> res = new DescrFormListDataV<FizVel>();
            res.List = parametricFizVelId;
            return PartialView(res);
        }

        public ActionResult DescrFormVrem(string type, List<Vrem> vrems, DescrSearchI param = null)
        {
            DescrFormListDataV<Vrem> res = new DescrFormListDataV<Vrem>();
            res.List = vrems;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }

        
             public ActionResult DescrFormSpec(string type, List<Spec> specs, DescrSearchI param = null)
        {
            DescrFormListDataV<Spec> res = new DescrFormListDataV<Spec>();
            res.List = specs;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }

        public ActionResult DescrFormPros(string type, List<Pro> pros, DescrSearchI param = null)
        {
            DescrFormListDataV<Pro> res = new DescrFormListDataV<Pro>();
            res.List = pros;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }

        public ActionResult DescrFormProsEdit(List<Pro> pros,string parentId)
        {
            DescrFormListDataV<Pro> res = new DescrFormListDataV<Pro>();
            res.List = pros;
            res.ParentId = parentId;


            return PartialView(res);
        }

        public ActionResult DescrFormSpecEdit(List<Spec> specs, string parentId)
        {
            DescrFormListDataV<Spec> res = new DescrFormListDataV<Spec>();
            res.List = specs;
            res.ParentId = parentId;

            return PartialView(res);
        }

        public ActionResult DescrFormVremEdit(List<Vrem> vrems, string parentId)
        {
            DescrFormListDataV<Vrem> res = new DescrFormListDataV<Vrem>();
            res.List = vrems;
            res.ParentId = parentId;

            return PartialView(res);
        }



        //TODO не помню зачем это нужно и нужно ли вообще
        public ActionResult selectList(dynamic a, string label, string id, string name)
        {
            ViewBag.a = a;
            ViewBag.label = label;
            ViewBag.id = id;
            ViewBag.name = name;
            return PartialView();
        }
        public ActionResult ImageLink(Image a )
        {
            ImageV res = new ImageV();
            res.Image = a;
           
            
            return PartialView(res);
        }
        public ActionResult Image(Image a, bool show_empty_img)
        {
            ImageV res = new ImageV();
            res.Image = a;
            res.Show_empty_img = show_empty_img;

            return PartialView(res);
        }
        public ActionResult Favourite(bool? favourite)
        {
            return PartialView(favourite);
        }



        public ActionResult ListFeText(ListFeTextV model)
        {
            


            return PartialView(model);
        }


    }
}