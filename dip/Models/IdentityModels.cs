using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using dip.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace dip.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime Dateregistration { get; set; }
        //TODO раскомментить
        public bool CloseProfile { get; set; }

        //1--many
        public ICollection<Log> UserLogs { get; set; }




        public ApplicationUser() : base()
        {
            UserLogs = new List<Log>();
            Name = null;
            Surname = null;
            Birthday = null;
                Dateregistration = DateTime.Now;
            CloseProfile = false;

        }





        public static string GetUserId()
        {
            return System.Web.HttpContext.Current.User.Identity.GetUserId();
        }

        public static ApplicationUser GetUser(string id)
        {
            //string check_id = ApplicationUser.GetUserId();
            ApplicationUser res = null;
            //if (string.IsNullOrWhiteSpace(id))
            //    return res;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                res = ApplicationUser.GetUser(id, db);
            }

            return res;
        }
        public static ApplicationUser GetUser(string id, ApplicationDbContext db)
        {
            //string check_id = ApplicationUser.GetUserId();
            ApplicationUser res = null;
            if (string.IsNullOrWhiteSpace(id))
                return res;
            res = db.Users.FirstOrDefault(x1 => x1.Id == id);

            return res;
        }









        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //---------------------------old part-----------------------------------------------------
       // public DbSet<Domain.ActionPro> ActionPros { get; set; }
        public DbSet<Domain.Action> Actions { get; set; }
        //public DbSet<ActionSpec> ActionSpecs { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        //public DbSet<ActionVrem> ActionVrems { get; set; }
        public DbSet<AllAction> AllActions { get; set; }
        //public DbSet<Chain> Chains { get; set; }
        public DbSet<FEAction> FEActions { get; set; }
        public DbSet<FEIndex> FEIndexs { get; set; }
        public DbSet<FEObject> FEObjects { get; set; }
        public DbSet<FEText> FEText { get; set; }
        public DbSet<FizVel> FizVels { get; set; }
        public DbSet<NewFEIndex> NewFEIndexs { get; set; }
        public DbSet<NeZakon> NeZakons { get; set; }
        public DbSet<Pro> Pros { get; set; }
        //public DbSet<ReverseChain> ReverseChains { get; set; }
        public DbSet<Spec> Specs { get; set; }
        //public DbSet<TasksToSynthesy> TasksToSynthesys { get; set; }
        public DbSet<The> Thes { get; set; }
        public DbSet<ThesChild> ThesChilds { get; set; }
        public DbSet<Vrem> Vrems { get; set; }

        //--------------------------------------------------------------

        public DbSet<TechnicalFunctions.Index> Indexs { get; set; }
        public DbSet<TechnicalFunctions.Limit> Limits { get; set; }
        public DbSet<TechnicalFunctions.Operand> Operands { get; set; }
        public DbSet<TechnicalFunctions.OperandGroup> OperandGroups { get; set; }
        public DbSet<TechnicalFunctions.Operation> Operations { get; set; }

        //--------------------------------------------------------------
        public DbSet<Log> Logs{ get; set; }
        public DbSet<LogParam> LogParams { get; set; }
        public DbSet<Image> Images { get; set; }


        //----------------------------TEST--
        //public DbSet<test> tests { get; set; }
        


        //--------------------------------------------------------------
        //public DbSet<Team> Teams { get; set; }



        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Action>().HasMany(c => c.Pros)//1 класс и свойство который связываем
                .WithMany(s => s.Actions)//2 класс и свойство с которым связываем
                .Map(t => t.MapLeftKey("ActionId")//id 1 которое в таблице будет
                .MapRightKey("ProId")//id 2
                .ToTable("ActionPros"));//название таблицы



            modelBuilder.Entity<Domain.Action>().HasMany(c => c.Specs)//1 класс и свойство который связываем
               .WithMany(s => s.Actions)//2 класс и свойство с которым связываем
               .Map(t => t.MapLeftKey("ActionId")//id 1 которое в таблице будет
               .MapRightKey("SpecId")//id 2
               .ToTable("ActionSpec"));//название таблицы


            modelBuilder.Entity<Domain.Action>().HasMany(c => c.Vrems)//1 класс и свойство который связываем
               .WithMany(s => s.Actions)//2 класс и свойство с которым связываем
               .Map(t => t.MapLeftKey("ActionId")//id 1 которое в таблице будет
               .MapRightKey("VremId")//id 2
               .ToTable("ActionVrem"));//название таблицы




            base.OnModelCreating(modelBuilder);//инициализация что бы роли и все остальное добавилось нормально
        }







    }
}