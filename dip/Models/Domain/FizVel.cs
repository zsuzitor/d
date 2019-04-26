using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{

    /// <summary>
    /// Название физической величины
    /// </summary>
    public class FizVel//: Item
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public bool Parametric { get; set; }//если этот экземпляр ЯВЛЯЕТСЯ параметрическим(именно ребенком)

        public ICollection<FEAction> FEActions { get; set; }

        public FizVel()
        {
            Parametric = false;

        }


        /// <summary>
        /// Метод для получения FizVel которые зависят от FizVel с id==id
        /// </summary>
        /// <param name="id">id записи для которой нужно найти зависимых</param>
        /// <returns></returns>
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
                }
            }
            return res;
        }



        /// <summary>
        /// метод проверяет есть ли фэ которые используют что то из списка(не грузит детей и тд) и если хотя бы 1 итем блокируется не удаляет ничего
        /// </summary>
        /// <param name="db"></param>
        /// <param name="list">список для удаления</param>
        /// <returns></returns>
        public static List<int> TryDelete(ApplicationDbContext db, List<FizVel> list)//db_ = null
        {
            List<string> listId = list.Select(x1 => x1.Id).ToList();
            var blocked = db.FEActions.Where(x1 => listId.Contains(x1.FizVelId) || listId.Contains(x1.FizVelSection)).Select(x1 => x1.Idfe).ToList();
            if (blocked.Count > 0)
                return blocked;

            db.FizVels.RemoveRange(list);
            db.SaveChanges();
            return blocked;
        }
    }
}