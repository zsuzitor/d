/*файл класса модели представления Details
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using dip.Models.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.PhysicV
{
    //класс-ViewModel
    public class DetailsV
    {
        public string EffectName { get; set; }
        public bool Admin { get; set; }
        public FEText Effect { get; set; }
        public bool? Favourited { get; set; }

        public List<IShowsImage> AllImages { get; set; }


        public DetailsV()
        {
            Favourited = null;
            EffectName = null;
            Admin = false;
            Effect = null;
            AllImages = new List<IShowsImage>();


        }

        /// <summary>
        ///  метод для подготовки записи ФЭ к отображению
        /// </summary>
        /// <param name="id">id ФЭ</param>
        /// <param name="HttpContext">контекст http</param>
        public void Data(int? id, HttpContextBase HttpContext)//bool go
        {

            FEText phys = FEText.GetIfAccess(id, HttpContext);
            Data(phys, HttpContext);
        }

        /// <summary>
        /// метод для подготовки записи ФЭ к отображению без проверок
        /// </summary>
        /// <param name="phys">записи ФЭ</param>
        /// <param name="HttpContext">контекст http</param>
        public void Data(FEText phys, HttpContextBase HttpContext)
        {
            if (phys == null)
                throw new Exception("Запись с данным id не найдена");
            Effect = phys;
            string check_id = ApplicationUser.GetUserId();

            Effect.LoadImage();
            Effect.AddByteToLatexImages();
            this.SetListAllImages();
            EffectName = Effect.Name;


            if (check_id != null)
            {
                ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>();
                IList<string> roles = userManager?.GetRoles(check_id);
                if (roles != null)
                    if (roles.Contains("admin"))
                        Admin = true;
                Favourited = Effect.Favourited(check_id);
            }
        }

        /// <summary>
        /// устанавливает лист со всеми изображениями+latex изображениями
        /// </summary>
        public void SetListAllImages()
        {
            if (this.Effect?.Images != null)
                this.AllImages.AddRange(this.Effect.Images);
            if (this.Effect?.LatexFormulas != null)
                this.AllImages.AddRange(this.Effect.LatexFormulas);
        }

    }
}