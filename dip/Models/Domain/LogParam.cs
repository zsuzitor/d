/*файл класса модели БД предназначенного для хранения параметра лога
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    /// <summary>
    /// класс для хранения параметра лога
    /// </summary>
    public class LogParam
    {
        public int Id { get; set; }
        public string Param { get; set; }
        public string Name { get; set; }

        public int LogId { get; set; }
        public Log Log { get; set; }

        public LogParam()
        {

        }
    }
}