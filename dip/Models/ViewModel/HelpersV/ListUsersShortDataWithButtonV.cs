﻿/*файл класса модели представления ListUsersShortDataWithButton
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HelpersV
{
    //класс-ViewModel
    public class ListUsersShortDataWithButtonV
    {
        public List<ApplicationUser> Users { get; set; }
        public string ButText { get; set; }


        public ListUsersShortDataWithButtonV()
        {

        }
    }
}