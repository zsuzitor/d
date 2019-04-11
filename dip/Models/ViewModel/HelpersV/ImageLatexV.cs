using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HelpersV
{
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