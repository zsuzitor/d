/*файл класса модели БД предназначенного для хранения временной характеристики, добавлены методы для взаимодействия с сущностью
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

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

    /// <summary>
    /// класс для хранения 1 записи(checbox) для временных характеристик
    /// </summary>
    public class Vrem : ItemDescrFormCheckbox<Vrem>
    {

        public Vrem()
        {
            Childs = new List<Vrem>();

        }


        /// <summary>
        /// возвращает список от родителя к ребенку (последний элемент -ближайший родитель того к  которому применили)
        /// </summary>
        /// <param name="db_">контекст бд</param>
        /// <returns>список от родителя к ребенку</returns>
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


        /// <summary>
        ///  метод возвращает список ВСЕХ родителей(и их родителей) для id содержащихся в str
        /// </summary>
        /// <param name="str">строка с id, где id разделенны ' '</param>
        /// <param name="db">контекст бд</param>
        /// <returns>список id всех родителей</returns>
        public static List<string> GetParentListForIds(string str, ApplicationDbContext db)
        {
            var lstId = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var lst2Elem = db.Vrems.Where(x1 => lstId.Contains(x1.Id)).ToList();
            var lstRes = new List<string>();
            foreach (var i in lst2Elem)
            {
                lstRes.Add(i.Id);
                lstRes.AddRange(i.GetParentsList(db).Select(x1 => x1.Id));
            }
            return lstRes.Distinct().ToList();
        }


        /// <summary>
        ///  метод который из строки только детей формирует строку со всеми(дети+родители) 
        /// </summary>
        /// <param name="str">строка с id, где id разделенны ' '</param>
        /// <returns>строка дети+родители</returns>
        public static string GetAllIdsFor(string str)
        {
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


        /// <summary>
        /// метод для получения детей записи
        /// </summary>
        /// <param name="id">id записи</param>
        /// <returns>список vrem-детей записи id которой передано в параметре</returns>
        public static List<Vrem> GetChild(string id)
        {
            List<Vrem> res = new List<Vrem>();
            using (var db = new ApplicationDbContext())
                res = db.Vrems.Where(spec => spec.Parent == id).ToList();
            return res;
        }


        /// <summary>
        /// метод для загрузки детей записи
        /// </summary>
        public override void ReLoadChild()
        {
            using (var db = new ApplicationDbContext())
                this.Childs = db.Vrems.Where(x1 => x1.Parent == this.Id).ToList();
        }


        /// <summary>
        ///  метод проверяет есть ли фэ которые используют что то из списка(грузит детей и тд) и если хотя бы 1 итем блокируется не удаляет ничего
        /// </summary>
        /// <param name="db">контекст бд</param>
        /// <param name="list">список для удаления</param>
        /// <returns>список id которые блокируют удаление</returns>
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


        /// <summary>
        /// метод проверяет есть ли фэ которые используют что то из списка(не грузит детей и тд) и если хотя бы 1 итем блокируется не удаляет ничего
        /// </summary>
        /// <param name="db">контекст бд</param>
        /// <param name="list">записи для удаления</param>
        /// <returns>список id которые блокируют удаление</returns>
        public static List<int> TryDelete(ApplicationDbContext db, List<Vrem> list)//TODO вынести
        {

            var predicate = PredicateBuilder.False<FEAction>();
            foreach (var i in list)
            {
                predicate = predicate.Or(x1 => x1.Vrem == i.Id || x1.Vrem.StartsWith(i.Id + " ") ||
                  x1.Vrem.EndsWith(" " + i.Id) || x1.Vrem.Contains(" " + i.Id + " "));
            }

            var blocked = db.FEActions.Where(predicate).Select(x1 => x1.Idfe).ToList();

            if (blocked.Count > 0)
                return blocked;

            Vrem.DeleteFromDbFromListOnly(db, db.Vrems, list.Select(x1 => x1.Id));
            return blocked;
        }





        /// <summary>
        ///  метод для удаления прямых родителей если и родитель и ребенок есть в строке. вернет строку содержащую только id записей у которых нет детей
        /// </summary>
        /// <param name="strIds">строка с id, где id разделенны ' '</param>
        /// <returns>строка без прямых родителей(строка содержащуя только id записей у которых нет детей)</returns>
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