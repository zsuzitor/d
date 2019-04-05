using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class FizVel//: Item
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public bool Parametric { get; set; }//если этот экземпляр ЯВЛЯЕТСЯ параметрическим(именно ребенком)

        public ICollection< Action >Actions { get; set; }

        //? не сгенерировалось
        public ICollection<FEAction> FEActions { get; set; }

        public FizVel()
        {
            Parametric = false;

        }



        public static List<FizVel> GetParametricFizVels(string id)
        {
            List<FizVel> res = new List<FizVel>();
            using (var db = new ApplicationDbContext())
            {
                 res = db.FizVels.Where(x1 => x1.Parent == id).ToList();

                //TODO ошибка? условие должно быть если записей 0?????
                if (res.Count != 0)
                {
                    res.Add(db.FizVels.Where(parametricFizVel => parametricFizVel.Id == "NO_FIZVEL").First());

                    res = res.OrderBy(parametricFizVel => parametricFizVel.Id)
                                                                    .ToList();

                    // Отправляем его в представление
                    //ViewBag.parametricFizVelId = selectListOfParametricFizVels;
                }

                // Отправляем его в представление

            }
            return res;
        }


        //проверяет есть ли фэ которые используют что то из списка(не грузит детей и тд) и если хотя бы 1 итем блокируется не удаляет ничего
        public static List<int> TryDelete(ApplicationDbContext db, List<FizVel> list)//db_ = null
        {

            //var list = db.FizVels.Where(x1 => this.MassDeletedFizVels.FirstOrDefault(x2 => x2.Id == x1.Id || x2.Id == x1.Parent) != null).ToList();
            List<string> listId = list.Select(x1 => x1.Id).ToList();
            var blocked = db.FEActions.Where(x1 => listId.Contains(x1.FizVelId)).Select(x1 => x1.Idfe).ToList();
            if (blocked.Count > 0)
                return blocked;

            db.FizVels.RemoveRange(list);
            db.SaveChanges();
            return blocked;

        }

    }
}