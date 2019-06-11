/*файл класса модели представления PartFormCharacteristicObject
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
    public class PartFormCharacteristicObjectV
    {
        public CharacteristicObject Characteristic { get; set; }
        public string Type { get; set; }


        public PartFormCharacteristicObjectV()
        {

        }
    }
}