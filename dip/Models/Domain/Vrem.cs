﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Vrem: Item
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Parent { get; set; }

        public ICollection<Action> Actions { get; set; }

        public Vrem()
        {
            Actions = new List<Action>();

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