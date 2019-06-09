using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.PhysicV
{
    //класс-ViewModel
    public class CreateDescriptionV
    {
        public DescriptionForm Form { get; set; }
        public string ActionParametricIds { get; set; }

        /// <summary>
        /// загружает список все параметрических воздействий загруженных в  форму, одной строкой
        /// </summary>
        public void SetAllParametricAction()
        {
            //ActionId будет 1 и тоже, поэтому берем любое
            if (Form != null)

                ActionParametricIds = Form.GetAllParametricAction();

        }
    }
}