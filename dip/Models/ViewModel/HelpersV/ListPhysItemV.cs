/*файл класса модели представления ListPhysItem
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HelpersV
{
    //класс-ViewModel
    public class ListPhysItemV
    {

        public ListPhysics ListPhysic { get; set; }
        public bool Select { get; set; }
        public ListPhysItemV()
        {
            Select = false;
        }
    }
}