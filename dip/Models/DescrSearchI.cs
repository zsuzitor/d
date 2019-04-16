using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static dip.Models.Functions;


namespace dip.Models
{
    public class DescrSearchI
    {

        public string ActionId { get; set; }
        public bool? Parametric { get; set; }
        public string ActionType { get; set; }
        public string FizVelId { get; set; }
        public string ParametricFizVelId { get; set; }
        public string ListSelectedPros { get; set; }
        public string ListSelectedSpec { get; set; }
        public string ListSelectedVrem { get; set; }

        public bool InputForm { get; set; }//вход\выход

        [ScaffoldColumn(false)]
        public bool Valide { get; private set; }//мб private

        public DescrSearchI()
        {
            Parametric = null;
            InputForm = true;
            Valide = false;
        }





        public bool? CheckParametric()
        {
            this.Parametric = AllAction.CheckParametric(this.ActionId);
            return this.Parametric;
        }

        public DescrSearchI(FEAction a)
        {

            this.ActionId = a.Name;
            this.ActionType = a.Type;
            this.FizVelId = a.FizVelId;
            this.ListSelectedPros = a.Pros;
            this.ListSelectedSpec = a.Spec;
            this.ListSelectedVrem = a.Vrem;
            this.ParametricFizVelId = a.FizVelSection;

        }



        public static bool IsNull(DescrSearchI a)
        {
            if (a == null)
                return true;
            if (string.IsNullOrWhiteSpace(a.ActionId) && string.IsNullOrWhiteSpace(a.ActionType) && string.IsNullOrWhiteSpace(a.FizVelId)
                && string.IsNullOrWhiteSpace(a.ParametricFizVelId) && string.IsNullOrWhiteSpace(a.ListSelectedPros)
                && string.IsNullOrWhiteSpace(a.ListSelectedSpec) && string.IsNullOrWhiteSpace(a.ListSelectedVrem))
                return true;


            return false;
        }

        //удаляет прямых родителей если и родитель и ребенок есть в строке
        public void DeleteNotChildCheckbox()
        {

            //pro
            this.ListSelectedPros = Pro.DeleteNotChildCheckbox(this.ListSelectedPros);

            //spec
            this.ListSelectedSpec = Spec.DeleteNotChildCheckbox(this.ListSelectedSpec);

            //vrem
            this.ListSelectedVrem = Vrem.DeleteNotChildCheckbox(this.ListSelectedVrem);


        }


        public static bool ValidationIfNeed(DescrSearchI a)
        {
            var res = a?.Valide ?? DescrSearchI.Validation(a);
            //if (res == null)
            //    res = DescrSearchIInput.Validation(a);
            return res;
        }

        public static bool Validation(DescrSearchI a)
        {
            bool res = true;
            if (a != null)
            {
                a.ActionId = NullToEmpryStr(a?.ActionId);
                if (string.IsNullOrWhiteSpace(a.ActionId))

                    res = false;

                a.ActionType = NullToEmpryStr(a?.ActionType);
                if (string.IsNullOrWhiteSpace(a.ActionType))
                    res = false;

                a.FizVelId = NullToEmpryStr(a?.FizVelId);
                if (string.IsNullOrWhiteSpace(a.FizVelId))
                    res = false;

                a.ParametricFizVelId = NullToEmpryStr(a?.ParametricFizVelId);
                a.ListSelectedPros = NullToEmpryStr(a?.ListSelectedPros);
                a.ListSelectedSpec = NullToEmpryStr(a?.ListSelectedSpec);
                a.ListSelectedVrem = NullToEmpryStr(a?.ListSelectedVrem);


                a.ListSelectedPros = Pro.SortIds(a?.ListSelectedPros);


                a.ListSelectedSpec = Spec.SortIds(a?.ListSelectedSpec);

                a.ListSelectedVrem = Vrem.SortIds(a?.ListSelectedVrem);

                //if(res)
                //a.Valide = true;
                //else
                //    a.Valide = false;
                a.Valide = res;
            }
            //a.Valide = false;
            return res;
        }
        
    }
}