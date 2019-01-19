using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel
{
    public class SelectedItem
    {

        public string Id { get; set; } // дескриптор
        public string Name { get; set; } // название
        public bool IsSelected { get; set; } // признак того, что характеристика выбрана

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public SelectedItem()
        {
            Id = "-1";
            Name = "";
            IsSelected = false;
        }

        /// <summary>
        /// Конструктор со всеми параметрами
        /// </summary>
        /// <param name="id"> дескриптор характеристики </param>
        /// <param name="name"> название характеристики </param>
        /// <param name="isSelected"> признак того, что характеристика выбрана </param>
        public SelectedItem(string id, string name, bool isSelected)
        {
            this.Id = id;
            this.Name = name;
            this.IsSelected = isSelected;
        }

        /// <summary>
        /// Конструктор без признака выбора характеристики
        /// </summary>
        /// <param name="id"> дескриптор характеристики </param>
        /// <param name="name"> название характеристики </param>
        public SelectedItem(string id, string name)
        {
            this.Id = id;
            this.Name = name;
            IsSelected = false;
        }



        //type : 0-Vrems   1-spec
        public static List<SelectedItem> GetListSelectedItem<T>
            (List<T> list, Models.Domain.Action action, ApplicationDbContext db, int type) 
            where T : Item
        {
            // Сортируем список характеристик
            list = list.OrderBy(pros => pros.ParentId).ToList();

            // Создаем список List<SelectedItem>
            var listSelectedPros = new List<SelectedItem>();

            // Приводим List<T> к List<SelectedItem>
            foreach (var item in list)
            {
                // Проверяем, отмечена ли характеристика в воздействии
                bool isContains = false;
                if (action != null)
                {
                    if (db == null)
                    {
                        db = new ApplicationDbContext();
                        db.Set<Models.Domain.Action>().Attach(action);
                    }
                    switch (type)
                    {
                        case 0:
                            if (!db.Entry(action).Collection(x1 => x1.Vrems).IsLoaded)
                                db.Entry(action).Collection(x1 => x1.Vrems).Load();
                            isContains = (action.Vrems.FirstOrDefault(x1 => x1.Id == item.Id) == null ? false : true);
                            break;
                        case 1:
                            if (!db.Entry(action).Collection(x1 => x1.Specs).IsLoaded)
                                db.Entry(action).Collection(x1 => x1.Specs).Load();
                            isContains = (action.Specs.FirstOrDefault(x1 => x1.Id == item.Id) == null ? false : true);
                            break;
                        case 2:
                            if (!db.Entry(action).Collection(x1 => x1.Pros).IsLoaded)
                                db.Entry(action).Collection(x1 => x1.Pros).Load();
                            isContains = (action.Pros.FirstOrDefault(x1 => x1.Id == item.Id) == null ? false : true);
                            break;
                    }


                }

                var selectedPros = new SelectedItem(item.Id, item.Name, isContains);
                listSelectedPros.Add(selectedPros);
            }

            return listSelectedPros;
        }


        //type : 0-Vrems   1-spec
        public static List<SelectedItem> GetListSelectedItem<T>
            (List<T> list, Models.Domain.Action action, int type)
            where T : Item
        {
            List<SelectedItem> res = new List<SelectedItem>();
            using (var db=new ApplicationDbContext())
            {
                res = GetListSelectedItem(list,action,db,type);
            }
            return res;
        }


    }
}