﻿/*файл класса модели представления PartFormStateObject
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HelpersV
{
    //класс-ViewModel
    public class PartFormStateObjectV
    {
        public StateObject State { get; set; }
        public string Type { get; set; }
        public string Param { get; set; }


        public PartFormStateObjectV()
        {

        }
    }
}