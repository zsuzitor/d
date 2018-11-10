using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class FEText
    {
        [Key]
        public int IDFE { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string TextInp { get; set; }
        public string TextOut { get; set; }
        public string TextObj { get; set; }
        public string TextApp { get; set; }
        public string TextLit { get; set; }

        public ICollection<Image> Images { get; set; }

        public FEText()
        {
            this.Images = new List<Image>();
        }

        public bool Equal (FEText a)
        {
            this.Name = a.Name;
            this.Text = a.Text;
            this.TextInp = a.TextInp;
            this.TextOut = a.TextOut;
            this.TextObj = a.TextObj;
            this.TextApp = a.TextApp;
            this.TextLit = a.TextLit;

            
            return true;
        }

        public bool EqualWithId(FEText a)
        {
            this.Equal(a);
            this.IDFE = a.IDFE;

            return true;
        }
    }
}