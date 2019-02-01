using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Pro: Item<Pro>
    {
        //[Key]
        //public string Id { get; set; }

        //public string Name { get; set; }

        //public string Parent { get; set; }

        //public List<Action> Actions { get; set; }

        //[NotMapped]
        //public List<Pro> ProsChilds { get; set; }

        public Pro()
        {
            Actions = new List<Action>();
            Childs = new List<Pro>();
        }



        /// <summary>
        /// загружает необходимое древо детей
        /// </summary>
        /// <param name="list">список детей</param>
        /// <returns></returns>
        //public override bool LoadPartialTree(List<Pro> list)
        //{
        //    this.LoadChild();
        //    if (list == null || list.Count < 1)
        //        return false;
        //    //this.LoadChild();
        //    foreach(var i in this.Childs)
        //    {
        //        if (list.FirstOrDefault(x1=>x1.Id== i.Id)!=null)
        //            i.LoadPartialTree(list);
        //    }
        


        //    return true;
        //}



        public static string SortIds(string ids)
        {
            if (ids == null)
                return null;
            if (string.IsNullOrWhiteSpace(ids))
                return "";
           // var gg = ids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
           string res= string.Join(" ", ids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
                    OrderBy(x1 => int.Parse(x1.Split(new string[] { "PROS" }, StringSplitOptions.RemoveEmptyEntries)[1])).Distinct().ToList());
            return res;
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

        //public static string GetParentsId(string id, ApplicationDbContext db_ = null)
        //{
        //    string res = "";
        //    var db = db_ ?? new ApplicationDbContext();

        //    var cur = db.Pros.FirstOrDefault(x1 => x1.Id == id)?.Parent;
        //    if (!string.IsNullOrWhiteSpace(cur))
        //        if(cur.Split(new string[] { "PROS" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
        //        {
        //            res += cur + " ";
        //            res += Pro.GetParentsid(cur, db);

        //        }


        //    if (db_ == null)
        //        db.Dispose();

        //    return res;
        //}

        //TODO вынести в обстрактный класс?? сейчас придумал только костыльный способ через объект из за метода Pro.GetChild
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