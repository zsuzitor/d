﻿/*файл класса модели представления LoadDescr
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    //класс-ViewModel
    public class LoadDescrV
    {
        public Dictionary<string, string> DictDescrData { get; set; }
        
        public LoadDescrV()
        {
            DictDescrData = null;// new Dictionary<string, string>();

        }
    }
}