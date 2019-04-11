using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WpfMath;

namespace dip.Models.Domain
{
    public class FELatexFormula: ShowsFEImage
    {
        public int Id { get; set; }
        public string Formula { get; set; }
        public int FeTextIDFE { get; set; }
        public FEText FeText { get; set; }

        [NotMapped]
        public byte[] Data { get; set; }

        [NotMapped]
        public string IdForShow { get {
                return Id + "lateximg";
            } }

        public FELatexFormula()
        {
            Data = null;
            FeTextIDFE = 0;
            FeText = null;
            Formula = null;
        }

        public void SetBytes()
        {
            try
            {
                var parser = new TexFormulaParser();
                var formulabyte = parser.Parse(Formula ?? "");
                this.Data = formulabyte.RenderToPng(20.0, 0.0, 0.0, "Arial");

            }
            catch
            {
                this.Data = null;
            }

        }



    }
}