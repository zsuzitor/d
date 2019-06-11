/*файл класса модели представления ObjectInput
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
    public class ObjectInputV
    {
        public List<StateObject> StatesBegin { get; set; }
        public string StateBeginSelected { get; set; }

        public bool changedObject { get; set; }

        public List<StateObject> StatesEnd { get; set; }
        public string StateEndSelected { get; set; }

        public CharacteristicObject CharacteristicsBegin { get; set; }


        public CharacteristicObject CharacteristicsEnd { get; set; }



        public ObjectInputV()
        {
            StateBeginSelected = "";
            StateEndSelected = "";
            changedObject = false;
            CharacteristicsBegin = new CharacteristicObject();
            CharacteristicsEnd = new CharacteristicObject();
        }


        /// <summary>
        /// метод для определения списка состояний которые необходимо выделить, и для загрузки первого уровня характеристик для выделенного состояния
        /// </summary>
        /// <param name="stateIdBegin">id состояния текущего</param>
        /// <param name="basePhase">список базовых состояний</param>
        /// <param name="States">состояния с которыми будем взаимодействовать</param>
        /// <param name="Characteristics">характеристики</param>
        /// <param name="StateSelected">id состояний которые надо выделить на форме разделенные ' '</param>
        public void StatesBeginFirstLvlPhase(string stateIdBegin, List<PhaseCharacteristicObject> basePhase,
            List<StateObject> States, CharacteristicObject Characteristics, ref string StateSelected)
        {
            StateObject state = StateObject.Get(stateIdBegin);
            if (state != null)
            {
                //int countPhase;
                var massparent = state.GetParentsList();
                foreach (var asd in States)
                {
                    if (massparent.FirstOrDefault(x1 => x1.Id == asd.Id) == null)
                    {
                        if (asd.Id == state.Id)
                        {
                            asd.LoadPartialTree(massparent);
                        }
                        else
                            continue;
                    }
                    asd.LoadPartialTree(massparent);
                }
                StateSelected = string.Join(" ", massparent.Select(x1 => x1.Id).ToList());
                StateSelected += " " + state.Id;
                //res.StateBeginSelected=state.LoadPartialTree(res.StatesBegin);//, out countPhase
                Characteristics.SetFirstLvlStates(state.CountPhase, basePhase);

            }
        }
    }
}