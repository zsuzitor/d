using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using dip.Models.Domain;
using System.Collections.Generic;
using System.Linq;
using dip.Models.CustomException;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

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
        public List<Log> UserLogs { get; set; }


        public List<FEText> FavouritedPhysics { get; set; }


        public List<FEText> Physics { get; set; }
        public List<ListPhysics> ListPhysics { get; set; }



        public ApplicationUser() : base()
        {
            UserLogs = new List<Log>();
            Name = null;
            Surname = null;
            Birthday = null;
            Dateregistration = DateTime.Now;
            CloseProfile = false;

            FavouritedPhysics = new List<FEText>();

            Physics = new List<FEText>();
            ListPhysics = new List<ListPhysics>();

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

        public static List<ApplicationUser> GetAllUsers(ApplicationDbContext db_ = null)
        {
            //string check_id = ApplicationUser.GetUserId();
            var db = db_ ?? new ApplicationDbContext();
            List<ApplicationUser> res = db.Users.ToList();

            if (db_ == null)
                db.Dispose();
            return res;
        }

        public void LoadFavouritedList()
        {

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Set<ApplicationUser>().Attach(this);
                if (!db.Entry(this).Collection(x1 => x1.FavouritedPhysics).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.FavouritedPhysics).Load();
            }
        }


        public void LoadListPhysics(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();

            db.Set<ApplicationUser>().Attach(this);
            if (!db.Entry(this).Collection(x1 => x1.ListPhysics).IsLoaded)
                db.Entry(this).Collection(x1 => x1.ListPhysics).Load();
            if (db_ == null)
                db.Dispose();
        }

        /// <summary>
        /// загрузить разрешенные ФЭ
        /// </summary>
        /// <param name="db_"></param>
        public void LoadPhysics(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();

            db.Set<ApplicationUser>().Attach(this);
            if (!db.Entry(this).Collection(x1 => x1.Physics).IsLoaded)
                db.Entry(this).Collection(x1 => x1.Physics).Load();
            if (db_ == null)
                db.Dispose();
        }

        //genered exception: NotFoundException
        public static ListPhysics AddList(string iduser, int idlist, out bool? hadList)
        {
            var user = ApplicationUser.GetUser(iduser);
            hadList = null;
            //try
            //{
            return user.AddList(idlist, out hadList);

            //}
            //catch (NotFoundException e)
            //{

            //}
        }
        //genered exception: NotFoundException
        public ListPhysics AddList(int idlist, out bool? hadList)
        {
            ListPhysics list = null;
            hadList = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Set<ApplicationUser>().Attach(this);
                if (!db.Entry(this).Collection(x1 => x1.ListPhysics).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.ListPhysics).Load();
                list = Domain.ListPhysics.Get(idlist, db);
                if (list == null)
                    return list;
                //throw new NotFoundException("List Not Founded");
                db.Set<ListPhysics>().Attach(list);

                if (this.ListPhysics.FirstOrDefault(x1 => x1.Id == list.Id) == null)
                {
                    //return;
                    this.ListPhysics.Add(list);
                    db.SaveChanges();
                    hadList = false;
                }
                else
                    hadList = true;

                //
                list.LoadPhysics(db);

                if (!db.Entry(this).Collection(x1 => x1.Physics).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.Physics).Load();
                foreach (var i in list.Physics)

                    if (this.Physics.FirstOrDefault(x1 => x1.IDFE == i.IDFE) == null)
                        this.Physics.Add(i);
                db.SaveChanges();


            }
            //hadList = hadList ?? true;
            return list;
        }

        public static void RemoveList(string iduser, int idlist)
        {
            var user = ApplicationUser.GetUser(iduser);
            user.RemoveList(idlist);
        }
        public void RemoveList(int idlist)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Set<ApplicationUser>().Attach(this);
                if (!db.Entry(this).Collection(x1 => x1.ListPhysics).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.ListPhysics).Load();
                ListPhysics list = this.ListPhysics.FirstOrDefault(x1 => x1.Id == idlist);
                if (list == null)
                    return;

                this.ListPhysics.Remove(list);
                db.SaveChanges();
                //
                list.LoadPhysics(db);

                if (!db.Entry(this).Collection(x1 => x1.Physics).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.Physics).Load();
                foreach (var i in list.Physics)
                {
                    var ph = this.Physics.FirstOrDefault(x1 => x1.IDFE == i.IDFE);
                    if (ph != null)
                        this.Physics.Remove(ph);
                }

                db.SaveChanges();


            }
        }



        public static FEText AddPhysics(string iduser, int idphys, out bool? hadPhys)
        {
            hadPhys = null;
            var user = ApplicationUser.GetUser(iduser);
            return user.AddPhysics(idphys, out hadPhys);
        }

        public FEText AddPhysics(int idphys, out bool? hadPhys)
        {
            hadPhys = null;
            FEText phys = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Set<ApplicationUser>().Attach(this);

                if (!db.Entry(this).Collection(x1 => x1.Physics).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.Physics).Load();

                phys = this.Physics.FirstOrDefault(x1 => x1.IDFE == idphys);
                if (phys == null)
                {
                    //phys = FEText.Get(idphys);
                    //db.Set<FEText>().Attach(phys);
                    phys = db.FEText.FirstOrDefault(x1 => x1.IDFE == idphys);
                    this.Physics.Add(phys);
                    hadPhys = false;
                }
                else
                    hadPhys = true;

                db.SaveChanges();


            }
            return phys;
        }

        public static void RemovePhysics(string iduser, int idphys)
        {
            var user = ApplicationUser.GetUser(iduser);
            user.RemovePhysics(idphys);
        }

        public void RemovePhysics(int idphys)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Set<ApplicationUser>().Attach(this);

                if (!db.Entry(this).Collection(x1 => x1.Physics).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.Physics).Load();

                var phys = this.Physics.FirstOrDefault(x1 => x1.IDFE == idphys);
                if (phys == null)
                    return;
                this.Physics.Remove(phys);
                db.SaveChanges();

            }
        }


        public List<int> CheckAccessPhys(List<int> idphys, HttpContextBase HttpContext)
        {
            List<int> res = new List<int>();
            if (HttpContext == null)
                return res;

            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(this.Id);

            if (roles.Contains(RolesProject.admin.ToString()))
                return idphys ?? res;
            if (roles.Contains(RolesProject.subscriber.ToString()))
                return idphys ?? res;

            if (roles.Contains(RolesProject.user.ToString()))
                if (idphys != null && idphys.Count > 0)
                {
                    List<int> tmpRes;
                        using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        db.Set<ApplicationUser>().Attach(this);
                         tmpRes = db.Entry(this).Collection(x1 => x1.Physics).Query().Where(x1 => idphys.Contains(x1.IDFE)).Select(x1 => x1.IDFE).ToList();

                    }
                        foreach(var i in idphys)
                    {
                        if (tmpRes.Contains(i))
                            res.Add(i);
                    }
                }
                    
            //if (roles.Contains(RolesProject.NotApproveUser.ToString()))
            //    return res;
            return res;
        }



        public FEText GetNextAccessPhysic(int id, HttpContextBase HttpContext)
        {
            FEText res = null;
            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(this.Id);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //DbSet<FEText> collect = null;
                if (roles.Contains(RolesProject.admin.ToString()) || roles.Contains(RolesProject.subscriber.ToString()))
                {
                    //collect = db.FEText;
                    res = db.FEText.FirstOrDefault(x1 => x1.IDFE > id);
                    if (res == null)
                        res = db.FEText.FirstOrDefault();
                }

                else if (roles.Contains(RolesProject.user.ToString()))
                {
                    db.Set<ApplicationUser>().Attach(this);
                    db.Entry(this).Collection(x1 => x1.Physics).Query().FirstOrDefault(x1 => x1.IDFE > id);
                    if (res == null)
                        res = db.Entry(this).Collection(x1 => x1.Physics).Query().FirstOrDefault();
                    //collect = user.Physics;
                }

            }

            return res;

        }


        public FEText GetPrevAccessPhysic(int id, HttpContextBase HttpContext)
        {
            FEText res = null;
            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(this.Id);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //DbSet<FEText> collect = null;
                if (roles.Contains(RolesProject.admin.ToString()) || roles.Contains(RolesProject.subscriber.ToString()))
                {
                    
                    res = db.FEText.OrderByDescending(x1 => x1.IDFE).FirstOrDefault(x1 => x1.IDFE < id);
                    if (res == null)
                        res = db.FEText.OrderByDescending(x1 => x1.IDFE).FirstOrDefault();
                }

                else if (roles.Contains(RolesProject.user.ToString()))
                {
                    db.Set<ApplicationUser>().Attach(this);
                    db.Entry(this).Collection(x1 => x1.Physics).Query().OrderByDescending(x1 => x1.IDFE).FirstOrDefault(x1 => x1.IDFE < id);
                    if (res == null)
                        res = db.Entry(this).Collection(x1 => x1.Physics).Query().OrderByDescending(x1 => x1.IDFE).FirstOrDefault();
                    //collect = user.Physics;
                }

            }

            return res;

        }


        public FEText GetFirstAccessPhysic(HttpContextBase HttpContext)
        {
            FEText res = null;
            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(this.Id);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //DbSet<FEText> collect = null;
                if (roles.Contains(RolesProject.admin.ToString()) || roles.Contains(RolesProject.subscriber.ToString()))
                {

                    res = db.FEText.FirstOrDefault();
                    
                }

                else if (roles.Contains(RolesProject.user.ToString()))
                {
                    db.Set<ApplicationUser>().Attach(this);
                    db.Entry(this).Collection(x1 => x1.Physics).Query().FirstOrDefault();
                    
                }

            }

            return res;

        }


        public FEText GetLastAccessPhysic(HttpContextBase HttpContext)
        {
            FEText res = null;
            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(this.Id);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //DbSet<FEText> collect = null;
                if (roles.Contains(RolesProject.admin.ToString()) || roles.Contains(RolesProject.subscriber.ToString()))
                {

                    res = db.FEText.OrderByDescending(x1 => x1.IDFE).FirstOrDefault();

                }

                else if (roles.Contains(RolesProject.user.ToString()))
                {
                    db.Set<ApplicationUser>().Attach(this);
                    db.Entry(this).Collection(x1 => x1.Physics).Query().OrderByDescending(x1 => x1.IDFE).FirstOrDefault();

                }

            }

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

        //public DbSet<TechnicalFunctions.Index> Indexs { get; set; }
        //public DbSet<TechnicalFunctions.Limit> Limits { get; set; }
        //public DbSet<TechnicalFunctions.Operand> Operands { get; set; }
        //public DbSet<TechnicalFunctions.OperandGroup> OperandGroups { get; set; }
        //public DbSet<TechnicalFunctions.Operation> Operations { get; set; }

        //--------------------------------------------------------------
        public DbSet<Log> Logs{ get; set; }
        public DbSet<LogParam> LogParams { get; set; }
        public DbSet<Image> Images { get; set; }

        //
        public DbSet<StateObject> StateObjects { get; set; }
       // public DbSet<CharacteristicObject> CharacteristicObjects { get; set; }
        public DbSet<PhaseCharacteristicObject> PhaseCharacteristicObjects { get; set; }
        public DbSet<ListPhysics> ListPhysics { get; set; }
        
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


            modelBuilder.Entity<ApplicationUser>().HasMany(c => c.FavouritedPhysics)//1 класс и свойство который связываем
              .WithMany(s => s.FavouritedUser)//2 класс и свойство с которым связываем
              .Map(t => t.MapLeftKey("ApplicationUserId")//id 1 которое в таблице будет
              .MapRightKey("FETextId")//id 2
              .ToTable("ApplicationUserFETextFavourite"));//название таблицы

            modelBuilder.Entity<ApplicationUser>().HasMany(c => c.ListPhysics)//1 класс и свойство который связываем
              .WithMany(s => s.Users)//2 класс и свойство с которым связываем
              .Map(t => t.MapLeftKey("ApplicationUserId")//id 1 которое в таблице будет
              .MapRightKey("ListPhysicsId")//id 2
              .ToTable("ApplicationUserListPhysics"));//название таблицы

            modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Physics)//1 класс и свойство который связываем
              .WithMany(s => s.Users)//2 класс и свойство с которым связываем
              .Map(t => t.MapLeftKey("ApplicationUserId")//id 1 которое в таблице будет
              .MapRightKey("FETextId")//id 2
              .ToTable("ApplicationUserFEText"));//название таблицы


            modelBuilder.Entity<ListPhysics>().HasMany(c => c.Physics)//1 класс и свойство который связываем
              .WithMany(s => s.Lists)//2 класс и свойство с которым связываем
              .Map(t => t.MapLeftKey("ListPhysicsId")//id 1 которое в таблице будет
              .MapRightKey("FETextIdId")//id 2
              .ToTable("ApplicationUserFETextId"));//название таблицы

            


            modelBuilder.Entity<StateObject>().HasMany(c => c.FeTextBegin)//
              .WithOptional(s => s.StateBegin);//

            modelBuilder.Entity<StateObject>().HasMany(c => c.FeTextEnd)//
             .WithOptional(s => s.StateEnd);//

            base.OnModelCreating(modelBuilder);//инициализация что бы роли и все остальное добавилось нормально
        }







    }
}