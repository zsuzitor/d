/*файл класса модели представления GetStateObject
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
    public class GetStateObjectV
    {
        public List<StateObject> List { get; set; }
        public string Type { get; set; }


        public GetStateObjectV()
        {
            List = new List<StateObject>();
        }
    }
}