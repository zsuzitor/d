/*файл класса модели представления ChangeRole
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.AdminV
{
    //класс-ViewModel
    public class ChangeRoleV
    {
        public List<string> Roles { get; set; }

        public ChangeRoleV()
        {
            Roles = new List<string>();
        }
    }
}