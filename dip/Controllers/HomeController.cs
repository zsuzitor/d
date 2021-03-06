﻿/*файл контроллера, отвечающий за отрисовку общих частичных представлений
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/


using dip.Models.DataBase;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;

//
using System.Configuration;
using System.Data.SqlClient;
using dip.Models.Domain;
using dip.Models;
using dip.Models.ViewModel.HomeV;
using System.Reflection;
using System.Data;
using WpfMath;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace dip.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// возаращает header веб-приложения
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult MainHeader()
        {

            MainHeaderV res = new MainHeaderV();
            res.SearchList = new List<string>() { "lucene", "fullTextSearchF", "fullTextSearchCf", "fullTextSearchCl" };
            string id=ApplicationUser.GetUserId();
            if (id != null)
            {
                IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(ApplicationUser.GetUserId());
                if (roles.Contains(RolesProject.admin.ToString()))
                {
                    res.Admin = true;
                }
            }
            


            return PartialView(res);
        }

        /// <summary>
        /// возаращает footer веб-приложения
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult MainFooter()
        {

            return PartialView();
        }


    }
}