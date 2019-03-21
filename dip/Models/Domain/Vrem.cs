using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Vrem: ItemDescrFormCheckbox<Vrem>
    {
        //[Key]
        //public string Id { get; set; }

        //public string Name { get; set; }

        //public string Parent { get; set; }

        //public List<Action> Actions { get; set; }

        //[NotMapped]
        //public List<Vrem> VremChilds { get; set; }

        public Vrem()
        {
            Actions = new List<Action>();
            Childs = new List<Vrem>();

        }


        public static string SortIds(string ids)
        {
            if (ids == null)
                return null;
            string res = string.Join(" ", ids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
                     OrderBy(x1 => int.Parse(x1.Split(new string[] { "VREM" }, StringSplitOptions.RemoveEmptyEntries)[1])).Distinct().ToList());
            return res;
        }


        //public static string GetParentsId(string id, ApplicationDbContext db_ = null)
        //{
        //    string res = "";
        //    var db = db_ ?? new ApplicationDbContext();

        //    var cur = db.Vrems.FirstOrDefault(x1 => x1.Id == id)?.Parent;
        //    if (!string.IsNullOrWhiteSpace(cur))
        //        if (cur.Split(new string[] { "VREM" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
        //        {
        //            res += cur + " ";
        //            res += Vrem.GetParentsId(cur, db);

        //        }


        //    if (db_ == null)
        //        db.Dispose();

        //    return res;
        //}



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