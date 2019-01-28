using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.HelpersV
{
    public class ImageV
    {
        public Image Image { get; set; }
        public bool? Show_empty_img { get; set; }


        public ImageV()
        {
            Image = null;

            Show_empty_img = null;
        }
    }
}