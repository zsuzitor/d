﻿using dip.Models.Domain;
using dip.Models.ViewModel.HelpersV;
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

        public ActionResult DescrFormFizVels(string type, List<FizVel> fizVelId, DescrSearchI param = null)
        {
            DescrFormListDataV<FizVel> res = new DescrFormListDataV<FizVel>();
            res.List= fizVelId;
            res.Type = type;
            res.Param = param;
            
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
        

    }
}