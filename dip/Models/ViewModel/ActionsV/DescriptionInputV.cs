using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    public class DescriptionInputV
    {
        //ActionId будет 1 и тоже
        //public List<DescriptionForm> InputForm { get; set; }
        //public List<DescriptionForm> OutpForm { get; set; }

        //public DescrSearchI InputFormData { get; set; }
        //public DescrSearchI OutputFormData { get; set; }

            public List<DescriptionFormWithData> InputForms{ get; set; }
        public List<DescriptionFormWithData> OutpForms { get; set; }

        public string ActionParametricIds { get; set; }

        public DescriptionInputV()
        {
            InputForms = new List<DescriptionFormWithData>() ;
            OutpForms = new List<DescriptionFormWithData>();
            //InputFormData = null;
            //OutputFormData = null;

            ActionParametricIds = null;
        }

        /// <summary>
        /// заносит в объект строку с списком всех параметрических actionid
        /// </summary>
        public void SetAllParametricAction()
        {
            //ActionId будет 1 и тоже, поэтому берем любое
            if (InputForms != null&& InputForms.Count>0)
                ActionParametricIds = InputForms[0].Form.GetAllParametricAction(); 
            
            else if (OutpForms != null && OutpForms.Count > 0)
                ActionParametricIds =OutpForms[0].Form.GetAllParametricAction();


        }

    }
}