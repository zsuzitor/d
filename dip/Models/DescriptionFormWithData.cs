﻿/*файл класса для хранения дескрипторной формы (вход и выход)
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using dip.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models
{
    /// <summary>
    /// класс для хранения дескрипторной формы(вход+выход)
    /// </summary>
    public class DescriptionFormWithData
    {

        public DescriptionForm Form { get; set; }
        public DescrSearchI FormData { get; set; }


        public DescriptionFormWithData()
        {

        }
    }
}