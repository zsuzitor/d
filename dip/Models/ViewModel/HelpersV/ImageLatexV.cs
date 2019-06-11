/*файл класса модели представления ImageLatex
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HelpersV
{
    //класс-ViewModel
    public class ImageLatexV
    {
        public FELatexFormula Formula { get; set; }
        public bool? ShowEmptyFormula { get; set; }
        public ImageLatexV()
        {
            Formula = null;

            ShowEmptyFormula = null;
        }
    }
}