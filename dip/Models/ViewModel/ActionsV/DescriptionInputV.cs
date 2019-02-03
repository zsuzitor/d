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
        public DescriptionForm InputForm { get; set; }
        public DescriptionForm OutpForm { get; set; }

        public DescrSearchI InputFormData { get; set; }
        public DescrSearchI OutputFormData { get; set; }

        public string ActionParametricIds { get; set; }

        public DescriptionInputV()
        {
            InputForm = null;
            OutpForm = null;
            InputFormData = null;
            OutputFormData = null;

            ActionParametricIds = null;
        }

        public void SetAllParametricAction()
        {
            //ActionId будет 1 и тоже, поэтому берем любое
            if (InputForm != null)
            {
                ActionParametricIds = InputForm.GetAllParametricAction(); ;
            }
            else if (OutpForm != null)
                ActionParametricIds =OutpForm.GetAllParametricAction();


        }

    }
}