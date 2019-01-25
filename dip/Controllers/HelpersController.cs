using dip.Models.Domain;
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
            ViewBag.form = a;
            ViewBag.type = type;
            ViewBag.param = param;
            return PartialView();
        }

        public ActionResult DescrFormFizVels(string type, List<FizVel> fizVelId, DescrSearchI param = null)
        {
            ViewBag.fizVelId = fizVelId;
            ViewBag.type = type;
            ViewBag.param = param;
            return PartialView();
        }


        public ActionResult DescrFormParamFizVels(string type, List<FizVel> parametricFizVelId, DescrSearchI param = null)
        {
            ViewBag.parametricFizVelId = parametricFizVelId;
            ViewBag.type = type;
            ViewBag.param = param;
            return PartialView();
        }

        public ActionResult DescrFormVrem(string type, List<Vrem> vrems, DescrSearchI param = null)
        {
            ViewBag.vrems = vrems;
            ViewBag.type = type;
            ViewBag.param = param;
            return PartialView();
        }

        
             public ActionResult DescrFormSpec(string type, List<Spec> specs, DescrSearchI param = null)
        {
            ViewBag.specs = specs;
            ViewBag.type = type;
            ViewBag.param = param;
            return PartialView();
        }

        public ActionResult DescrFormPros(string type, List<Pro> pros, DescrSearchI param = null)
        {
            ViewBag.pros = pros;
            ViewBag.type = type;
            ViewBag.param = param;
            return PartialView();
        }

        // не помню зачем это нужно и нужно ли вообще
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
            ViewBag.a = a;
            
            return PartialView();
        }
        public ActionResult Image(Image a, bool show_empty_img)
        {
            ViewBag.a = a;
            ViewBag.show_empty_img = show_empty_img;

            return PartialView();
        }
        

    }
}