/*файл класса модели представления ListFeText
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.PhysicV
{
    //класс-ViewModel
    public class ListFeTextV
    {
        public List<FEText> FeTexts { get; set; }
        public int NumLoad { get; set; }

        public ListFeTextV()
        {
            FeTexts = new List<FEText>();
            NumLoad = 0;
        }
    }
}