using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel
{
    /// <summary>
    /// класс для текстового представления дескрипторов объекта
    /// </summary>
    public class FormObjectTextRepresentation
    {
        public string StateBegin { get; set; }
        public string StateEnd { get; set; }

        public List<List<string>> PhaseBegin1 { get; set; }
        public List<List<string>> PhaseBegin2 { get; set; }
        public List<List<string>> PhaseBegin3 { get; set; }

        public List<List<string>> PhaseEnd1 { get; set; }
        public List<List<string>> PhaseEnd2 { get; set; }
        public List<List<string>> PhaseEnd3 { get; set; }


        public FormObjectTextRepresentation()
        {
            StateBegin = "";
            StateEnd = "";

            PhaseBegin1 = new List<List<string>>();
            PhaseBegin2 = new List<List<string>>();
            PhaseBegin3 = new List<List<string>>();

            PhaseEnd1 = new List<List<string>>();
            PhaseEnd2 = new List<List<string>>();
            PhaseEnd3 = new List<List<string>>();
        }


        /// <summary>
        /// метод который возвращает все id состояний которые нужно выделить(всех родителей)
        /// </summary>
        /// <param name="stateId">id выбранного состояния</param>
        /// <returns>строка id состояний для выделения разделенных ' '</returns>
        public static string DataState(string stateId)
        {
            if (string.IsNullOrWhiteSpace(stateId))
                return null;
            List<List<StateObject>> states = new List<List<StateObject>>();
            StateObject st;
            List<StateObject> list;
            using (var db = new ApplicationDbContext())
            {
                st = db.StateObjects.FirstOrDefault(x1 => x1.Id == stateId);
                if (st == null)
                    return null;
                list = st.GetParentsList(db);
            }
            list.Add(st);
            states.Add(list);
            return string.Join(" ", StateObject.GetQueueParentString(states));//["StateBegin"]

        }

        /// <summary>
        /// метод возвращает только данные(дескрипторы объекта) для отображения(список полей и чему равны) (текстовое представление)
        /// </summary>
        /// <param name="idfe"></param>
        /// <returns></returns>
        public static FormObjectTextRepresentation GetFormShow(int idfe)
        {
            FormObjectTextRepresentation res = new FormObjectTextRepresentation();

            FEText fet = FEText.Get(idfe);
            if (fet == null)
                return null;

            using (var db = new ApplicationDbContext())
            {

                res.StateBegin = FormObjectTextRepresentation.DataState(fet.StateBeginId);
                res.StateEnd = FormObjectTextRepresentation.DataState(fet.StateEndId);

                //
                var objs = db.FEObjects.Where(x1 => x1.Idfe == idfe).ToList();
                foreach (var i in objs)
                {

                    res.SetOnePhaseText(i.PhaseState.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries), db, i);
                    res.SetOnePhaseText(i.Composition.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries), db, i);
                    res.SetOnePhaseText(i.Conductivity.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries), db, i);
                    res.SetOnePhaseText(i.MagneticStructure.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries), db, i);
                    res.SetOnePhaseText(i.MechanicalState.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries), db, i);
                    res.SetOnePhaseText(i.OpticalState.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries), db, i);
                    res.SetOnePhaseText(i.Special.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries), db, i);
                }

            }


            return res;
        }


        /// <summary>
        /// метод устанавливает текстовое представление для 1й фазы в res, 
        /// </summary>
        /// <param name="mass">список id выбранных в фазе</param>
        /// <param name="db">контекст бд</param>
        /// <param name="feobj">объект фазы</param>
        public void SetOnePhaseText(string[] mass, ApplicationDbContext db, FEObject feobj)
        {
            List<List<PhaseCharacteristicObject>> d = new List<List<PhaseCharacteristicObject>>();
            List<PhaseCharacteristicObject> listForMass = db.PhaseCharacteristicObjects.Where(x1 => mass.Contains(x1.Id)).ToList();

            d = PhaseCharacteristicObject.GetQueueParent(listForMass);

            var strRes = PhaseCharacteristicObject.GetQueueParentString(d);
            if (feobj.Begin == 1)
            {
                switch (feobj.NumPhase)
                {
                    case 1:
                        this.PhaseBegin1.Add(strRes);
                        break;

                    case 2:
                        this.PhaseBegin2.Add(strRes);
                        break;

                    case 3:
                        this.PhaseBegin3.Add(strRes);
                        break;
                }
            }
            else
            {
                switch (feobj.NumPhase)
                {
                    case 1:
                        this.PhaseEnd1.Add(strRes);
                        break;

                    case 2:
                        this.PhaseEnd2.Add(strRes);
                        break;

                    case 3:
                        this.PhaseEnd3.Add(strRes);
                        break;
                }
            }
        }

    }
}