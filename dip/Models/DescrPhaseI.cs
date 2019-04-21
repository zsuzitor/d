﻿using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static dip.Models.Functions;

namespace dip.Models
{

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
                //NumPhase = 1;
            }

        }

        public string GetListStr_()//TODO
        {
            string res = "";
            res += PhaseState + " " +
                Composition + " " +
                MagneticStructure + " " +
                Conductivity + " " +
                MechanicalState + " " +
                OpticalState + " " +
                Special + " ";


            return res;
        }

        //удаляет прямых родителей если и родитель и ребенок есть в строке
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

        public static bool Validation(DescrPhaseI a)
        {
            if (a != null)
            {
                // a.DeleteNotChildCheckbox();
                //TODO валидация
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


        public bool SortIds()//TODO
        {
            bool res = true;
            //if (string.IsNullOrWhiteSpace(PhaseState))
            //    return false;
            //// var gg = ids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            //string resStr = string.Join(" ", PhaseState.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
            //         OrderBy(x1 => x1).Distinct().ToList());


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