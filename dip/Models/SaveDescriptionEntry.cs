using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models
{
    /// <summary>
    /// класс для сохранения дескрипторов
    /// </summary>
    public class SaveDescriptionEntry
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Text { get; set; }
        public bool Parametric { get; set; }


        public SaveDescriptionEntry()
        {
        }
    }
}