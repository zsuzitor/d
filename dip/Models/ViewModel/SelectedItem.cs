using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel
{
    public class SelectedItem
    {

        public string id { get; set; } // дескриптор
        public string name { get; set; } // название
        public bool isSelected { get; set; } // признак того, что характеристика выбрана

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public SelectedItem()
        {
            id = "-1";
            name = "";
            isSelected = false;
        }

        /// <summary>
        /// Конструктор со всеми параметрами
        /// </summary>
        /// <param name="id"> дескриптор характеристики </param>
        /// <param name="name"> название характеристики </param>
        /// <param name="isSelected"> признак того, что характеристика выбрана </param>
        public SelectedItem(string id, string name, bool isSelected)
        {
            this.id = id;
            this.name = name;
            this.isSelected = isSelected;
        }

        /// <summary>
        /// Конструктор без признака выбора характеристики
        /// </summary>
        /// <param name="id"> дескриптор характеристики </param>
        /// <param name="name"> название характеристики </param>
        public SelectedItem(string id, string name)
        {
            this.id = id;
            this.name = name;
            isSelected = false;
        }


    }
}