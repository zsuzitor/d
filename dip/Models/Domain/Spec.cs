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
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Parent { get; set; }

        public List<Action> Actions { get; set; }

        [NotMapped]
        public List<Spec> SpecChilds { get; set; }

        public Spec()
        {
            Actions = new List<Action>();
            SpecChilds = new List<Spec>();

        }


        public static List<Spec> GetChild(string id)
        {
            // Получаем список значений, соответствующий данной характеристике
            List<Spec> res = new List<Spec>();
            using (var db = new ApplicationDbContext())
                 res = db.Specs.Where(spec => spec.Parent == id).ToList();
            return res;

        }







    }
}