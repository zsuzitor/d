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


    }
}