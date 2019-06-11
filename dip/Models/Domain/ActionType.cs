/*файл класса модели БД предназначенного для хранения типа входа 
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
    /// Тип входа-(внутреннее\внешнее)
    /// </summary>
    public class ActionType
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }

        //public ICollection<Action> Action { get; set; }

        public ActionType()
        {

        }
    }
}