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
    /// <summary>
    /// класс для хранения пользователей
    /// </summary>
    public class ApplicationUser : IdentityUser
    {

        public string Name { get; set; }
        public string Surname { get; set; }
        public bool? Male { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime DateRegistration { get; set; }
        public bool CloseProfile { get; set; }

        public List<Log> UserLogs { get; set; }
        public List<FEText> FavouritedPhysics { get; set; }
        public List<FEText> Physics { get; set; }//список выданных ФЭ
        public List<ListPhysics> ListPhysics { get; set; }



        public ApplicationUser() : base()
        {
            UserLogs = new List<Log>();
            Name = null;
            Surname = null;
            Birthday = null;
            DateRegistration = DateTime.Now;
            CloseProfile = false;
            Male = null;
            FavouritedPhysics = new List<FEText>();
            Physics = new List<FEText>();
            ListPhysics = new List<ListPhysics>();

        }



        /// <summary>
        /// метод для получения id текущего пользователя
        /// </summary>
        /// <returns>id пользователя</returns>
        public static string GetUserId()
        {
            return System.Web.HttpContext.Current.User.Identity.GetUserId();
        }

        /// <summary>
        /// метод для получения записи пользователя по id
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>запись пользователя</returns>
        public static ApplicationUser GetUser(string id)
        {
            ApplicationUser res = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
                res = ApplicationUser.GetUser(id, db);
            return res;
        }

        /// <summary>
        /// метод для получения записи пользователя по id
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <param name="db">контекст бд </param>
        /// <returns>запись пользователя</returns>
        public static ApplicationUser GetUser(string id, ApplicationDbContext db)
        {
            ApplicationUser res = null;
            if (string.IsNullOrWhiteSpace(id))
                return res;
            res = db.Users.FirstOrDefault(x1 => x1.Id == id);
            return res;
        }

        /// <summary>
        /// метод для получения всех пользователей системы
        /// </summary>
        /// <param name="db_">контекст бд</param>
        /// <returns>список пользователей</returns>
        public static List<ApplicationUser> GetAllUsers(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();
            List<ApplicationUser> res = db.Users.ToList();
            if (db_ == null)
                db.Dispose();
            return res;
        }


        /// <summary>
        /// метод для изменение записи пользователя
        /// </summary>
        /// <param name="user"> запись пользователя которую нужно изменить</param>
        public static void Edit(ApplicationUser user)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var old = ApplicationUser.GetUser(user.Id, db);
                old.Name = user.Name;
                old.Surname = user.Surname;
                old.Birthday = user.Birthday;
                old.Male = user.Male;
                db.SaveChanges();
            }
        }


        /// <summary>
        /// метод для загрузки ФЭ которые пользователь добавил в избранное
        /// </summary>
        public void LoadFavouritedList()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Set<ApplicationUser>().Attach(this);
                if (!db.Entry(this).Collection(x1 => x1.FavouritedPhysics).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.FavouritedPhysics).Load();
            }
        }


        /// <summary>
        /// метод для загрузки списков пользователя
        /// </summary>
        /// <param name="db_"> контекст бд</param>
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
        ///метод для загрузки разрешенных ФЭ пользователя 
        /// </summary>
        /// <param name="db_">контекст бд</param>
        public void LoadPhysics(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();
            db.Set<ApplicationUser>().Attach(this);
            if (!db.Entry(this).Collection(x1 => x1.Physics).IsLoaded)
                db.Entry(this).Collection(x1 => x1.Physics).Load();
            if (db_ == null)
                db.Dispose();
        }


        /// <summary>
        /// метод для добавление списка пользователю
        /// </summary>
        /// <param name="iduser">id пользователя</param>
        /// <param name="idlist">id списка</param>
        /// <param name="hadList">был ли список у пользователя до вызова этого метода</param>
        /// <returns>добавленный список</returns>
        //genered exception: NotFoundException
        public static ListPhysics AddList(string iduser, int idlist, out bool? hadList)
        {
            var user = ApplicationUser.GetUser(iduser);
            hadList = null;
            if (user != null)
                return user.AddList(idlist, out hadList);
            else
                return null;
        }

        /// <summary>
        /// метод для добавление списка пользователю
        /// </summary>
        /// <param name="idlist">id списка</param>
        /// <param name="hadList">был ли список у пользователя до вызова этого метода</param>
        /// <returns>добавленный список</returns>
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
                    this.ListPhysics.Add(list);
                    db.SaveChanges();
                    hadList = false;
                }
                else
                    hadList = true;

                list.LoadPhysics(db);

                if (!db.Entry(this).Collection(x1 => x1.Physics).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.Physics).Load();
                foreach (var i in list.Physics)

                    if (this.Physics.FirstOrDefault(x1 => x1.IDFE == i.IDFE) == null)
                        this.Physics.Add(i);
                db.SaveChanges();

            }
            return list;
        }


        /// <summary>
        /// метод для удаления списка у пользователя
        /// </summary>
        /// <param name="iduser">id пользователя</param>
        /// <param name="idlist">id списка</param>
        public static void RemoveList(string iduser, int idlist)
        {
            var user = ApplicationUser.GetUser(iduser);
            user.RemoveList(idlist);
        }

        /// <summary>
        /// метод для удаления списка у пользователя
        /// </summary>
        /// <param name="idlist">id списка</param>
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


        /// <summary>
        /// метод для добавления ФЭ пользователю
        /// </summary>
        /// <param name="iduser">id пользователя</param>
        /// <param name="idphys">id ФЭ</param>
        /// <param name="hadPhys">имел ли пользователь этот ФЭ до вызова этого метода</param>
        /// <returns>добавленная запись ФЭ</returns>
        public static FEText AddPhysics(string iduser, int idphys, out bool? hadPhys)
        {
            hadPhys = null;
            var user = ApplicationUser.GetUser(iduser);
            if (user != null)
                return user.AddPhysics(idphys, out hadPhys);
            return null;
        }

        /// <summary>
        /// метод для добавления ФЭ пользователю
        /// </summary>
        /// <param name="idphys">id ФЭ</param>
        /// <param name="hadPhys">имел ли пользователь этот ФЭ до вызова этого метода</param>
        /// <returns>добавленная запись ФЭ</returns>
        public FEText AddPhysics(int idphys, out bool? hadPhys)
        {
            FEText phys = null;
            hadPhys = null;
            if (idphys == Models.Constants.FEIDFORSEMANTICSEARCH)//id временной записи для сематического поиска у нее нет дескрипторов и text=="---"
                return phys;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Set<ApplicationUser>().Attach(this);

                if (!db.Entry(this).Collection(x1 => x1.Physics).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.Physics).Load();

                phys = this.Physics.FirstOrDefault(x1 => x1.IDFE == idphys);
                if (phys == null)
                {
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

        /// <summary>
        /// метод для удаления ФЭ у пользователя
        /// </summary>
        /// <param name="iduser">id пользователя</param>
        /// <param name="idphys">id ФЭ</param>
        public static void RemovePhysics(string iduser, int idphys)
        {
            var user = ApplicationUser.GetUser(iduser);
            user.RemovePhysics(idphys);
        }

        /// <summary>
        /// метод для удаления ФЭ у пользователя
        /// </summary>
        /// <param name="idphys">id ФЭ</param>
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

        /// <summary>
        /// метод для проверки записей которые разрешены для отображения для текущего пользователя
        /// </summary>
        /// <param name="idphys">список id фэ которые нужно проверить</param>
        /// <param name="HttpContext">контекст http</param>
        /// <returns>список выданных ФЭ из списка idphys</returns>
        public List<int> CheckAccessPhys(List<int> idphys, HttpContextBase HttpContext)
        {
            List<int> res = new List<int>();
            if (HttpContext == null)
                return res;

            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(this.Id);

            int? semanticFe = idphys.FirstOrDefault(x1 => x1 == Models.Constants.FEIDFORSEMANTICSEARCH);
            if (semanticFe != null)
                idphys.Remove((int)semanticFe);

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
                    foreach (var i in idphys)
                    {
                        if (tmpRes.Contains(i))
                            res.Add(i);
                    }
                }
            return res;
        }


        /// <summary>
        /// метод для получения следующего разрешенного ФЭ
        /// </summary>
        /// <param name="id">id текущего ФЭ</param>
        /// <param name="HttpContext">контекст http</param>
        /// <returns>следующая выданныя запись ФЭ</returns>
        public FEText GetNextAccessPhysic(int id, HttpContextBase HttpContext)
        {
            FEText res = null;
            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(this.Id);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (roles.Contains(RolesProject.admin.ToString()) || roles.Contains(RolesProject.subscriber.ToString()))
                {
                    res = db.FEText.FirstOrDefault(x1 => x1.IDFE > id && x1.IDFE != Models.Constants.FEIDFORSEMANTICSEARCH);
                    if (res == null)
                        res = db.FEText.FirstOrDefault();
                }
                else if (roles.Contains(RolesProject.user.ToString()))
                {
                    db.Set<ApplicationUser>().Attach(this);
                    res = db.Entry(this).Collection(x1 => x1.Physics).Query().FirstOrDefault(x1 => x1.IDFE > id);
                    if (res == null)
                        res = db.Entry(this).Collection(x1 => x1.Physics).Query().FirstOrDefault();
                }
            }
            return res;
        }

        /// <summary>
        ///  метод для получения предыдущего разрешенного ФЭ
        /// </summary>
        /// <param name="id">id текущего ФЭ</param>
        /// <param name="HttpContext">контекст http</param>
        /// <returns>предыдущая выданныя запись ФЭ</returns>
        public FEText GetPrevAccessPhysic(int id, HttpContextBase HttpContext)
        {
            FEText res = null;
            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(this.Id);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (roles.Contains(RolesProject.admin.ToString()) || roles.Contains(RolesProject.subscriber.ToString()))
                {
                    res = db.FEText.OrderByDescending(x1 => x1.IDFE).FirstOrDefault(x1 => x1.IDFE < id && x1.IDFE != Models.Constants.FEIDFORSEMANTICSEARCH);
                    if (res == null)
                        res = db.FEText.OrderByDescending(x1 => x1.IDFE).FirstOrDefault();
                }
                else if (roles.Contains(RolesProject.user.ToString()))
                {
                    db.Set<ApplicationUser>().Attach(this);
                    res = db.Entry(this).Collection(x1 => x1.Physics).Query().OrderByDescending(x1 => x1.IDFE).FirstOrDefault(x1 => x1.IDFE < id);
                    if (res == null)
                        res = db.Entry(this).Collection(x1 => x1.Physics).Query().OrderByDescending(x1 => x1.IDFE).FirstOrDefault();
                }
            }
            return res;
        }

        /// <summary>
        /// метод для получения первого разрешенного ФЭ
        /// </summary>
        /// <param name="HttpContext">контекст http</param>
        /// <returns>первая выданныя запись ФЭ</returns>
        public FEText GetFirstAccessPhysic(HttpContextBase HttpContext)
        {
            FEText res = null;
            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(this.Id);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (roles.Contains(RolesProject.admin.ToString()) || roles.Contains(RolesProject.subscriber.ToString()))
                {
                    res = db.FEText.FirstOrDefault(x1 => x1.IDFE != Models.Constants.FEIDFORSEMANTICSEARCH);
                }
                else if (roles.Contains(RolesProject.user.ToString()))
                {
                    db.Set<ApplicationUser>().Attach(this);
                    res = db.Entry(this).Collection(x1 => x1.Physics).Query().FirstOrDefault();
                }
            }
            return res;
        }

        /// <summary>
        /// метод для получения последнего разрешенного ФЭ
        /// </summary>
        /// <param name="HttpContext">контекст http</param>
        /// <returns>последняя выданныя запись ФЭ</returns>
        public FEText GetLastAccessPhysic(HttpContextBase HttpContext)
        {
            FEText res = null;
            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(this.Id);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (roles.Contains(RolesProject.admin.ToString()) || roles.Contains(RolesProject.subscriber.ToString()))
                {
                    res = db.FEText.OrderByDescending(x1 => x1.IDFE).FirstOrDefault(x1 => x1.IDFE != Models.Constants.FEIDFORSEMANTICSEARCH);
                }
                else if (roles.Contains(RolesProject.user.ToString()))
                {
                    db.Set<ApplicationUser>().Attach(this);
                    res = db.Entry(this).Collection(x1 => x1.Physics).Query().OrderByDescending(x1 => x1.IDFE).FirstOrDefault();
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

        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<AllAction> AllActions { get; set; }
        public DbSet<FEAction> FEActions { get; set; }
        public DbSet<FEObject> FEObjects { get; set; }
        public DbSet<FEText> FEText { get; set; }
        public DbSet<FizVel> FizVels { get; set; }
        public DbSet<Pro> Pros { get; set; }
        public DbSet<Spec> Specs { get; set; }
        public DbSet<Vrem> Vrems { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogParam> LogParams { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<FELatexFormula> FELatexFormulas { get; set; }

        public DbSet<StateObject> StateObjects { get; set; }
        public DbSet<PhaseCharacteristicObject> PhaseCharacteristicObjects { get; set; }
        public DbSet<ListPhysics> ListPhysics { get; set; }



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

            //список выданных ФЭ
            modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Physics)//1 класс и свойство который связываем
              .WithMany(s => s.Users)//2 класс и свойство с которым связываем
              .Map(t => t.MapLeftKey("ApplicationUserId")//id 1 которое в таблице будет
              .MapRightKey("FETextId")//id 2
              .ToTable("ApplicationUserFEText"));//название таблицы


            modelBuilder.Entity<ListPhysics>().HasMany(c => c.Physics)//1 класс и свойство который связываем
              .WithMany(s => s.Lists)//2 класс и свойство с которым связываем
              .Map(t => t.MapLeftKey("ListPhysicsId")//id 1 которое в таблице будет
              .MapRightKey("FETextId")//id 2
              .ToTable("ListPhysicsFEText"));//название таблицы



            modelBuilder.Entity<StateObject>().HasMany(c => c.FeTextBegin)//
              .WithOptional(s => s.StateBegin);//

            modelBuilder.Entity<StateObject>().HasMany(c => c.FeTextEnd)//
             .WithOptional(s => s.StateEnd);//

            base.OnModelCreating(modelBuilder);//инициализация что бы роли и все остальное добавилось нормально
        }

    }
}