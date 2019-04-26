using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static dip.Models.Functions;

namespace dip.Models
{
    /// <summary>
    /// 1 фаза объекта, данные(класс для view)
    /// </summary>
    public class DescrPhaseI : FEObject
    {

        public DescrPhaseI()
        {

        }
        public DescrPhaseI(FEObject a)
        {
            if (a != null)
            {
                Id = a.Id;
                Idfe = a.Idfe;
                Begin = a.Begin;
                NumPhase = a.NumPhase;

                PhaseState = a.PhaseState;
                Composition = a.Composition;
                MagneticStructure = a.MagneticStructure;
                Conductivity = a.Conductivity;
                MechanicalState = a.MechanicalState;
                OpticalState = a.OpticalState;
                Special = a.Special;
            }
        }

        /// <summary>
        /// метод для получения строки, содержащей id всех чекбоксов в фазе
        /// </summary>
        /// <returns></returns>
        public string GetListStr_()//TODO
        {
            string res = "";
            if (!string.IsNullOrWhiteSpace(PhaseState))
                res += PhaseState + " ";
            if (!string.IsNullOrWhiteSpace(Composition))
                res += Composition + " ";
            if (!string.IsNullOrWhiteSpace(MagneticStructure))
                res += MagneticStructure + " ";
            if (!string.IsNullOrWhiteSpace(Conductivity))
                res += Conductivity + " ";
            if (!string.IsNullOrWhiteSpace(MechanicalState))
                res += MechanicalState + " ";
            if (!string.IsNullOrWhiteSpace(OpticalState))
                res += OpticalState + " ";
            if (!string.IsNullOrWhiteSpace(Special))
                res += Special + " ";
            return res.Trim();
        }


        /// <summary>
        ///  метод для удаления прямых родителей если и родитель и ребенок есть в строке. вернет строку содержащую только id записей у которых нет детей
        /// </summary>
        public void DeleteNotChildCheckbox()
        {
            this.PhaseState = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.PhaseState);
            this.Composition = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.Composition);
            this.MagneticStructure = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.MagneticStructure);
            this.Conductivity = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.Conductivity);
            this.MechanicalState = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.MechanicalState);
            this.OpticalState = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.OpticalState);
            this.Special = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.Special);

        }

        /// <summary>
        /// метод для валидации
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool Validation(DescrPhaseI a)
        {
            if (a != null)
            {
                a.PhaseState = NullToEmpryStr(a?.PhaseState);
                a.Composition = NullToEmpryStr(a?.Composition);
                a.MagneticStructure = NullToEmpryStr(a?.MagneticStructure);
                a.Conductivity = NullToEmpryStr(a?.Conductivity);
                a.MechanicalState = NullToEmpryStr(a?.MechanicalState);
                a.OpticalState = NullToEmpryStr(a?.OpticalState);
                a.Special = NullToEmpryStr(a?.Special);
                a.SortIds();
            }
            return true;
        }


        /// <summary>
        /// сортирует все id в фазе
        /// </summary>
        /// <returns></returns>
        public bool SortIds()//TODO
        {
            bool res = true;
            this.PhaseState = PhaseCharacteristicObject.SortIds(this.PhaseState);
            this.Composition = PhaseCharacteristicObject.SortIds(this.Composition);
            this.MagneticStructure = PhaseCharacteristicObject.SortIds(this.MagneticStructure);
            this.Conductivity = PhaseCharacteristicObject.SortIds(this.Conductivity);
            this.MechanicalState = PhaseCharacteristicObject.SortIds(this.MechanicalState);
            this.OpticalState = PhaseCharacteristicObject.SortIds(this.OpticalState);
            this.Special = PhaseCharacteristicObject.SortIds(this.Special);
            return res;
        }
    }
}