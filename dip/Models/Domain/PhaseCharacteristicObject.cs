using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{

    /// <summary>
    /// класс для хранения 1 записи(checbox) для характеристик объекта
    /// </summary>
    public class PhaseCharacteristicObject : ItemFormCheckbox<PhaseCharacteristicObject>
    {

        public PhaseCharacteristicObject()
        {

        }

        /// <summary>
        /// метод для получения базовых харектеристик(1 уровень)
        /// </summary>
        /// <returns></returns>
        public static List<PhaseCharacteristicObject> GetBase()
        {
            List<PhaseCharacteristicObject> res = new List<PhaseCharacteristicObject>();
            using (var db = new ApplicationDbContext())
                res = db.PhaseCharacteristicObjects.Where(x1 => x1.Parent == Constants.FeObjectBaseCharacteristic).ToList();
            return res;
        }

        /// <summary>
        /// метод для удаления прямых родителей если и родитель и ребенок есть в строке. вернет строку содержащую только id записей у которых нет детей
        /// </summary>
        /// <param name="strIds">строка с id, где id разделенны ' '</param>
        /// <returns></returns>
        public static string DeleteNotChildCheckbox(string strIds)
        {
            string res = "";
            var listId = strIds.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var i in listId)
            {
                var listItem = PhaseCharacteristicObject.GetChild(i);
                if (listItem.Count == 0)
                    res += i + " ";
                else
                {
                    bool needAdd = true;
                    //проверяем содержет ли strIds этот элемент
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


        /// <summary>
        /// метод возвращает список ВСЕХ родителей(и их родителей) для id содержащихся в str
        /// </summary>
        /// <param name="str">строка с id, где id разделенны ' '</param>
        /// <param name="db">контекст</param>
        /// <returns></returns>
        public static List<string> GetParentListForIds(string str, ApplicationDbContext db)
        {
            var lstId = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var lst2Elem = db.PhaseCharacteristicObjects.Where(x1 => lstId.Contains(x1.Id)).ToList();
            var lstRes = new List<string>();
            foreach (var i in lst2Elem)
            {
                lstRes.Add(i.Id);
                lstRes.AddRange(i.GetParentsList(db).Select(x1 => x1.Id));
            }
            return lstRes.Distinct().ToList();
        }


        /// <summary>
        /// метод возвращает ближайших детей
        /// </summary>
        /// <param name="id">id записи для которой нужно вернуть детей</param>
        /// <returns></returns>
        public static List<PhaseCharacteristicObject> GetChild(string id)
        {
            List<PhaseCharacteristicObject> res = new List<PhaseCharacteristicObject>();
            using (var db = new ApplicationDbContext())
                res = db.PhaseCharacteristicObjects.Where(x1 => x1.Parent == id).ToList();
            return res;
        }

        /// <summary>
        /// клонирование объекта без ссылок
        /// </summary>
        /// <returns></returns>
        public PhaseCharacteristicObject CloneWithOutRef()
        {
            return new PhaseCharacteristicObject()
            {
                Id = this.Id,
                Name = this.Name,
                Parent = this.Parent
            };
        }

        /// <summary>
        /// получение записи по id
        /// </summary>
        /// <param name="id">id записи</param>
        /// <returns></returns>
        public static PhaseCharacteristicObject Get(string id)
        {
            PhaseCharacteristicObject res = null;
            if (!string.IsNullOrWhiteSpace(id))
                using (var db = new ApplicationDbContext())
                    res = db.PhaseCharacteristicObjects.FirstOrDefault(x1 => x1.Id == id);
            return res;
        }



        /// <summary>
        /// перезагружает детей для записи
        /// </summary>
        public override void ReLoadChild()
        {
            using (var db = new ApplicationDbContext())
                this.Childs = db.PhaseCharacteristicObjects.Where(x1 => x1.Parent == this.Id).ToList();
        }




        /// <summary>
        /// возвращает список от родителя к ребенку (последний элемент -ближайший родитель this)
        /// </summary>
        /// <param name="db_"></param>
        /// <returns></returns>
        public override List<PhaseCharacteristicObject> GetParentsList(ApplicationDbContext db_ = null)
        {
            List<PhaseCharacteristicObject> res = new List<PhaseCharacteristicObject>();
            var db = db_ ?? new ApplicationDbContext();
            var par = db.PhaseCharacteristicObjects.FirstOrDefault(x1 => x1.Id == this.Parent);

            if (par != null)
            {
                if (par.Parent != Constants.FeObjectBaseCharacteristic)
                    res.AddRange(par.GetParentsList(db));
                res.Add(par);
            }
            if (db_ == null)
                db.Dispose();

            return res;
        }


        /// <summary>
        /// метод который из строки только детей формирует строку со всеми(дети+родители) 
        /// </summary>
        /// <param name="str">строка с id, где id разделенны ' '</param>
        /// <returns></returns>
        public static string GetAllIdsFor(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "";

            List<PhaseCharacteristicObject> mainLst = new List<PhaseCharacteristicObject>();
            var strmass = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (strmass == null)
                return null;
            foreach (var i in strmass)
            {
                using (var db = new ApplicationDbContext())
                {
                    var pr = db.PhaseCharacteristicObjects.First(x1 => x1.Id == i);

                    var lstPr = pr.GetParentsList();
                    lstPr.Add(pr);
                    mainLst.AddRange(lstPr);
                }
            }
            return string.Join(" ", mainLst.Select(x1 => x1.Id).Distinct());
        }
    }
}