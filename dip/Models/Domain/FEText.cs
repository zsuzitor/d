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
        [DataType(DataType.MultilineText)]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        [DataType(DataType.MultilineText)]
        public string TextInp { get; set; }
        [DataType(DataType.MultilineText)]
        public string TextOut { get; set; }
        [DataType(DataType.MultilineText)]
        public string TextObj { get; set; }
        [DataType(DataType.MultilineText)]
        public string TextApp { get; set; }
        [DataType(DataType.MultilineText)]
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