using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class AllAction//: Item
    { 
        public string Id { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public bool Parametric { get; set; }


        public ICollection<Action> Actions { get; set; }

        public AllAction()
        {
            Actions = new List<Action>();
            Parametric = false;
        }


        public static AllAction Get(string id)
        {
            AllAction res = null;
            if(!string.IsNullOrWhiteSpace(id))
            using (var db=new ApplicationDbContext())
            {
                res = db.AllActions.FirstOrDefault(x1=>x1.Id==id);
            }
            return res;

        }

        public static bool? CheckParametric(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;
            using (var db = new ApplicationDbContext())
                return db.AllActions.FirstOrDefault(x1 => x1.Id == id)?.Parametric;

        }

    }
}