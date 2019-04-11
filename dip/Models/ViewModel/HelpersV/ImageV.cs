using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HelpersV
{
    public class ImageV
    {
        public ShowsImage Image { get; set; }
        public bool? ShowEmptyImage { get; set; }


        public ImageV()
        {
            Image = null;

            ShowEmptyImage = null;
        }
    }
}