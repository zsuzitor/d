using dip.Models.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.PhysicV
{
    public class DetailsV
    {
        public string EffectName { get; set; }
        //public string TechnicalFunctionId { get; set; }
        public bool Admin { get; set; }
        public FEText Effect { get; set; }
        public bool? Favourited { get; set; }
        //public byte[] Lat { get; set; }

        public List<ShowsFEImage> AllImages { get; set; }


        public DetailsV()
        {
            Favourited = null;
            EffectName = null;
            //TechnicalFunctionId = null;
            Admin = false;
            Effect = null;
            AllImages = new List<ShowsFEImage>();


        }

        public void Data(int?id,HttpContextBase HttpContext)//bool go
        {

            FEText phys = FEText.GetIfAccess(id, HttpContext);
            Data(phys, HttpContext);
        }

        /// <summary>
        /// без проверок
        /// </summary>
        /// <param name="phys"></param>
        /// <param name="HttpContext"></param>
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

        public void SetListAllImages()
        {
            if(this.Effect?.Images!=null)
            this.AllImages.AddRange(this.Effect.Images);
            if (this.Effect?.LatexFormulas != null)
                this.AllImages.AddRange(this.Effect.LatexFormulas);
        }

        //public void GetModel(FEText phys)
        //{

        //}
    }
}