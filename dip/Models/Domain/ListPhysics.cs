using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class ListPhysics
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ApplicationUser> Users { get; set; }
        public List<FEText> Physics { get; set; }

        public ListPhysics()
        {
            Users = new List<ApplicationUser>();
            Physics = new List<Domain.FEText>();
        }
        public ListPhysics(string name):this()
        {
            this.Name = name;
        }



        public static List<ListPhysics> GetAll()
        {
            using (var db=new ApplicationDbContext())
            {
                return db.ListPhysics.ToList();
            }
        }
        public static ListPhysics Get(int id, ApplicationDbContext db_=null)
        {
            var db = db_ ?? new ApplicationDbContext();
           
                var res= db.ListPhysics.FirstOrDefault(x1 => x1.Id == id);
            if (db_ == null)
                db.Dispose();
            return res;
        }

        public static ListPhysics Create(string name)
        {
            ListPhysics res = new ListPhysics(name);
            if (res != null)
                using (var db = new ApplicationDbContext())
            {
                
                 db.ListPhysics.Add(res);
                db.SaveChanges();
            }
            return res;
        }

        public static ListPhysics Edit(int id,string name)
        {
            ListPhysics res = ListPhysics.Get(id) ;
            if (res != null)
                using (var db = new ApplicationDbContext())
            {
                db.Set<ListPhysics>().Attach(res);
                res.Name = name;
                db.SaveChanges();
            }
            return res;
        }

        public static ListPhysics Delete(int id)
        {
            ListPhysics res = ListPhysics.Get(id);
            if(res!=null)
            using (var db = new ApplicationDbContext())
            {
                db.Set<ListPhysics>().Attach(res);
                db.ListPhysics.Remove(res);
                db.SaveChanges();
            }
            return res;
        }

        public static ListPhysics AddPhys(int idphys,int idlist)
        {
            ListPhysics res = ListPhysics.Get(idlist);
            
            res.LoadPhysics();
            FEText phys = res.Physics.FirstOrDefault(x1 => x1.IDFE == idphys);
            if (phys != null)
                return null;
               
                phys = FEText.Get(idphys);
                if (phys == null)
                    return null;
            
            //FEText phys = FEText.Get(idphys);
            using (var db = new ApplicationDbContext())
            {
                db.Set<ListPhysics>().Attach(res);
                db.Set<FEText>().Attach(phys);
                res.Physics.Add(phys);
                db.SaveChanges();

                res.LoadUsers(db);
            }
            foreach (var i in res.Users)
                i.AddPhysics(phys.IDFE);

            return res;
        }
        public static ListPhysics DeletePhys(int idphys, int idlist)
        {
            ListPhysics res = ListPhysics.Get(idlist);
            if (res == null)
                return null;
            //FEText phys = FEText.Get(idphys);
            //if (phys == null)
            //    return null;
            res.LoadPhysics();
            FEText phys = res.Physics.FirstOrDefault(x1 => x1.IDFE == idphys);
            if (phys == null)
                return null;
            using (var db = new ApplicationDbContext())
            {
                db.Set<ListPhysics>().Attach(res);
                //db.Set<FEText>().Attach(phys);
                res.Physics.Remove(phys);
                db.SaveChanges();

                res.LoadUsers(db);
            }

            foreach (var i in res.Users)
                i.RemovePhysics(phys.IDFE);
            return res;
        }

        public static ListPhysics LoadPhysics(int id)
        {
            var obj=ListPhysics.Get(id);
            obj?.LoadPhysics();
            return obj;
        }

        public  void LoadPhysics(ApplicationDbContext db_=null)
        {
            var db = db_ ?? new ApplicationDbContext();
            
                db.Set<ListPhysics>().Attach(this);
                if (!db.Entry(this).Collection(x1 => x1.Physics).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.Physics).Load();

            if (db_ == null)
                db.Dispose();
        }

        public void LoadUsers(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();

            db.Set<ListPhysics>().Attach(this);
            if (!db.Entry(this).Collection(x1 => x1.Users).IsLoaded)
                db.Entry(this).Collection(x1 => x1.Users).Load();

            if (db_ == null)
                db.Dispose();
        }

    }
}