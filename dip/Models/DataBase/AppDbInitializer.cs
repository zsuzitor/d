using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace dip.Models.DataBase
{
    public class AppDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // создаем две роли
            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "user" };

            // добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);

            // создаем пользователей

            var admin = new ApplicationUser { Email = "admin@mail.ru", UserName = "admin@mail.ru", Name = "zsuz", Surname = "zsuzSUR", Birthday = DateTime.Now };
            string password = "Admin1!";
            var result = userManager.Create(admin, password);
            //
            var Nadmin = new ApplicationUser { Email = "asa123A@mail.ru", UserName = "asa123A@mail.ru", Name = "zsuz11", Surname = "zsuzSUR111", Birthday = DateTime.Now };
            string Npassword = "Admin1!";
            var resultN = userManager.Create(Nadmin, Npassword);
            //
            
            // если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
                //
                userManager.AddToRole(Nadmin.Id, role2.Name);
            }
            
            base.Seed(context);
        }
    }
}