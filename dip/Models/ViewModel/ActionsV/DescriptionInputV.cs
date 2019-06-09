using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.ActionsV
{
    //класс-ViewModel
    public class DescriptionInputV
    {
        public List<DescriptionFormWithData> InputForms { get; set; }
        public List<DescriptionFormWithData> OutpForms { get; set; }

        public int CountInput { get; set; }

        public string ActionParametricIds { get; set; }

        public DescriptionInputV()
        {
            InputForms = new List<DescriptionFormWithData>();
            OutpForms = new List<DescriptionFormWithData>();

            ActionParametricIds = null;
        }

        /// <summary>
        /// заносит в объект строку с списком всех параметрических actionid
        /// </summary>
        public void SetAllParametricAction()
        {
            //ActionId будет 1 и тоже, поэтому берем любое
            if (InputForms != null && InputForms.Count > 0)
                ActionParametricIds = InputForms[0].Form.GetAllParametricAction();

            else if (OutpForms != null && OutpForms.Count > 0)
                ActionParametricIds = OutpForms[0].Form.GetAllParametricAction();

        }

    }
}