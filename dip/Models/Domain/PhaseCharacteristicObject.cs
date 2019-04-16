using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class PhaseCharacteristicObject : ItemFormCheckbox<PhaseCharacteristicObject>
    {


        public PhaseCharacteristicObject()
        {

        }



        public static List<PhaseCharacteristicObject> GetBase()
        {
            List<PhaseCharacteristicObject> res = new List<PhaseCharacteristicObject>();
            using (var db = new ApplicationDbContext())
                res = db.PhaseCharacteristicObjects.Where(x1 => x1.Parent == Constants.FeObjectBaseCharacteristic).ToList();
            return res;
        }

        //удаляет прямых родителей если и родитель и ребенок есть в строке
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





        //ближайшие дети
        public static List<PhaseCharacteristicObject> GetChild(string id)
        {
            // Получаем список значений, соответствующий данной характеристике
            List<PhaseCharacteristicObject> res = new List<PhaseCharacteristicObject>();
            using (var db = new ApplicationDbContext())
                res = db.PhaseCharacteristicObjects.Where(x1 => x1.Parent == id).ToList();
            return res;

        }


        public PhaseCharacteristicObject CloneWithOutRef()
        {
            return new PhaseCharacteristicObject()
            {

                Id = this.Id,
                Name = this.Name,
                Parent = this.Parent
            };


        }

        public static PhaseCharacteristicObject Get(string id)
        {
            PhaseCharacteristicObject res = null;
            if (!string.IsNullOrWhiteSpace(id))
                using (var db = new ApplicationDbContext())
                    res = db.PhaseCharacteristicObjects.FirstOrDefault(x1 => x1.Id == id);
            return res;
        }




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


        public static string GetAllIdsFor(string str)
        {
            //из строки только детей формирует строку со всеми(дети+родители) id которые нужно выделить
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