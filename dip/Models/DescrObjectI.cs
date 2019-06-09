using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models
{

    /// <summary>
    /// класс для хранения 3х фаз объекта(характеристики)
    /// </summary>
    public class DescrObjectI
    {
        public DescrPhaseI ListSelectedPhase1 { get; set; }
        public DescrPhaseI ListSelectedPhase2 { get; set; }
        public DescrPhaseI ListSelectedPhase3 { get; set; }
        public int Length
        {
            get
            {
                return this.GetCountPhase();
            }
        }
        public bool Begin { get; set; }//начальное или конечное состояние
                                       //public int NumPhase { get; set; }

        public bool Valide { get; private set; }
        public DescrObjectI()
        {
            Begin = true;
            ListSelectedPhase1 = null;
            ListSelectedPhase2 = null;
            ListSelectedPhase3 = null;
            Valide = false;
        }

        /// <summary>
        /// индексатор
        /// </summary>
        /// <param name="index">индекс</param>
        /// <returns></returns>
        public DescrPhaseI this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return ListSelectedPhase1;
                    case 1:
                        return ListSelectedPhase2;
                    case 2:
                        return ListSelectedPhase3;
                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        ListSelectedPhase1 = value;
                        break;
                    case 1:
                        ListSelectedPhase2 = value;
                        break;
                    case 2:
                        ListSelectedPhase3 = value;
                        break;
                }
            }
        }

        /// <summary>
        /// метод для перебора foreach
        /// </summary>
        /// <returns></returns>
        public IEnumerator<DescrPhaseI> GetEnumerator()
        {

            yield return ListSelectedPhase1;
            yield return ListSelectedPhase2;
            yield return ListSelectedPhase3;
        }

        /// <summary>
        /// метод для определения количества фаз
        /// </summary>
        /// <returns>количество фаз</returns>
        public int GetCountPhase()
        {
            int res = 0;
            if (this.ListSelectedPhase1 != null)
            {
                ++res;
                if (this.ListSelectedPhase2 != null)
                {
                    ++res;
                    if (this.ListSelectedPhase3 != null)
                        ++res;
                }
            }
            return res;
        }

        /// <summary>
        /// метод для получения всех фаз списком без null
        /// </summary>
        /// <returns>список фаз</returns>
        public List<DescrPhaseI> GetActualPhases()
        {
            List<DescrPhaseI> res = new List<DescrPhaseI>();
            if (this.ListSelectedPhase1 != null)
            {
                res.Add(this.ListSelectedPhase1);
                if (this.ListSelectedPhase2 != null)
                {
                    res.Add(this.ListSelectedPhase2);
                    if (this.ListSelectedPhase3 != null)
                    {
                        res.Add(this.ListSelectedPhase3);
                    }
                }
            }
            return res;
        }


        /// <summary>
        /// метод для получения списка строк(каждая для каждой фазы) содержащих id всех чекбоксов в фазе
        /// </summary>
        /// <returns>список строк</returns>
        public List<string> GetList_()
        {
            List<string> res = new List<string>()
            {
                ListSelectedPhase1?.GetListStr_(),
                ListSelectedPhase2?.GetListStr_(),
                ListSelectedPhase3?.GetListStr_()
            };
            return res;
        }


        /// <summary>
        ///  метод для удаления прямых родителей если и родитель и ребенок есть в строке. вернет строку содержащую только id записей у которых нет детей
        /// </summary>
        public void DeleteNotChildCheckbox()
        {
            ListSelectedPhase1?.DeleteNotChildCheckbox();
            ListSelectedPhase2?.DeleteNotChildCheckbox();
            ListSelectedPhase3?.DeleteNotChildCheckbox();

        }

        /// <summary>
        /// метод для валидации
        /// </summary>
        /// <param name="a">объект валидации</param>
        /// <returns>флаг успеха</returns>
        public static bool Validation(DescrObjectI a)
        {
            bool res = true;
            if (a != null)
            {
                DescrPhaseI.Validation(a.ListSelectedPhase1);
                DescrPhaseI.Validation(a.ListSelectedPhase2);
                DescrPhaseI.Validation(a.ListSelectedPhase3);
            }
            a.Valide = res;
            return res;
        }
    }
}