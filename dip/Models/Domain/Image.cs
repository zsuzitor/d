using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WpfMath;

namespace dip.Models.Domain
{
    public class Image: ShowsFEImage
    {
        public int Id { get; set; }

        public byte[] Data { get; set; }

        public int FeTextIDFE { get; set; }
        public FEText FeText { get; set; }

        [NotMapped]
        public string IdForShow { get {
                return Id + "img";
            }  }
        public Image()
        {
            Data = null;
            FeTextIDFE = 0;
            FeText = null;
            //IdForShow = "";
        }


        public static Image GetFromLatex(FELatexFormula formula)
        {
            try
            {
                var parser = new TexFormulaParser();
                var formulabyte = parser.Parse(formula.Formula);
                return new Image() { Data = formulabyte.RenderToPng(20.0, 0.0, 0.0, "Arial") };

            }
            catch
            {
               
            }
            return null;
            
           
        }

        public static Image GetFromLatex(string formula)
        {
            try
            {
                var parser = new TexFormulaParser();
                var formulabyte = parser.Parse(formula);
                return new Image() { Data = formulabyte.RenderToPng(20.0, 0.0, 0.0, "Arial") };

            }
            catch
            {
                
            }
            return null;
        }


    }
}