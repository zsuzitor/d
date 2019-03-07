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
        }



        public static List<ListPhysics> GetAll()
        {
            using (var db=new ApplicationDbContext())
            {
                return db.ListPhysics.ToList();
            }
        }
        public static ListPhysics Get(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.ListPhysics.FirstOrDefault(x1=>x1.Id== id);
            }
        }

        public static ListPhysics Create(string name)
        {
            ListPhysics res = new ListPhysics(name);
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
            using (var db = new ApplicationDbContext())
            {
                db.Set<ListPhysics>().Attach(res);
                db.ListPhysics.Remove(res);
                db.SaveChanges();
            }
            return res;
        }

        public static ListPhysics AddPhys(int id)
        {
            ListPhysics res = ListPhysics.Get(id);
            FEText phys = FEText.Get(id);
            using (var db = new ApplicationDbContext())
            {
                db.Set<ListPhysics>().Attach(res);
                db.Set<FEText>().Attach(phys);
                res.Physics.Add(phys);
                db.SaveChanges();
            }
            return res;
        }
        public static ListPhysics DeletePhys(int id)
        {
            ListPhysics res = ListPhysics.Get(id);
            FEText phys = FEText.Get(id);
            using (var db = new ApplicationDbContext())
            {
                db.Set<ListPhysics>().Attach(res);
                db.Set<FEText>().Attach(phys);
                res.Physics.Remove(phys);
                db.SaveChanges();
            }
            return res;
        }


    }
}