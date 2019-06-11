/*файл класса модели представления ShowSimilar
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.PhysicV
{
    //класс-ViewModel
    public class ShowSimilarV
    {
        public List<int> ListSimilarIds { get; set; }


        public ShowSimilarV()
        {
            ListSimilarIds = null;
        }
    }
}