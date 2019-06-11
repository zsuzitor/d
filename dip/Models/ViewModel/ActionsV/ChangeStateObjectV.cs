/*файл класса модели представления ChangeStateObject
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    //класс-ViewModel
    public class ChangeStateObjectV
    {
        public List<StateObject> States { get; set; }
        public string Type { get; set; }


        public CharacteristicObject Characteristics { get; set; }

        public ChangeStateObjectV()
        {
            States = new List<Domain.StateObject>();
            Type = "";
            Characteristics = new CharacteristicObject();
        }
    }
}