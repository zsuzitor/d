﻿/*файл класса модели представления GetListSomething
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    //класс-ViewModel
    public class GetListSomethingV<T>
    {
        public List<T> List { get; set; }
        public string CurrentActionId { get; set; }
        public string Type { get; set; }
        public string ParentId { get; set; }//для редактирования

        public GetListSomethingV()
        {
            List = null;
            CurrentActionId = null;
            Type = null;
            ParentId = null;
        }
    }
}