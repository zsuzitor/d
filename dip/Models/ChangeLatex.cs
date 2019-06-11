/*файл класса для хранения измененных формул latex
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models
{
    /// <summary>
    /// класс для хранения измененных формул latex
    /// </summary>
    public class ChangeLatex
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Action { get; set; }//0-добавление, 1- изменение 2- удаление

        public ChangeLatex()
        {

        }

    }
}