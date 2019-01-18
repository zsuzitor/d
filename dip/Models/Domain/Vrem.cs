using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Vrem: Item
    {
        //[Key]
        //public string Id { get; set; }

        //public string Name { get; set; }

        //public string Parent { get; set; }

        public List<Action> Actions { get; set; }

        [NotMapped]
        public List<Vrem> VremChilds { get; set; }

        public Vrem()
        {
            Actions = new List<Action>();
            VremChilds = new List<Vrem>();

        }


        public static string SortIds(string ids)
        {
            if (ids == null)
                return null;
            string res = string.Join(" ", ids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
                     OrderBy(x1 => int.Parse(x1.Split(new string[] { "VREM" }, StringSplitOptions.RemoveEmptyEntries)[1])).Distinct().ToList());
            return res;
        }


        public static string GetParents(string id, ApplicationDbContext db_ = null)
        {
            string res = "";
            var db = db_ ?? new ApplicationDbContext();

            var cur = db.Vrems.FirstOrDefault(x1 => x1.Id == id)?.Parent;
            if (!string.IsNullOrWhiteSpace(cur))
                if (cur.Split(new string[] { "VREM" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
                {
                    res += cur + " ";
                    res += Vrem.GetParents(cur, db);

                }


            if (db_ == null)
                db.Dispose();

            return res;
        }


        public static List<Vrem> GetChild(string id)
        {
            // Получаем список значений, соответствующий данной характеристике
            List<Vrem> res = new List<Vrem>();
            using (var db = new ApplicationDbContext())
                res = db.Vrems.Where(spec => spec.Parent == id).ToList();
            return res;

        }

    }
}