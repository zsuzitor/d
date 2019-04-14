using Binbin.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Pro: ItemDescrFormCheckbox<Pro>
    {
 
        public Pro()
        {
            //Actions = new List<Action>();
            Childs = new List<Pro>();
        }



        /// <summary>
        /// возвращает список от родителя к ребенку (последний элемент -ближайший родитель this)
        /// </summary>
        /// <param name="db_"></param>
        /// <returns></returns>
        public override List<Pro> GetParentsList( ApplicationDbContext db_ = null)
        {
            List<Pro> res = new List<Pro>();
            var db = db_ ?? new ApplicationDbContext();

            var par = db.Pros.FirstOrDefault(x1 => x1.Id == this.Parent);
            
            if (par != null)
            {
                if (par.Parent.Split(new string[] { "PROS" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
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
            List<Pro> mainLst = new List<Pro>();

            foreach (var i in str.Split(new string[] {" " },StringSplitOptions.RemoveEmptyEntries))
            {
                using (var db = new ApplicationDbContext())
                {
                    var pr = db.Pros.First(x1 => x1.Id == i);

                    var lstPr = pr.GetParentsList();
                    lstPr.Add(pr);
                    mainLst.AddRange(lstPr);
                }
            }
            return string.Join(" ", mainLst.Select(x1 => x1.Id).Distinct());

        }


        //TODO вынести в обстрактный класс?? сейчас придумал только костыльный способ через объект из за метода Pro.GetChild
        //удаляет прямых родителей если и родитель и ребенок есть в строке
        public static string DeleteNotChildCheckbox(string strIds)
        {

            string res = "";
            var listId = strIds.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var i in listId)
            {
                var listItem = Pro.GetChild(i);
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
                    if(needAdd)
                        res += i + " ";
                }
                    

            }

            return res.Trim();

        }

        //проверяет есть ли фэ которые используют что то из списка(грузит детей и тд) и если хотя бы 1 итем блокируется не удаляет ничего
        public static List<int> TryDeleteWithChilds(ApplicationDbContext db, List<Pro> list)//TODO вынести
        {
            if (list.Count == 0)
                return new List<int>();

            List<Pro> fordel = new List<Pro>();
            foreach (var i in list)
            {
                fordel.Add(i);
                fordel.AddRange(i.GetChildsList(db));

            }

            return Pro.TryDelete(db, fordel);
        }


        //ищет фэ которые ссылаются, если есть хотя бы 1 то ничего не удаляет
        //проверяет есть ли фэ которые используют что то из списка(не грузит детей и тд) и если хотя бы 1 итем блокируется не удаляет ничего
        public static List<int> TryDelete(ApplicationDbContext db, List<Pro> list)//TODO вынести
        {
            //TODO проверям можно ли удалить проверять actionid и все что удалится связанное
            //List<int> blockFe = new List<int>();

            //формировать регулярку по списку удаляемых объектов
            var predicate = PredicateBuilder.False<FEAction>();
            foreach (var i in list)
            {
              
                predicate = predicate.Or(x1 => x1.Pros == i.Id || x1.Pros.StartsWith(i.Id + " ") ||
                 x1.Pros.EndsWith(" "+i.Id ) || x1.Pros.Contains(" " + i.Id + " "));
            }
           // var people = db.FEActions.Where("it.Pros LIKE @searchTerm", new ObjectParameter("searchTerm", ""));
            var blocked = db.FEActions.Where(predicate).Select(x1 => x1.Idfe).ToList();


            if (blocked.Count > 0)
                return blocked;
            Pro.DeleteFromDbFromListOnly(db, db.Pros, list.Select(x1 => x1.Id));
            return blocked;
        }


        public static List<Pro> GetChild(string id)
        {
            // Получаем список значений, соответствующий данной характеристике
            List<Pro> res = new List<Pro>();
            using (var db = new ApplicationDbContext())
                res = db.Pros.Where(pros => pros.Parent == id).ToList();
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
                    this.Childs = db.Pros.Where(x1 => x1.Parent == this.Id).ToList();
        }

    }
}