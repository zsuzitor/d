using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Spec: Item
    {
        //[Key]
        //public string Id { get; set; }

        //public string Name { get; set; }

        //public string Parent { get; set; }

        public List<Action> Actions { get; set; }

        [NotMapped]
        public List<Spec> SpecChilds { get; set; }

        public Spec()
        {
            Actions = new List<Action>();
            SpecChilds = new List<Spec>();

        }


        public static string SortIds(string ids)
        {
            if (ids == null)
                return null;
            string res = string.Join(" ", ids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
                     OrderBy(x1 => int.Parse(x1.Split(new string[] { "SPEC" }, StringSplitOptions.RemoveEmptyEntries)[1])).Distinct().ToList());
            return res;
        }


        public static string GetParents(string id, ApplicationDbContext db_ = null)
        {
            string res = "";
            var db = db_ ?? new ApplicationDbContext();

            var cur = db.Specs.FirstOrDefault(x1 => x1.Id == id)?.ParentId;
            if (!string.IsNullOrWhiteSpace(cur))
                if (cur.Split(new string[] { "SPEC" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
                {
                    res += cur + " ";
                    res += Spec.GetParents(cur, db);

                }


            if (db_ == null)
                db.Dispose();

            return res;
        }



        public static List<Spec> GetChild(string id)
        {
            // Получаем список значений, соответствующий данной характеристике
            List<Spec> res = new List<Spec>();
            using (var db = new ApplicationDbContext())
                 res = db.Specs.Where(spec => spec.ParentId == id).ToList();
            return res;

        }







    }
}