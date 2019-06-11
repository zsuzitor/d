/*файл класса модели представления PersonalRecord
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.PersonV
{
    //класс-ViewModel
    public class PersonalRecordV
    {
        public string CurrenUserId { get; set; }
        public ApplicationUser User { get; set; }
        RolesProject[] RoleMax { get; set; }

        public bool Admin { get; set; }

        public PersonalRecordV()
        {
            CurrenUserId = null;
            User = null;
            RoleMax = null;
            Admin = false;
        }
    }
}