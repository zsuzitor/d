/*файл класса изображения для модели представления 
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
    public class ImageV
    {
        public IShowsImage Image { get; set; }
        public bool? ShowEmptyImage { get; set; }


        public ImageV()
        {
            Image = null;

            ShowEmptyImage = null;
        }
    }
}