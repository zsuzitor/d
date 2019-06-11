/*файл класса модели БД предназначенного для хранения списка записей ФЭ, добавлены методы для взаимодействия с сущностью
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{

    /// <summary>
    /// класс для хранения списков ФЭ
    /// </summary>
    public class ListPhysics
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название должно быть установлено")]
        [MinLength(5, ErrorMessage = "Длина должна быть больше 5")]
        public string Name { get; set; }

        public List<ApplicationUser> Users { get; set; }
        public List<FEText> Physics { get; set; }

        public ListPhysics()
        {
            Users = new List<ApplicationUser>();
            Physics = new List<Domain.FEText>();
        }
        public ListPhysics(string name) : this()
        {
            this.Name = name;
        }


        /// <summary>
        /// метод для получения всех списков
        /// </summary>
        /// <returns>список списков</returns>
        public static List<ListPhysics> GetAll()
        {
            using (var db = new ApplicationDbContext())
                return db.ListPhysics.ToList();
        }

        /// <summary>
        /// метод для получения списка по id
        /// </summary>
        /// <param name="id">id списка</param>
        /// <param name="db_">контекст</param>
        /// <returns>запись списка</returns>
        public static ListPhysics Get(int? id, ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();

            var res = db.ListPhysics.FirstOrDefault(x1 => x1.Id == id);
            if (db_ == null)
                db.Dispose();
            return res;
        }


        /// <summary>
        /// метод для создания нового списка
        /// </summary>
        /// <param name="name"> название списка</param>
        /// <returns>созданный список</returns>
        public static ListPhysics Create(string name)
        {
            ListPhysics res = new ListPhysics(name);
            if (res != null)
                using (var db = new ApplicationDbContext())
                {
                    db.ListPhysics.Add(res);
                    db.SaveChanges();
                }
            return res;
        }


        /// <summary>
        /// метод для редактирования списка
        /// </summary>
        /// <param name="id">id списка</param>
        /// <param name="name">новое название списка</param>
        /// <returns>запись измененного списка</returns>
        public static ListPhysics Edit(int? id, string name)
        {
            ListPhysics res = ListPhysics.Get(id);
            if (res != null)
                using (var db = new ApplicationDbContext())
                {
                    db.Set<ListPhysics>().Attach(res);
                    res.Name = name;
                    db.SaveChanges();
                }
            return res;
        }


        /// <summary>
        /// метод для удаления списка
        /// </summary>
        /// <param name="id">id списка</param>
        /// <returns>запись удаленного списка</returns>
        public static ListPhysics Delete(int? id)
        {
            ListPhysics res = ListPhysics.Get(id);
            if (res != null)
                using (var db = new ApplicationDbContext())
                {
                    db.Set<ListPhysics>().Attach(res);
                    db.ListPhysics.Remove(res);
                    db.SaveChanges();
                }
            return res;
        }


        /// <summary>
        /// метод для добавления ФЭ в список
        /// </summary>
        /// <param name="idphys">id фэ</param>
        /// <param name="idlist">id списка</param>
        /// <returns>запись списка</returns>
        public static ListPhysics AddPhys(int idphys, int idlist)
        {
            ListPhysics res = ListPhysics.Get(idlist);

            res.LoadPhysics();
            FEText phys = res.Physics.FirstOrDefault(x1 => x1.IDFE == idphys);
            if (phys != null)
                return null;

            if (idphys == Constants.FEIDFORSEMANTICSEARCH)
                return null;

            phys = FEText.Get(idphys);
            if (phys == null)
                return null;

            using (var db = new ApplicationDbContext())
            {
                db.Set<ListPhysics>().Attach(res);
                db.Set<FEText>().Attach(phys);
                res.Physics.Add(phys);
                db.SaveChanges();
                res.LoadUsers(db);
            }
            foreach (var i in res.Users)
            {
                bool? f;
                i.AddPhysics(phys.IDFE, out f);
            }

            return res;
        }


        /// <summary>
        /// метод для удаления фэ из списка
        /// </summary>
        /// <param name="idphys">id фэ</param>
        /// <param name="idlist">id списка</param>
        /// <returns>запись списка из которого удаляли</returns>
        public static ListPhysics DeletePhys(int idphys, int idlist)
        {
            ListPhysics res = ListPhysics.Get(idlist);
            if (res == null)
                return null;
            res.LoadPhysics();
            FEText phys = res.Physics.FirstOrDefault(x1 => x1.IDFE == idphys);
            if (phys == null)
                return null;
            using (var db = new ApplicationDbContext())
            {
                db.Set<ListPhysics>().Attach(res);
                res.Physics.Remove(phys);
                db.SaveChanges();
                res.LoadUsers(db);
            }
            foreach (var i in res.Users)
                i.RemovePhysics(phys.IDFE);
            return res;
        }


        /// <summary>
        /// метод для загрузки всех фэ находящихся в списке
        /// </summary>
        /// <param name="id"></param>
        /// <returns>запись списка id которого передали параметром</returns>
        public static ListPhysics LoadPhysics(int id)
        {
            var obj = ListPhysics.Get(id);
            obj?.LoadPhysics();
            return obj;
        }

        /// <summary>
        /// метод для загрузки всех фэ находящихся в списке
        /// </summary>
        /// <param name="db_">контекст бд</param>
        public void LoadPhysics(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();

            db.Set<ListPhysics>().Attach(this);
            if (!db.Entry(this).Collection(x1 => x1.Physics).IsLoaded)
                db.Entry(this).Collection(x1 => x1.Physics).Load();

            if (db_ == null)
                db.Dispose();
        }

        /// <summary>
        /// метод для загрузки всех пользователей которым выдан список
        /// </summary>
        /// <param name="id">id списка</param>
        /// <param name="db_">контекст бд</param>
        /// <returns>список id которого передали параметром</returns>
        public static ListPhysics LoadUsers(int id, ApplicationDbContext db_ = null)
        {
            var obj = ListPhysics.Get(id);
            obj?.LoadUsers();
            return obj;
        }

        /// <summary>
        /// метод для загрузки всех пользователей которым выдан список
        /// </summary>
        /// <param name="db_">контекст бд</param>
        public void LoadUsers(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();
            db.Set<ListPhysics>().Attach(this);
            if (!db.Entry(this).Collection(x1 => x1.Users).IsLoaded)
                db.Entry(this).Collection(x1 => x1.Users).Load();

            if (db_ == null)
                db.Dispose();
        }
    }
}