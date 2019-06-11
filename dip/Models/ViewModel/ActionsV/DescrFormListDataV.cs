/*файл класса модели представления DescrFormListData
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    //класс-ViewModel
    public class DescrFormListDataV<T>
    {
        public List<T> List { get; set; }
        
        public string ParentId { get; set; }//для редактирования формы

        public DescrFormListDataV()
        {
            List = null;

            ParentId = "";
        }
    }
}