using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    public class ObjectInputV
    {
        public List<StateObject> StatesBegin { get; set; }
        public string StateBeginSelected { get; set; }

        public bool changedObject { get; set; }

        public List<StateObject> StatesEnd { get; set; }
        public string StateEndSelected { get; set; }

        public CharacteristicObject CharacteristicsBegin { get; set; }
        //public List<PhaseCharacteristicObject> CharacteristicStart1 { get; set; }
        //    public List<PhaseCharacteristicObject> CharacteristicStart2 { get; set; }
        //    public List<PhaseCharacteristicObject> CharacteristicStart3 { get; set; }

        public CharacteristicObject CharacteristicsEnd { get; set; }
        //public List<PhaseCharacteristicObject> CharacteristicEnd1 { get; set; }
        //public List<PhaseCharacteristicObject> CharacteristicEnd2 { get; set; }
        //public List<PhaseCharacteristicObject> CharacteristicEnd3 { get; set; }


        public ObjectInputV()
        {
            StateBeginSelected = "";
            StateEndSelected = "";
            changedObject = false;
            CharacteristicsBegin = new CharacteristicObject();
            CharacteristicsEnd = new CharacteristicObject();
        }



        public void StatesBeginFirstLvlPhase(string stateIdBegin, List<PhaseCharacteristicObject> basePhase,
            List<StateObject> States,CharacteristicObject Characteristics, ref string StateSelected)
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