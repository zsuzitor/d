/*файл класса модели представления DescriptionSearch
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.SearchV
{
    //класс-ViewModel
    public class DescriptionSearchV
    {

        public int[] SearchList { get; set; }
        public List<DescrSearchI> FormInput { get; set; }
        public List<DescrSearchI> FormOutput { get; set; }

        public DescrObjectI FormObjectBegin { get; set; }
        public DescrObjectI FormObjectEnd { get; set; }

        public DescriptionSearchV()
        {
            SearchList = null;
            FormInput = null;
            FormOutput = null;
            FormObjectBegin = null;
            FormObjectEnd = null;
        }
    }
}