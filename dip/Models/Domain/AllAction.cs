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


        //проверяет есть ли фэ которые используют что то из списка(грузит детей и тд) и если хотя бы 1 итем блокируется не удаляет ничего
        public static List<int> TryDeleteWithChilds(ApplicationDbContext db, List<AllAction> list)//TODO вынести
        {
            //TODO проверям можно ли удалить проверять actionid и все что удалится связанное
            List<int> blockFe = new List<int>();
            
            if (list.Count == 0)
                return blockFe;
            foreach (var i in list)
            {
                blockFe.AddRange(db.FEActions.Where(x1 => x1.Name == i.Id).Select(x1 => x1.Idfe).ToList());

                var fizvel = db.FizVels.Where(x1 => x1.Parent == i.Id + "_FIZVEL").ToList();
                //List<FizVel> paramFizvel = new List<FizVel>();
                if (i.Parametric)
                {
                    List<string> fizveldtr = fizvel.Select(x1 => x1.Id).ToList();
                    fizvel.AddRange(db.FizVels.Where(x1 => fizveldtr.FirstOrDefault(x2 => x2 == x1.Parent) != null).ToList());
                }
                else
                {
                    var pros = db.Pros.Where(x1 => x1.Parent == i.Id + "_PROS").Select(x1 => x1.Id).ToList();
                    //Pro.DeleteFromDb(db, db.Pros, pros);
                    blockFe.AddRange(Pro.TryDeleteWithChilds(db, db.Pros.Where(x1 => pros.FirstOrDefault(x2 => x2 == x1.Id) != null).ToList()));

                    var spec = db.Specs.Where(x1 => x1.Parent == i.Id + "_SPEC").Select(x1 => x1.Id).ToList();
                    //Spec.DeleteFromDb(db, db.Specs, spec);
                    blockFe.AddRange(Spec.TryDeleteWithChilds(db, db.Specs.Where(x1 => spec.FirstOrDefault(x2 => x2 == x1.Id) != null).ToList()));
                    var vrem = db.Vrems.Where(x1 => x1.Parent == i.Id + "_VREM").Select(x1 => x1.Id).ToList();
                    //Vrem.DeleteFromDb(db, db.Vrems, vrem);
                    blockFe.AddRange(Vrem.TryDeleteWithChilds(db, db.Vrems.Where(x1 => vrem.FirstOrDefault(x2 => x2 == x1.Id) != null).ToList()));
                }
                blockFe.AddRange(FizVel.TryDelete(db, fizvel));
            }
            if (blockFe.Count == 0)
            {
                db.AllActions.RemoveRange(list);
                db.SaveChanges();

            }

            return blockFe;



        }



        }
}