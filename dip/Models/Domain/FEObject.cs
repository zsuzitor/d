/*файл класса модели БД предназначенного для хранения одной фазы объекта
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{

    /// <summary>
    /// 1 фаза объекта, данные
    /// </summary>
    public class FEObject
    {

        public int Id { get; set; }
        public int Idfe { get; set; }
        public int Begin { get; set; }
        public int NumPhase { get; set; }

        public string PhaseState { get; set; }//type-checkbox
        public string Composition { get; set; }//type-checkbox
        public string MagneticStructure { get; set; }//type-checkbox
        public string Conductivity { get; set; }//type-checkbox
        public string MechanicalState { get; set; }//type-checkbox
        public string OpticalState { get; set; }//type-checkbox
        public string Special { get; set; }//type-checkbox

        public FEObject()
        {
            NumPhase = 1;
            Begin = 1;
            PhaseState = "";
            Composition = "";
            MagneticStructure = "";
            Conductivity = "";
            MechanicalState = "";
            OpticalState = "";
            Special = "";

        }
        public FEObject(DescrPhaseI a, int idfe, int begin)
        {
            this.Idfe = idfe;
            this.Begin = begin;
            this.NumPhase = a.NumPhase;

            PhaseState = a.PhaseState;
            Composition = a.Composition;
            MagneticStructure = a.MagneticStructure;
            Conductivity = a.Conductivity;
            MechanicalState = a.MechanicalState;
            OpticalState = a.OpticalState;
            Special = a.Special;

        }

        public FEObject(DescrPhaseI a) : this(a, a.Idfe, a.Begin)
        {

        }

        /// <summary>
        /// Метод для получения начальных и конечных характеристик фэ
        /// </summary>
        /// <param name="id">id фэ</param>
        /// <param name="inp">начальные характеристики</param>
        /// <param name="outp">конечные характеристики</param>
        /// <returns>флаг успеха</returns>
        public static bool Get(int id, ref List<FEObject> inp, ref List<FEObject> outp)
        {
            using (var db = new ApplicationDbContext())
            {
                var lst = db.FEObjects.Where(x1 => x1.Idfe == id).ToList();
                inp = lst.Where(x1 => x1.Begin == 1).ToList();
                outp = lst.Where(x1 => x1.Begin == 0).ToList();
            }
            return true;
        }

    }
}