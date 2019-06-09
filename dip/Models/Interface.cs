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

    /// <summary>
    /// абстрактный класс который содержит детей и родителя
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AHasChilds<T> where T : new()
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

        /// <summary>
        /// метод для загрузки детей, если их количество == 0
        /// </summary>
        public virtual void LoadChild()
        {
            if (this.Childs.Count < 1)
                this.ReLoadChild();
        }

        /// <summary>
        /// метод для перезагрузки детей
        /// </summary>
        public abstract void ReLoadChild();

        /// <summary>
        /// метод для возвращения списока от родителя к ребенку (последний элемент -ближайший родитель this)
        /// </summary>
        /// <param name="db_"></param>
        /// <returns></returns>
        public abstract List<T> GetParentsList(ApplicationDbContext db_ = null);


        /// <summary>
        ///метод- возвращает список всех детей и их детей до "корней древа" для this
        /// </summary>
        /// <param name="db_"></param>
        /// <returns></returns>
        public abstract List<T> GetChildsList(ApplicationDbContext db_ = null);


        /// <summary>
        /// загружает необходимое древо детей
        /// </summary>
        /// <param name="list">список детей элемента,которые должны быть загружены, и для которых должны быть загружены дети</param>
        /// <returns></returns>
        public abstract bool LoadPartialTree(List<T> list);
    }

    /// <summary>
    /// абстрактный класс записи которая хранится в бд, имеет список детей и родителя
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AParentDb<T> : AHasChilds<T> where T : AParentDb<T>, new()
    {
        [Key]
        public string Id { get; set; }
        public string Parent { get; set; }



        /// <summary>
        /// метод для загрузки частичного древа детей(загрузит до уровня детей из list включая их уровень)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public override bool LoadPartialTree(List<T> list)
        {
            this.LoadChild();
            if (list == null || list.Count < 1)
                return false;
            foreach (var i in this.Childs)
            {
                if (list.FirstOrDefault(x1 => x1.Id == i.Id) != null) //if (list.Contains(i))
                    i.LoadPartialTree(list);
            }
            return true;
        }

        /// <summary>
        /// метод для удаления элементов которые входят в список из коллекции
        /// </summary>
        /// <param name="db"></param>
        /// <param name="collect"></param>
        /// <param name="del"></param>
        public static void DeleteFromDbFromListOnly(ApplicationDbContext db, System.Data.Entity.DbSet<T> collect, IEnumerable<string> del) //where T : ItemFormCheckbox<T>, new()
        {
            collect.RemoveRange(collect.Where(x1 => del.Contains(x1.Id)));
            db.SaveChanges();
        }


        /// <summary>
        /// метод для удаления элементов коллекции и их детей
        /// </summary>
        /// <param name="db"></param>
        /// <param name="collect"></param>
        /// <param name="del"></param>
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

        /// <summary>
        /// метод для получения очереди родителей
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<List<T>> GetQueueParent(List<T> list)
        {
            List<T> withOutCHild = null;
            withOutCHild = list.Where(x1 => list.FirstOrDefault(x2 => x2.Parent == x1.Id) == null).ToList();
            List<List<T>> prosPath = new List<List<T>>();
            foreach (var i in withOutCHild)
            {
                List<T> queue = new List<T>();
                queue.Add(i);
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


        /// <summary>
        /// метод для получения списка детей
        /// </summary>
        /// <param name="db_"></param>
        /// <returns></returns>
        public override List<T> GetChildsList(ApplicationDbContext db_ = null)
        {
            List<T> res = new List<T>();
            var db = db_ ?? new ApplicationDbContext();
            this.ReLoadChild();
            res.AddRange(this.Childs);
            foreach (var i in this.Childs)
                res.AddRange(i.GetChildsList(db));
            if (db_ == null)
                db.Dispose();
            return res;
        }
    }





    /// <summary>
    /// абстрактный класс для чекбоксов форма
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ItemFormCheckbox<T> : AParentDb<T> where T : ItemFormCheckbox<T>, new()
    {
        [Required]
        public string Name { get; set; }


        public ItemFormCheckbox()
        {

        }


        /// <summary>
        ///  метод преобразования очереди родителей в очередь родителей(только названия)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
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


        /// <summary>
        /// метод для сортировки id
        /// </summary>
        /// <param name="ids"> строка с id где id должны быть разделены ' '</param>
        /// <returns></returns>
        public static string SortIds(string ids)
        {
            if (ids == null)
                return null;
            if (string.IsNullOrWhiteSpace(ids))
                return "";
            var gg = ids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            return SortIds(gg);
        }


        /// <summary>
        /// метод для сортировки id
        /// </summary>
        /// <param name="ids">коллекцияя id для сортировки</param>
        /// <returns></returns>
        public static string SortIds(IEnumerable<string> ids)
        {
            if (ids == null)
                return null;
            var resArray = ids.ToArray();
            Array.Sort(resArray);
            string res = string.Join(" ", resArray);
            return res;
        }
    }






    /// <summary>
    /// абстрактный класс  для чекбоксов-дескрипторная форма
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ItemDescrFormCheckbox<T> : ItemFormCheckbox<T> where T : ItemDescrFormCheckbox<T>, new()
    {

        public ItemDescrFormCheckbox()
        {

        }

    }



    /// <summary>
    /// интерфейс для хранения данных изображения
    /// </summary>
    public interface IShowsImage
    {
        int Id { get; set; }
        string IdForShow { get; }
        byte[] Data { get; set; }
    }


    /// <summary>
    /// интерфейс для хранения данных изображения который относится к записи фэ
    /// </summary>
    public interface IShowsFEImage : IShowsImage
    {
        int FeTextIDFE { get; set; }
        FEText FeText { get; set; }
    }




    public interface Idsf
    {

    }

}