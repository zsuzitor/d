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
    public class AppDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
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
            // добавляем роли в бд
            //roleManager.Create(roleAdmin);
            //roleManager.Create(roleUser);
            //roleManager.Create(roleVip);

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
                userManager.AddToRole(admin.Id, RolesProject.admin.ToString());
                userManager.AddToRole(admin.Id, RolesProject.user.ToString());
                userManager.AddToRole(admin.Id, RolesProject.vip.ToString());
                //
                userManager.AddToRole(Nadmin.Id, RolesProject.user.ToString());
            }


            //load old db 
            
            OldData.ReloadDataBase();
            //строим индексы lucene
            Lucene_.BuildIndex();
            //строим индексы sql server
            FullTextSearch_.Create();

            base.Seed(context);
        }
    }
}