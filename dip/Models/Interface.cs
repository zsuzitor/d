using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Web;

namespace dip.Models
{
    public class Interface
    {
        

    }


    public abstract class  AHasChilds<T> where T:new()
    {
        [NotMapped]
        public List<T> Childs { get; set; }
        [NotMapped]
        public T ParentItem { get; set; }



        public AHasChilds()
        {
            ParentItem = default(T);
            Childs = new List<T>();
        }

        public virtual void LoadChild()
        {
            if (this.Childs.Count < 1)
                this.ReLoadChild();
        }
        public abstract void ReLoadChild();

        //возвращает список от родителя к ребенку (последний элемент -ближайший родитель this)
        public abstract List<T> GetParentsList(ApplicationDbContext db_ = null);


        //возвращает список всех детей и их детей до "корней древа" для this
        public abstract List<T> GetChildsList(ApplicationDbContext db_ = null);
        

        /// <summary>
        /// загружает необходимое древо детей
        /// </summary>
        /// <param name="list">список детей элемента,которые должны быть загружены, и для которых должны быть загружены дети</param>
        /// <returns></returns>
        public abstract bool LoadPartialTree(List<T> list);
        //List<T> GetParentsList(ApplicationDbContext db_ = null);
    }


    public abstract class AParentDb<T> : AHasChilds<T> where T :  AParentDb<T>, new()
    {
        [Key]
        public string Id { get; set; }
        public string Parent { get; set; }



        /// <summary>
        /// загрузит частичное древо детей(загрузит до уровня детей из list включая их уровень)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public override bool LoadPartialTree(List<T> list)
        {
            this.LoadChild();
            if (list == null || list.Count < 1)
                return false;
            //this.LoadChild();
            foreach (var i in this.Childs)
            {
                if (list.FirstOrDefault(x1 => x1.Id == i.Id) != null) //if (list.Contains(i))
                    i.LoadPartialTree(list);
            }
          

            return true;
        }


        public static void DeleteFromDbFromListOnly(ApplicationDbContext db, System.Data.Entity.DbSet<T> collect, IEnumerable<string> del) //where T : ItemFormCheckbox<T>, new()
        {
            
            collect.RemoveRange(collect.Where(x1 => del.Contains(x1.Id)));
            db.SaveChanges();
        }


        public static void DeleteFromDb(ApplicationDbContext db, System.Data.Entity.DbSet<T> collect, IEnumerable<string> del) //where T : ItemFormCheckbox<T>, new()
        {
            //TODO можно переписать не под рефлексию а под вызов статики для интерфейса
            Type typeT = typeof(T);
            MethodInfo meth = typeT.GetMethod("GetChild");
            List<T> forDeleted = new List<T>();


            int start = 0;

            foreach (var i in collect.Where(x1 => del.FirstOrDefault(x2 => x2 == x1.Id) != null && x1.Parent != Constants.FeObjectBaseCharacteristic).ToList())
            {

                forDeleted.Add(i);

                for (; start < forDeleted.Count; ++start)
                {
                    var gg = new object[] { forDeleted[start].Id };
                    forDeleted.AddRange((List<T>)meth.Invoke(null, gg));
                }
            }

            collect.RemoveRange(forDeleted);
            db.SaveChanges();
        }

        public static List<List<T>> GetQueueParent(List<T> list)
        {
            List<T> withOutCHild = null;
            withOutCHild = list.Where(x1 => list.FirstOrDefault(x2 => x2.Parent == x1.Id) == null).ToList();
            List<List<T>> prosPath = new List<List<T>>();
            foreach (var i in withOutCHild)
            {

                List<T> queue = new List<T>();
                queue.Add(i);

                // bool ex = false;
                string prID = i.Parent;



                while (true)
                {

                    var pr = list.FirstOrDefault(x1 => x1.Id == prID);
                    if (pr == null)
                        break;
                    queue.Add(pr);
                    prID = pr.Parent;


                }

                queue.Reverse();
                prosPath.Add(queue);



            }
            return prosPath;
        }


        public override List<T> GetChildsList(ApplicationDbContext db_ = null)
        {
            List<T> res = new List<T>();
            var db = db_ ?? new ApplicationDbContext();

            //var par = db.StateObjects.FirstOrDefault(x1 => x1.Id == this.Parent);
            this.ReLoadChild();
            res.AddRange(this.Childs);
            foreach (var i in this.Childs)
            {
                res.AddRange(i.GetChildsList(db));
            }

            if (db_ == null)
                db.Dispose();

            return res;
        }

    }






    public abstract class ItemFormCheckbox<T> : AParentDb<T> where T : ItemFormCheckbox<T>, new()
    {
        //[Key]
        //public string Id { get; set; }
        public string Name { get; set; }
        //public string Parent { get; set; }
        //[NotMapped]
        //public T ParentItem { get; set; }

        //public List<Models.Domain.Action> Actions { get; set; }



        public ItemFormCheckbox()
        {

        }



        public static List<string> GetQueueParentString(List<List<T>> list)
        {
            List<string> res = new List<string>();
            foreach (var i in list)
            {
                string onePath = "";
                foreach (var i2 in i)
                {
                    onePath += i2.Name + "->";

                }
                res.Add(onePath);
            }
            return res;

        }

        public static string SortIds(string ids)
        {
            if (ids == null)
                return null;
            if (string.IsNullOrWhiteSpace(ids))
                return "";
            var gg = ids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            Array.Sort(gg);

            string res = string.Join(" ", gg);

            
            return res;
        }


        //public static List<string> SqlLikeSerchIdInString(string id)
        //{
        //    List<string> res = new List<string>();
        //    res.Add(id);
        //    res.Add(id + " %");
        //    res.Add("% " + id);
        //    res.Add("% " + id + " %");
           
        //    return res;
        //    }

    }












    public abstract class ItemDescrFormCheckbox<T>: ItemFormCheckbox<T> where T : ItemDescrFormCheckbox<T>,new()
    {
        //[Key]
        //public string Id { get; set; }
        //public string Name { get; set; }
        //public string Parent { get; set; }
        //[NotMapped]
        //public T ParentItem { get; set; }

        public List<Models.Domain.Action> Actions { get; set; }



        public ItemDescrFormCheckbox()
        {

        }

     

        //public static List<string> GetQueueParentString(List<List<T>> list)
        //{
        //    List<string> res = new List<string>();
        //    foreach (var i in list)
        //    {
        //        string onePath = "";
        //        foreach (var i2 in i)
        //        {
        //            onePath += i2.Name + "->";

        //        }
        //        res.Add(onePath);
        //    }
        //    return res;

        //}




    }


    //public interface CheckBoxForm
    //{
    //    string Id { get; set; }
    //    string Name { get; set; }
    //    string Parent { get; set; }
    //}


    public interface ShowsImage
    {
        int Id { get; set; }
        
        string IdForShow { get; }//set;
        byte[] Data { get; set; }

        //int FeTextIDFE { get; set; }
        //FEText FeText { get; set; }
    }


    public interface ShowsFEImage : ShowsImage
    {
         //int Id { get; set; }

        // byte[] Data { get; set; }

         int FeTextIDFE { get; set; }
         FEText FeText { get; set; }
    }




    public interface Idsf
    {
        
    }

}