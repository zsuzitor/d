using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Spec: ItemDescrFormCheckbox<Spec>
    {
        //[Key]
        //public string Id { get; set; }

        //public string Name { get; set; }

        //public string Parent { get; set; }

        //public List<Action> Actions { get; set; }

        //[NotMapped]
        //public List<Spec> SpecChilds { get; set; }

        public Spec()
        {
            Actions = new List<Action>();
            Childs = new List<Spec>();

        }


        //public static string SortIds(string ids)
        //{
        //    if (ids == null)
        //        return null;
        //    if (string.IsNullOrWhiteSpace(ids))
        //        return "";
        //    var gg = ids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        //    Array.Sort(gg);
        //    string res = string.Join(" ", gg);
        //    //string res = string.Join(" ", ids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
        //    //         OrderBy(x1 => int.Parse(x1.Split(new string[] { "SPEC" }, StringSplitOptions.RemoveEmptyEntries)[1])).Distinct().ToList());
        //    return res;
        //}


        //public static string GetParentsId(string id, ApplicationDbContext db_ = null)
        //{
        //    string res = "";
        //    var db = db_ ?? new ApplicationDbContext();

        //    var cur = db.Specs.FirstOrDefault(x1 => x1.Id == id)?.Parent;
        //    if (!string.IsNullOrWhiteSpace(cur))
        //        if (cur.Split(new string[] { "SPEC" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
        //        {
        //            res += cur + " ";
        //            res += Spec.GetParentsId(cur, db);

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
        public override List<Spec> GetParentsList(ApplicationDbContext db_ = null)
        {
            List<Spec> res = new List<Spec>();
            var db = db_ ?? new ApplicationDbContext();

            var par = db.Specs.FirstOrDefault(x1 => x1.Id == this.Parent);

            if (par != null)
            {
                if (par.Parent.Split(new string[] { "SPEC" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
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
            List<Spec> mainLst = new List<Spec>();

            foreach (var i in str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
            {
                using (var db = new ApplicationDbContext())
                {
                    var pr = db.Specs.First(x1 => x1.Id == i);

                    var lstPr = pr.GetParentsList();
                    lstPr.Add(pr);
                    mainLst.AddRange(lstPr);
                }
            }
            return string.Join(" ", mainLst.Select(x1 => x1.Id).Distinct());

        }

        public static List<Spec> GetChild(string id)
        {
            // Получаем список значений, соответствующий данной характеристике
            List<Spec> res = new List<Spec>();
            using (var db = new ApplicationDbContext())
                 res = db.Specs.Where(spec => spec.Parent == id).ToList();
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
                this.Childs = db.Specs.Where(x1 => x1.Parent == this.Id).ToList();
        }



        //TODO вынести в обстрактный класс?? сейчас придумал только костыльный способ через объект из за метода Pro.GetChild
        //удаляет прямых родителей если и родитель и ребенок есть в строке
        public static string DeleteNotChildCheckbox(string strIds)
        {

            string res = "";
            var listId = strIds.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var i in listId)
            {
                var listItem = Spec.GetChild(i);
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