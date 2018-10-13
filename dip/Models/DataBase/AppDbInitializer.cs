using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace dip.Models.DataBase
{
    public class AppDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // создаем две роли
            var roleAdmin = new IdentityRole { Name = "admin" };
            var roleUser = new IdentityRole { Name = "user" };
            var roleVip = new IdentityRole { Name = "vip" };

            // добавляем роли в бд
            roleManager.Create(roleAdmin);
            roleManager.Create(roleUser);
            roleManager.Create(roleVip);

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
                userManager.AddToRole(admin.Id, roleAdmin.Name);
                userManager.AddToRole(admin.Id, roleUser.Name);
                //
                userManager.AddToRole(Nadmin.Id, roleUser.Name);
            }


            //load old db 
            OldData.ReloadDataBase();


            base.Seed(context);
        }
    }
}