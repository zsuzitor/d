using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WpfMath;

namespace dip.Models.Domain
{

    /// <summary>
    /// класс для хранения изображений ФЭ
    /// </summary>
    public class Image : IShowsFEImage
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public int FeTextIDFE { get; set; }
        public FEText FeText { get; set; }

        [NotMapped]
        public string IdForShow
        {
            get
            {
                return Id + "img";
            }
        }
        public Image()
        {
            Data = null;
            FeTextIDFE = 0;
            FeText = null;
        }

        /// <summary>
        /// метод для получения изображения из latex формулы
        /// </summary>
        /// <param name="formula">объект FELatexFormula</param>
        /// <returns>запись изображения</returns>
        public static Image GetFromLatex(FELatexFormula formula)
        {
            return GetFromLatex(formula.Formula);
        }

        /// <summary>
        /// метод для получения изображения из latex формулы
        /// </summary>
        /// <param name="formula">текст формулы</param>
        /// <returns>запись изображения</returns>
        public static Image GetFromLatex(string formula)
        {
            try
            {
                var parser = new TexFormulaParser();
                var formulabyte = parser.Parse(formula);
                return new Image() { Data = formulabyte.RenderToPng(20.0, 0.0, 0.0, "Arial") };
            }
            catch { }
            return null;
        }
    }
}