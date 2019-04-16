using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models
{
    public class ChangeLatex
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Action { get; set; }//0-добавление, 1- изменение 2- удаление

        public ChangeLatex()
        {

        }

    }
}