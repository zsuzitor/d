/*файл класса модели представления ListPhysInItem
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
    public class ListPhysInItemV
    {
        public ListPhysics Item { get; set; }
        public int Idlist { get; set; }

        public ListPhysInItemV()
        {

        }
    }
}