/*файл класса модели представления MainHeader
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HomeV
{
    //класс-ViewModel
    public class MainHeaderV
    {
        public bool Admin { get; set; }
        public List<string> SearchList { get; set; }

        public MainHeaderV()
        {
            Admin = false;
            SearchList = null;
        }

    }
}