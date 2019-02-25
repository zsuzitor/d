using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models
{
    public class Interface
    {
        

    }


    public abstract class  AHasChilds<T>
    {
        [NotMapped]
        public List<T> Childs { get; set; }
        [NotMapped]
        public T ParentItem { get; set; }


        public virtual void LoadChild()
        {
            if (this.Childs.Count < 1)
                this.ReLoadChild();
        }
        public abstract void ReLoadChild();

        //возвращает список от родителя к ребенку (последний элемент -ближайший родитель this)
        public abstract List<T> GetParentsList(ApplicationDbContext db_ = null);


        /// <summary>
        /// загружает необходимое древо детей
        /// </summary>
        /// <param name="list">список детей элемента,которые должны быть загружены, и для которых должны быть загружены дети</param>
        /// <returns></returns>
        public abstract bool LoadPartialTree(List<T> list);
        //List<T> GetParentsList(ApplicationDbContext db_ = null);
    }


    public abstract class AParentDb<T>: AHasChilds<T> where T: AParentDb<T>
    {
        [Key]
        public string Id { get; set; }
        public string Parent { get; set; }


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
            //foreach(var i in list)
            // {
            //     if (i.Id == this.Id)
            //         break;


            // }


            return true;
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

        


    }





        public abstract class ItemDescrFormCheckbox<T>: AParentDb<T> where T : ItemDescrFormCheckbox<T>
    {
        //[Key]
        //public string Id { get; set; }
        public string Name { get; set; }
        //public string Parent { get; set; }
        //[NotMapped]
        //public T ParentItem { get; set; }

        public List<Models.Domain.Action> Actions { get; set; }

        //[NotMapped]
        //public List<T> Childs { get; set; }

        //public override void LoadChild()
        //{
        //    if (this.Childs.Count < 1)
        //        this.ReLoadChild();
        //}

        //public abstract void ReLoadChild();

        //public abstract bool LoadPartialTree(List<T> list);
        //public abstract List<T> GetParentsList(ApplicationDbContext db_ = null); 



        //public override bool LoadPartialTree(List<T> list)
        //{
        //    this.LoadChild();
        //    if (list == null || list.Count < 1)
        //        return false;
        //    //this.LoadChild();
        //    foreach (var i in this.Childs)
        //    {
        //        if (list.FirstOrDefault(x1 => x1.Id == i.Id) != null) //if (list.Contains(i))
        //            i.LoadPartialTree(list);
        //    }
        //    //foreach(var i in list)
        //    // {
        //    //     if (i.Id == this.Id)
        //    //         break;


        //    // }


        //    return true;
        //}


        /// <summary>
        /// получаем списки  item от родителя к ребенку(просто выстроены по порядку)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">обычный список из которого надо построить древо родители\дети, содержит и детей и родителей, древо будет построено ТОЛЬКО по item из этого списка</param>
        /// <returns>список списков, в списке нижнего уровня последовательность от родителя к ребенку</returns>
        /// bad- родители\дети - список item в каждого проставлены дети(у кого нужно)
        //public static List<List<T>> GetQueueParent(List<T> list) 
        //{
        //    List<T> withOutCHild = null;
        //    withOutCHild = list.Where(x1 => list.FirstOrDefault(x2 => x2.Parent == x1.Id) == null).ToList();
        //    List<List<T>> prosPath = new List<List<T>>();
        //    foreach (var i in withOutCHild)
        //    {

        //        List<T> queue = new List<T>();
        //        queue.Add(i);

        //        // bool ex = false;
        //        string prID = i.Parent;



        //        while (true)
        //        {

        //            var pr = list.FirstOrDefault(x1 => x1.Id == prID);
        //            if (pr == null)
        //                break;
        //            queue.Add(pr);
        //            prID = pr.Parent;


        //        }

        //        queue.Reverse();
        //        prosPath.Add(queue);



        //    }
        //    return prosPath;
        //}

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




    }


    //public interface CheckBoxForm
    //{
    //    string Id { get; set; }
    //    string Name { get; set; }
    //    string Parent { get; set; }
    //}



    interface Idsf
    {
        
    }

}