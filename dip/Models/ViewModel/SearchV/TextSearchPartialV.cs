/*файл класса модели представления TextSearchPartial
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.SearchV
{
    //класс-ViewModel
    public class TextSearchPartialV
    {
        public int CountLoad { get; set; }
        public List<int> ListPhysId { get; set; }


        public TextSearchPartialV()
        {
            CountLoad = 0;
            ListPhysId = null;
        }
    }
}