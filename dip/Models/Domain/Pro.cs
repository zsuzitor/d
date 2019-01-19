using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Pro: Item
    {
        //[Key]
        //public string Id { get; set; }

        //public string Name { get; set; }

        //public string Parent { get; set; }

        public List<Action> Actions { get; set; }

        public Pro Parent { get; set; }
        //[NotMapped]
        public List<Pro> ProsChilds { get; set; }

        public Pro()
        {
            Actions = new List<Action>();
            ProsChilds = new List<Pro>();
        }




        public static string SortIds(string ids)
        {
            if (ids == null)
                return null;
            if (string.IsNullOrWhiteSpace(ids))
                return "";
           // var gg = ids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
           string res= string.Join(" ", ids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
                    OrderBy(x1 => int.Parse(x1.Split(new string[] { "PROS" }, StringSplitOptions.RemoveEmptyEntries)[1])).Distinct().ToList());
            return res;
        }


        public static string GetParents(string id, ApplicationDbContext db_ = null)
        {
            string res = "";
            var db = db_ ?? new ApplicationDbContext();

            var cur = db.Pros.FirstOrDefault(x1 => x1.Id == id)?.ParentId;
            if (!string.IsNullOrWhiteSpace(cur))
                if(cur.Split(new string[] { "PROS" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
                {
                    res += cur + " ";
                    res += Pro.GetParents(cur, db);

                }
            
                
            if (db_ == null)
                db.Dispose();

            return res;
        }
        

        public static List<Pro> GetChild(string id)
        {
            // Получаем список значений, соответствующий данной характеристике
            List<Pro> res = new List<Pro>();
            using (var db = new ApplicationDbContext())
                res = db.Pros.Where(pros => pros.ParentId == id).ToList();
            return res;

        }
    }
}