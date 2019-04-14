using Binbin.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Vrem: ItemDescrFormCheckbox<Vrem>
    {
     

        public Vrem()
        {
            //Actions = new List<Action>();
            Childs = new List<Vrem>();

        }



        /// <summary>
        /// возвращает список от родителя к ребенку (последний элемент -ближайший родитель того к  которому применили)
        /// </summary>
        /// <param name="db_"></param>
        /// <returns></returns>
        public override List<Vrem> GetParentsList(ApplicationDbContext db_ = null)
        {
            List<Vrem> res = new List<Vrem>();
            var db = db_ ?? new ApplicationDbContext();

            var par = db.Vrems.FirstOrDefault(x1 => x1.Id == this.Parent);

            if (par != null)
            {
                if (par.Parent.Split(new string[] { "VREM" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
                    res.AddRange(par.GetParentsList(db));

                res.Add(par);
            }



            if (db_ == null)
                db.Dispose();

            return res;
        }


        public static string GetAllIdsFor(string str)
        {
            //из строки только детей формирует строку со всеми(дети+родители) id которые нужно выделить
            List<Vrem> mainLst = new List<Vrem>();

            foreach (var i in str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
            {
                using (var db = new ApplicationDbContext())
                {
                    var pr = db.Vrems.First(x1 => x1.Id == i);

                    var lstPr = pr.GetParentsList();
                    lstPr.Add(pr);
                    mainLst.AddRange(lstPr);
                }
            }
            return string.Join(" ", mainLst.Select(x1 => x1.Id).Distinct());

        }


        public static List<Vrem> GetChild(string id)
        {
            // Получаем список значений, соответствующий данной характеристике
            List<Vrem> res = new List<Vrem>();
            using (var db = new ApplicationDbContext())
                res = db.Vrems.Where(spec => spec.Parent == id).ToList();
            return res;

        }

        public override void LoadChild()
        {
            if (this.Childs.Count < 1)
                this.ReLoadChild();
        }

        public override void ReLoadChild()
        {

            using (var db = new ApplicationDbContext())
                this.Childs = db.Vrems.Where(x1 => x1.Parent == this.Id).ToList();
        }

        //проверяет есть ли фэ которые используют что то из списка(грузит детей и тд) и если хотя бы 1 итем блокируется не удаляет ничего
        public static List<int> TryDeleteWithChilds(ApplicationDbContext db, List<Vrem> list)//TODO вынести
        {
            if (list.Count == 0)
                return new List<int>();

            List<Vrem> fordel = new List<Vrem>();
            foreach (var i in list)
            {
                fordel.Add(i);
                fordel.AddRange(i.GetChildsList(db));

            }

            return Vrem.TryDelete(db, fordel);
        }


        //проверяет есть ли фэ которые используют что то из списка(не грузит детей и тд) и если хотя бы 1 итем блокируется не удаляет ничего
        public static List<int> TryDelete(ApplicationDbContext db, List<Vrem> list)//TODO вынести
        {
            //TODO проверям можно ли удалить проверять actionid и все что удалится связанное



            //формировать регулярку по списку удаляемых объектов
            var predicate = PredicateBuilder.False<FEAction>();
            foreach (var i in list)
            {
                //foreach (var src in Vrem.SqlLikeSerchIdInString(i.Id))
                //{

                //    predicate = predicate.Or(x1 => SqlMethods.Like(x1.Vrem, src));
                //}
                predicate = predicate.Or(x1 => x1.Vrem == i.Id || x1.Vrem.StartsWith(i.Id + " ") ||
                  x1.Vrem.EndsWith(" " + i.Id) || x1.Vrem.Contains(" " + i.Id + " "));
            }

            var blocked = db.FEActions.Where(predicate).Select(x1 => x1.Idfe).ToList();




            if (blocked.Count > 0)
                return blocked;


            Vrem.DeleteFromDbFromListOnly(db, db.Vrems, list.Select(x1 => x1.Id));
            return blocked;
        }





        //TODO вынести в обстрактный класс?? сейчас придумал только костыльный способ через объект из за метода Pro.GetChild
        //удаляет прямых родителей если и родитель и ребенок есть в строке
        public static string DeleteNotChildCheckbox(string strIds)
        {

            string res = "";
            var listId = strIds.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var i in listId)
            {
                var listItem = Vrem.GetChild(i);
                if (listItem.Count == 0)
                    res += i + " ";
                else
                {
                    bool needAdd = true;
                    foreach (var i2 in listItem)
                    {
                        if (listId.Contains(i2.Id))
                            needAdd = false;
                    }
                    if (needAdd)
                        res += i + " ";
                }


            }

            return res.Trim();

        }


    }
}