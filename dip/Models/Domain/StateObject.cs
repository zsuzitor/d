/*файл класса модели БД предназначенного для хранения состояния объекта, добавлены методы для взаимодействия с сущностью
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{

    /// <summary>
    /// класс для хранения состояния объекта
    /// </summary>
    public class StateObject : ItemFormCheckbox<StateObject>
    {

        public int? CountPhase { get; set; }

        public List<FEText> FeTextBegin { get; set; }
        public List<FEText> FeTextEnd { get; set; }

        public StateObject()
        {
            CountPhase = null;
        }


        /// <summary>
        ///  метод для получения базовых состояний(1 уровень)
        /// </summary>
        /// <returns>список базовых состояний</returns>
        public static List<StateObject> GetBase()
        {
            List<StateObject> res = new List<StateObject>();
            using (var db = new ApplicationDbContext())
                res = db.StateObjects.Where(x1 => x1.Parent == "STRUCTOBJECT").ToList();
            return res;
        }

        /// <summary>
        /// клонирование объекта без ссылок
        /// </summary>
        /// <returns>новый объект-копия</returns>
        public StateObject CloneWithOutRef()
        {
            return new StateObject()
            {
                CountPhase = this.CountPhase,
                Id = this.Id,
                Name = this.Name,
                Parent = this.Parent
            };
        }

        /// <summary>
        /// метод возвращает ближайших детей
        /// </summary>
        /// <param name="id">id записи для которой нужно вернуть детей</param>
        /// <returns>список состояний</returns>
        public static List<StateObject> GetChild(string id)
        {
            List<StateObject> res = new List<StateObject>();
            using (var db = new ApplicationDbContext())
                res = db.StateObjects.Where(x1 => x1.Parent == id).ToList();
            return res;
        }

        /// <summary>
        /// получение записи по id
        /// </summary>
        /// <param name="id">id записи</param>
        /// <returns>запись найденная по id</returns>
        public static StateObject Get(string id)
        {
            StateObject res = null;
            if (!string.IsNullOrWhiteSpace(id))
                using (var db = new ApplicationDbContext())
                    res = db.StateObjects.FirstOrDefault(x1 => x1.Id == id);
            return res;
        }


        /// <summary>
        ///  перезагружает детей для записи
        /// </summary>
        public override void ReLoadChild()
        {
            using (var db = new ApplicationDbContext())
                this.Childs = db.StateObjects.Where(x1 => x1.Parent == this.Id).ToList();
        }


        /// <summary>
        /// возвращает список родителей и их родителей от корня до ребенка, где ребенок ближайший родитель this
        /// </summary>
        /// <param name="db_">контекст бд</param>
        /// <returns>список состояний</returns>
        public override List<StateObject> GetParentsList(ApplicationDbContext db_ = null)
        {
            List<StateObject> res = new List<StateObject>();
            var db = db_ ?? new ApplicationDbContext();
            var par = db.StateObjects.FirstOrDefault(x1 => x1.Id == this.Parent);
            if (par != null)
            {
                if (par.Parent != "STRUCTOBJECT")
                    res.AddRange(par.GetParentsList(db));
                res.Add(par);
            }
            if (db_ == null)
                db.Dispose();

            return res;
        }
    }
}