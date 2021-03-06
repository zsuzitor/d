﻿/*файл класса предназначенного для инициализации БД
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using dip.Models.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace dip.Models.DataBase
{
    /// <summary>
    /// Класс для инициализации бд
    /// </summary>
    public class AppDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>//DropCreateDatabaseIfModelChanges
    {

        /// <summary>
        /// Метод для инициализации
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // создаем  роли
            //var roleAdmin = new IdentityRole { Name = Roles.admin.ToString() };
            //var roleUser = new IdentityRole { Name = Roles.user.ToString() };
            //var roleVip = new IdentityRole { Name = Roles.vip.ToString() };

            foreach (RolesProject roleName in (RolesProject[])Enum.GetValues(typeof(RolesProject)))
            {
                var role = new IdentityRole { Name = roleName.ToString() };
                roleManager.Create(role);
            }


            // создаем пользователей
            //TODO ,EmailConfirmed=true  для пользователей убрать
            var admin = new ApplicationUser { Email = "admin@mail.ru", UserName = "admin@mail.ru", Name = "zsuz", Surname = "zsuzSUR", Birthday = DateTime.Now, EmailConfirmed = true };
            string password = "Admin1!";
            var result = userManager.Create(admin, password);
            //
            var Nadmin = new ApplicationUser { Email = "asa123A@mail.ru", UserName = "asa123A@mail.ru", Name = "zsuz11", Surname = "zsuzSUR111", Birthday = DateTime.Now, EmailConfirmed = true };
            string Npassword = "Admin1!";
            var resultN = userManager.Create(Nadmin, Npassword);
            var NAadmin = new ApplicationUser { Email = "asa123NA@mail.ru", UserName = "asa123NA@mail.ru", Name = "zsuz111", Surname = "zsuzSUR1111", Birthday = DateTime.Now, EmailConfirmed = true };
            string NApassword = "Admin1!";
            var resultNA = userManager.Create(NAadmin, NApassword);
            //

            // если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(admin.Id, RolesProject.admin.ToString());
                userManager.AddToRole(admin.Id, RolesProject.user.ToString());
                userManager.AddToRole(admin.Id, RolesProject.subscriber.ToString());
                //
                userManager.AddToRole(Nadmin.Id, RolesProject.user.ToString());
                //
                userManager.AddToRole(NAadmin.Id, RolesProject.NotApproveUser.ToString());
            }


            //load old db 

            OldData.ReloadDataBase();
            //строим индексы lucene
            Lucene_.BuildIndex();
            //строим индексы sql server
            DataBase.CreateAllForFullTextSearch();

            base.Seed(context);
        }
    }
}