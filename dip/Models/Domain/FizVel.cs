using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class FizVel: Item
    {

        //public string Id { get; set; }
        //public string Name { get; set; }
        //public string Parent { get; set; }

        public ICollection< Action >Actions { get; set; }

        //? не сгенерировалось
        public ICollection<FEAction> FEActions { get; set; }

        public FizVel()
        {


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

    }
}