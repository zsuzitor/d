﻿/*файл класса модели БД предназначенного для хранения названия воздействия
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    /// <summary>
    /// Название воздействия(VOZN)
    /// </summary>
    public class AllAction//: Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public bool Parametric { get; set; }


        //public ICollection<Action> Actions { get; set; }

        public AllAction()
        {
            // Actions = new List<Action>();
            Parametric = false;
        }

        /// <summary>
        /// Получить запись по id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>воздействие</returns>
        public static AllAction Get(string id)
        {
            AllAction res = null;
            if (!string.IsNullOrWhiteSpace(id))
                using (var db = new ApplicationDbContext())
                {
                    res = db.AllActions.FirstOrDefault(x1 => x1.Id == id);
                }
            return res;

        }
        /// <summary>
        /// Проверить является ли запись параметрической
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true-если параметрическое</returns>
        public static bool? CheckParametric(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;
            using (var db = new ApplicationDbContext())
                return db.AllActions.FirstOrDefault(x1 => x1.Id == id)?.Parametric;

        }


        /// <summary>
        /// проверяет есть ли фэ которые используют что то из списка(грузит детей и тд) и если хотя бы 1 итем блокируется не удаляет ничего
        /// </summary>
        /// <param name="db">контекст</param>
        /// <param name="list">список записей для удаления</param>
        /// <returns>id записей которые блокируют удаление</returns>
        public static List<int> TryDeleteWithChilds(ApplicationDbContext db, List<AllAction> list)//TODO вынести
        {
            List<int> blockFe = new List<int>();

            if (list.Count == 0)
                return blockFe;
            foreach (var i in list)
            {
                blockFe.AddRange(db.FEActions.Where(x1 => x1.Name == i.Id).Select(x1 => x1.Idfe).ToList());

                var fizvel = db.FizVels.Where(x1 => x1.Parent == i.Id + "_FIZVEL").ToList();
                if (i.Parametric)
                {
                    List<string> fizveldtr = fizvel.Select(x1 => x1.Id).ToList();
                    fizvel.AddRange(db.FizVels.Where(x1 => fizveldtr.FirstOrDefault(x2 => x2 == x1.Parent) != null).ToList());
                }
                else
                {
                    var pros = db.Pros.Where(x1 => x1.Parent == i.Id + "_PROS").Select(x1 => x1.Id).ToList();
                    blockFe.AddRange(Pro.TryDeleteWithChilds(db, db.Pros.Where(x1 => pros.FirstOrDefault(x2 => x2 == x1.Id) != null).ToList()));
                    var spec = db.Specs.Where(x1 => x1.Parent == i.Id + "_SPEC").Select(x1 => x1.Id).ToList();
                    blockFe.AddRange(Spec.TryDeleteWithChilds(db, db.Specs.Where(x1 => spec.FirstOrDefault(x2 => x2 == x1.Id) != null).ToList()));
                    var vrem = db.Vrems.Where(x1 => x1.Parent == i.Id + "_VREM").Select(x1 => x1.Id).ToList();
                    blockFe.AddRange(Vrem.TryDeleteWithChilds(db, db.Vrems.Where(x1 => vrem.FirstOrDefault(x2 => x2 == x1.Id) != null).ToList()));
                }
                blockFe.AddRange(FizVel.TryDelete(db, fizvel));
            }
            if (blockFe.Count == 0)
            {
                db.AllActions.RemoveRange(list);
                db.SaveChanges();

            }

            return blockFe;
        }
    }
}